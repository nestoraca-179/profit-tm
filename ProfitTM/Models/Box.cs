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
                             Sup = u.SupID.ToString()

                         }).ToList();

                foreach (Box box in boxes)
                {
                    box.Sup = box.Sup != "" ? User.GetUserByID(box.Sup).Username : null;
                    box.BoxMoves.ToList().ForEach(delegate(BoxMoves m) {
                        m.Boxes = null;
                    });
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

        public static Boxes AddBox(Boxes box)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            box.DateS = DateTime.Now;

            Boxes new_box = db.Boxes.Add(box);
            db.SaveChanges();

            return new_box;
        }

        public static BoxMoves AddBoxMove(BoxMoves move)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            Boxes box = GetBoxByID(move.BoxID.ToString());

            if (move.Type == 1)
                box.Incomes += move.Amount;
            else if (move.Type == 2)
                box.Expenses += move.Amount;

            db.Entry(box).State = EntityState.Modified;

            move.Date = DateTime.Now;
            BoxMoves new_move = db.BoxMoves.Add(move);
            db.SaveChanges();

            new_move.BoxID = move.BoxID;
            new_move.Boxes = null;

            return new_move;
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

        public static BoxMoves AddIncome(string mov, decimal amount, string user, string descrip)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            Boxes box = GetBoxByID(GetBoxOpenByUser(user).ToString());
            BoxMoves move = new BoxMoves()
            {
                BoxID = box.ID,
                UserID = user,
                Amount = amount,
                Type = 1,
                Date = DateTime.Now,
                Comment = descrip
            };

            box.Incomes += amount;
            db.Entry(box).State = EntityState.Modified;
            move = db.BoxMoves.Add(move);

            db.SaveChanges();
            return move;
        }

        public static BoxMoves AddExpense(string ord, decimal amount, string user, string descrip)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            Boxes box = GetBoxByID(GetBoxOpenByUser(user).ToString());
            BoxMoves move = new BoxMoves()
            {
                BoxID = box.ID,
                UserID = user,
                Amount = amount,
                Type = 2,
                Date = DateTime.Now,
                Comment = descrip
            };

            box.Expenses += amount;
            db.Entry(box).State = EntityState.Modified;
            move = db.BoxMoves.Add(move);

            db.SaveChanges();
            return move;
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