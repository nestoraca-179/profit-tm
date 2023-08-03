using System;
using System.Collections.Generic;
using System.Linq;

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

        public static List<Transfers> GetAllTransfers()
        {
            List<Transfers> transfers;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                transfers = db.Transfers.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                transfers = null;
                Incident.CreateIncident("ERROR BUSCANDO TRANSFERENCIAS", ex);
            }

            return transfers;
        }

        public static Transfers AddTransfer(string user, decimal amount, string account, string num, string descrip, int conn)
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
                Date = DateTime.Now,
                Comment = descrip,
                Concilied = false
            };

            db.Transfers.Add(transfer);
            db.SaveChanges();

            return transfer;
        }
    }
}