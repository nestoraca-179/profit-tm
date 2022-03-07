using System;
using System.Linq;
using System.Collections.Generic;
using ProfitTM.Controllers;
using System.Data.Entity;

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
                user = db.Users.SingleOrDefault(u => u.ID.ToString() == id);
                user.UserModules = GetUserModules(user.ID.ToString());
                user.UserOptions = GetUserOptions(user.ID.ToString());
            }
            catch (Exception ex)
            {
                user = null;
            }

            return user;
        }

        public static List<Users> GetAllUsers()
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Users> users = new List<Users>();

            try
            {
                users = db.Users.ToList();
            }
            catch (Exception ex)
            {
                users = null;
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
                modules = db.UserModules.Where(um => um.UserID.ToString() == id).ToList();
            }
            catch (Exception ex)
            {
                modules = null;
            }

            return modules;
        }

        private static List<UserOptions> GetUserOptions(string id)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<UserOptions> options = new List<UserOptions>();

            try
            {
                options = db.UserOptions.Where(uo => uo.UserID.ToString() == id).ToList();
            }
            catch (Exception ex)
            {
                options = null;
            }

            return options;
        }

        #region CODIGO ANTERIOR
        //public static User GetUser(string id)
        //{
        //    User user;
        //    string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBMain))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(string.Format("select * from Users where ID = {0}", id), conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        user = new User()
        //                        {
        //                            ID = reader["ID"].ToString(),
        //                            Username = reader["Username"].ToString(),
        //                            Descrip = reader["Descrip"].ToString(),
        //                            DateReg = DateTime.Parse(reader["DateReg"].ToString()),
        //                            CI = reader["CI"].ToString(),
        //                            Email = reader["Email"].ToString(),
        //                            Phone = reader["Phone"].ToString(),
        //                            IsAdm = bool.Parse(reader["IsAdm"].ToString()),
        //                            IsCon = bool.Parse(reader["IsCon"].ToString()),
        //                            IsNom = bool.Parse(reader["IsNom"].ToString()),
        //                            Enabled = bool.Parse(reader["Enabled"].ToString()),
        //                            Modules = GetUserModules(reader["ID"].ToString()),
        //                            Options = GetUserOptions(reader["ID"].ToString())
        //                        };
        //                    }
        //                    else
        //                    {
        //                        user = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        user = null;
        //    }

        //    return user;
        //}

        //public static List<User> GetUsers()
        //{
        //    List<User> users = new List<User>();
        //    string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBMain))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("SELECT * FROM Users", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        User user = new User();

        //                        user.ID = reader["ID"].ToString();
        //                        user.Username = reader["Username"].ToString();
        //                        user.Descrip = reader["Descrip"].ToString();
        //                        user.DateReg = DateTime.Parse(reader["DateReg"].ToString());
        //                        user.CI = reader["CI"].ToString();
        //                        user.Email = reader["Email"].ToString();
        //                        user.Phone = reader["Phone"].ToString();
        //                        user.IsAdm = bool.Parse(reader["IsAdm"].ToString());
        //                        user.IsCon = bool.Parse(reader["IsCon"].ToString());
        //                        user.IsNom = bool.Parse(reader["IsNom"].ToString());
        //                        user.Enabled = bool.Parse(reader["Enabled"].ToString());

        //                        users.Add(user);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        users = null;
        //    }

        //    return users;
        //}

        //private static List<string> GetUserModules(string id)
        //{
        //    List<string> modules = new List<string>();
        //    string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBMain))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(string.Format("select * from UserModules where UserID = {0}", id), conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        modules.Add(reader["ModuleID"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        modules = null;
        //    }

        //    return modules;
        //}

        //private static List<string> GetUserOptions(string id)
        //{
        //    List<string> options = new List<string>();
        //    string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBMain))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(string.Format("select * from UserOptions where UserID = {0}", id), conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        options.Add(reader["OptionID"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        options = null;
        //    }

        //    return options;
        //}
        #endregion
    }
}