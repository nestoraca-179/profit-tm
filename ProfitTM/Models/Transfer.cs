using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ProfitTM.Models
{
    public class Transfer
    {
        public static Transfers GetTransferByID(string id)
        {
            Transfers trans;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                trans = db.Transfers.AsNoTracking().First(t => t.ID.ToString() == id);
            }
            catch (Exception ex)
            {
                trans = null;
                Incident.CreateIncident("ERROR BUSCANDO TRANSFERENCIA " + id, ex);
            }

            return trans;
        }

        public static List<Transfers> GetAllTransfers(int conn)
        {
            List<Transfers> transfers;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                transfers = (from t in db.Transfers.AsNoTracking()
                             join b in db.Boxes.AsNoTracking() on t.BoxID equals b.ID
                             where b.ConnID == conn
                             select t).ToList();
            }
            catch (Exception ex)
            {
                transfers = null;
                Incident.CreateIncident("ERROR BUSCANDO TRANSFERENCIAS", ex);
            }

            return transfers;
        }

        public static Transfers AddTransfer(string user, decimal amount, string account, string num, DateTime date, string descrip, int conn)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            Users us = User.GetUserByName(user);
            Boxes box = Box.GetBoxByID(Box.GetBoxOpenByUser(user, conn, us.BoxType.Value, false).ToString());
            Transfers transfer = new Transfers()
            {
                BoxID = box.ID,
                UserID = user,
                Amount = amount,
                AccountID = account,
                DocNum = num,
                Date = date,
                DateReg = DateTime.Now,
                Comment = descrip,
                Concilied = false,
                Cancelled = false
            };

            db.Transfers.Add(transfer);
            db.SaveChanges();

            return transfer;
        }

        public static void ConcilTransf(string id, string user)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            int n_transf = int.Parse(id);
            Transfers tr = db.Transfers.AsNoTracking().Single(t => t.ID == n_transf);
            tr.Concilied = true;
            tr.ConciliedBy = user;
            tr.DateConcilied = DateTime.Now;

            db.Entry(tr).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void CancelTransf(string id, string user)
        {
            ProfitTMEntities db = new ProfitTMEntities();

            int n_transf = int.Parse(id);
            Transfers tr = db.Transfers.AsNoTracking().Single(t => t.ID == n_transf);
            tr.Cancelled = true;
            tr.CancelledBy = user;
            tr.DateCancelled = DateTime.Now;

            db.Entry(tr).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}