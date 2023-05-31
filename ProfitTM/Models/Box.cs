using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProfitTM.Models
{
    public class Box : Boxes
    {
        public string Descrip { get; set; }
        public string Sup { get; set; }
        public int BoxType { get; set; }

        public static Boxes GetBoxByID(string id)
        {
            Boxes box;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                box = db.Boxes.AsNoTracking().First(b => b.ID.ToString() == id);
            }
            catch (Exception ex)
            {
                box = null;
                Incident.CreateIncident("ERROR BUSCANDO CAJA " + id, ex);
            }

            return box;
        }

        public static List<Box> GetAllBoxesAndMoves()
        {
            List<Box> boxes;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();

                boxes = (from b in db.Boxes.AsNoTracking()
                         join u in db.Users.AsNoTracking() on b.UserID equals u.Username
                         select new Box()
                         {
                             ID = b.ID,
                             UserID = b.UserID,
                             Descrip = u.Descrip,
                             DateS = b.DateS,
                             DateE = b.DateE,
                             AmountInit = b.AmountInit,
                             Incomes = b.Incomes,
                             Expenses = b.Expenses,
                             Sales = b.Sales,
                             IsOpen = b.IsOpen,
                             Sup = u.SupID.ToString(),
                             BoxType = u.BoxType ?? 0

                         }).ToList();

                foreach (Box box in boxes)
                {
                    box.Sup = box.Sup != "" ? User.GetUserByID(box.Sup).Username : null;
                    box.BoxMoves = db.BoxMoves.AsNoTracking().Where(m => m.BoxID == box.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                boxes = null;
                Incident.CreateIncident("ERROR BUSCANDO CAJAS Y MOVIMIENTOS", ex);
            }
            
            return boxes;
        }

        public static int GetBoxOpenByUser(string user)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            Boxes box = db.Boxes.AsNoTracking().OrderByDescending(b => b.DateS).FirstOrDefault(b => b.UserID == user && b.IsOpen);

            if (box != null)
            {
                if (box.DateS.ToShortDateString() == DateTime.Now.ToShortDateString())
                    return box.ID;
                else
                    return 0;
            }
            else
                return 0;
        }

        public static Boxes AddBox(Boxes box, string user, string sucur)
        {
            Boxes new_box;
            ProfitTMEntities db = new ProfitTMEntities();

            using (DbContextTransaction tran = db.Database.BeginTransaction())
            {
                try
                {
                    box.DateS = DateTime.Now;
                    new_box = db.Boxes.Add(box);

                    if (new_box.AmountInit > 0)
                    {
                        saMovimientoCaja move = new saMovimientoCaja()
                        {
                            descrip = string.Format("SALDO INICIAL CAJA {0} EN FECHA {1}", box.UserID, box.DateS.ToString("dd/MM/yyyy")),
                            tipo_mov = "I",
                            forma_pag = "EF",
                            monto_h = box.AmountInit,
                            origen = "CAJ"
                        };

                        new BoxMove().AddBoxMove(move, user, sucur, true, true, false);

                        BoxMoves mov = new BoxMoves()
                        {
                            BoxID = new_box.ID,
                            UserID = user,
                            Amount = new_box.AmountInit,
                            Type = 1,
                            Date = DateTime.Now,
                            Comment = "SALDO INICIAL EN CAJA " + user
                        };

                        BoxMoves new_mov = db.BoxMoves.Add(mov);
                        new_box.BoxMoves.Add(new_mov);
                    }

                    db.SaveChanges();
                    tran.Commit();

                    new_box.BoxMoves.ToList().ForEach(delegate (BoxMoves m) {
                        m.Boxes = null;
                    });
                }
                catch (Exception ex)
                {
                    new_box = null;
                    tran.Rollback();
                    Incident.CreateIncident("ERROR INTERNO AGREGANDO CAJA", ex);

                    throw ex;
                }
            }

            return new_box;
        }

        public static BoxMoves AddMove(string user, string box_m, decimal amount, bool isIncome, string descrip)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            Boxes box = GetBoxByID(GetBoxOpenByUser(box_m).ToString());
            BoxMoves move = new BoxMoves()
            {
                BoxID = box.ID,
                UserID = user,
                Amount = amount,
                Date = DateTime.Now,
                Comment = descrip
            };

            if (isIncome)
            {
                move.Type = 1;
                box.Incomes += amount;
            }
            else
            {
                move.Type = 2;
                box.Expenses += amount;
            }

            db.Entry(box).State = EntityState.Modified;
            move = db.BoxMoves.Add(move);

            db.SaveChanges();
            return move;
        }

        public static void AddSale(string fact, decimal amount, string user)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            Boxes box = GetBoxByID(GetBoxOpenByUser(user).ToString());
            BoxMoves move = new BoxMoves() { 
                BoxID = box.ID,
                UserID = user,
                Amount = amount,
                Type = 1,
                Date = DateTime.Now,
                Comment = "COBRO EN DOLARES FACT " + fact
            };

            box.Sales += amount;
            db.Entry(box).State = EntityState.Modified;
            db.BoxMoves.Add(move);

            db.SaveChanges();
        }

        public static void CloseBox(string id)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            Boxes box = GetBoxByID(id);
            box.DateE = DateTime.Now;
            box.IsOpen = false;
            db.Entry(box).State = EntityState.Modified;

            db.SaveChanges();
        }

        public static void CloseAllBoxes()
        {
            ProfitTMEntities db = new ProfitTMEntities();

            List<Boxes> boxes = db.Boxes.AsNoTracking().Where(b => b.IsOpen).ToList();
            foreach (Boxes box in boxes)
            {
                box.IsOpen = false;
                box.DateE = DateTime.Now;
                db.Entry(box).State = EntityState.Modified;
            }

            db.SaveChanges();
        }
    }
}