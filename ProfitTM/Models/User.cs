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
            ProfitTMEntities db = new ProfitTMEntities();
            Users user;

            try
            {
                user = db.Users.AsNoTracking().SingleOrDefault(u => u.ID.ToString() == id);
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

        public static List<Users> GetAllUsers()
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Users> users;

            try
            {
                users = db.Users.AsNoTracking().ToList();
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
            Users newUser;

            user.DateReg = DateTime.Now;
            user.Password = SecurityController.Encrypt(user.Password);

            newUser = db.Users.Add(user);

            foreach (UserModules um in user.UserModules)
            {
                um.UserID = newUser.ID;
                db.UserModules.Add(um);
            }

            foreach (UserOptions uo in user.UserOptions)
            {
                uo.UserID = newUser.ID;
                db.UserOptions.Add(uo);
            }

            db.SaveChanges();

            newUser.UserModules = null;
            newUser.UserOptions = null;

            return newUser;
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

        private static List<UserModules> GetUserModules(string id)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<UserModules> modules = new List<UserModules>();

            try
            {
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
            ProfitTMEntities db = new ProfitTMEntities();
            List<UserOptions> options = new List<UserOptions>();

            try
            {
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