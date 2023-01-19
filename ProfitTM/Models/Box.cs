using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProfitTM.Models
{
    public class Box
    {
        public static Boxes GetBoxByID(string id)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            Boxes box;

            try
            {
                box = db.Boxes.AsNoTracking().First(b => b.ID.ToString() == id);
            }
            catch (Exception ex)
            {
                box = null;
                Incident.CreateIncident("ERROR BUSCANDO CAJA " + id, ex);
            }

            return box;
        }

        public static List<Boxes> GetAllBoxesAndMoves()
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Boxes> boxes;

            try
            {
                boxes = db.Boxes.AsNoTracking().Include("BoxMoves").ToList();
                foreach (Boxes box in boxes)
                {
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
    }
}