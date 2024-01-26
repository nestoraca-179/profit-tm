using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using ProfitTM.Controllers;

namespace ProfitTM.Models
{
    public class User
    {
        public static Users GetUserByID(string id)
        {
            Users user;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                user = db.Users.AsNoTracking().Single(u => u.ID.ToString() == id);
                user.UserModules = GetUserModules(user.ID.ToString());
                user.UserOptions = GetUserOptions(user.ID.ToString());
            }
            catch (Exception ex)
            {
                user = null;
                Incident.CreateIncident("ERROR BUSCANDO USUARIO " + id, ex);
            }

            return user;
        }

        public static Users GetUserByName(string name)
        {
            Users user;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                user = db.Users.AsNoTracking().SingleOrDefault(u => u.Username == name);
            }
            catch (Exception ex)
            {
                user = null;
                Incident.CreateIncident("ERROR BUSCANDO USUARIO " + name, ex);
            }

            return user;
        }

        public static List<Users> GetAllUsers(bool sups)
        {
            List<Users> users;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                users = db.Users.AsNoTracking().ToList();

                if (sups)
                    users = users.FindAll(u => u.BoxType == 2 || u.BoxType == 3);
            }
            catch (Exception ex)
            {
                users = null;
                Incident.CreateIncident("ERROR BUSCANDO USUARIOS", ex);
            }

            return users;
        }

        public static Users Add(Users user)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            Users new_user;

            user.DateReg = DateTime.Now;
            user.NextChange = DateTime.Now.AddMinutes(-1);
            user.Password = SecurityController.Encrypt(user.Password);

            new_user = db.Users.Add(user);

            foreach (UserModules um in user.UserModules)
            {
                um.UserID = new_user.ID;
                db.UserModules.Add(um);
            }

            foreach (UserOptions uo in user.UserOptions)
            {
                uo.UserID = new_user.ID;
                db.UserOptions.Add(uo);
            }

            db.SaveChanges();

            new_user.UserModules = null;
            new_user.UserOptions = null;

            return new_user;
        }

        public static Users Edit(Users user)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            db.UserModules.RemoveRange(db.UserModules.Where(um => um.UserID == user.ID));
            db.UserOptions.RemoveRange(db.UserOptions.Where(uo => uo.UserID == user.ID));

            foreach (UserModules um in user.UserModules)
            {
                um.UserID = user.ID;
                db.UserModules.Add(um);
            }

            foreach (UserOptions uo in user.UserOptions)
            {
                uo.UserID = user.ID;
                db.UserOptions.Add(uo);
            }

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            user.UserModules = null;
            user.UserOptions = null;

            return user;
        }

        public static Users Delete(int id)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            Users user = GetUserByID(id.ToString());

            db.Users.Attach(user);
            db.Users.Remove(user);
            db.SaveChanges();

            user.UserModules = null;
            user.UserOptions = null;

            return user;
        }

        public static void ResetPass(int id)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            Users user = GetUserByID(id.ToString());

            user.Password = SecurityController.Encrypt("gish.123");
            user.NextChange = DateTime.Now.AddDays(-1);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        private static List<UserModules> GetUserModules(string id)
        {
            List<UserModules> modules = new List<UserModules>();

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                modules = db.UserModules.AsNoTracking().Where(um => um.UserID.ToString() == id).ToList();
            }
            catch (Exception ex)
            {
                modules = null;
                Incident.CreateIncident("ERROR BUSCANDO MODULOS DE USUARIO " + id, ex);
            }

            return modules;
        }

        private static List<UserOptions> GetUserOptions(string id)
        {
            List<UserOptions> options = new List<UserOptions>();

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                options = db.UserOptions.AsNoTracking().Where(uo => uo.UserID.ToString() == id).ToList();
            }
            catch (Exception ex)
            {
                options = null;
                Incident.CreateIncident("ERROR BUSCANDO OPCIONES DE USUARIO " + id, ex);
            }

            return options;
        }
    }
}