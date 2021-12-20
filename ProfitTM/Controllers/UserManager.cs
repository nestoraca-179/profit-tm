using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ProfitTM.Controllers
{
    public class UserManager
    {
        string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

        public ProfitTMResponse addUser(User user)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("insert into Users (Username, Password, Descrip, DateReg, CI, Email, Phone, IsAdm, IsCon, IsNom, Enabled) ");
            query.AppendFormat(
                "values ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', {6}, '{7}', '{8}', '{9}', '{10}')", 
                user.Username,
                SecurityController.Encrypt(user.Password),
                user.Descrip,
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                StringController.VerifyValueDb(user.CI),
                user.Email,
                StringController.VerifyValueDb(user.Phone),
                user.IsAdm,
                user.IsCon,
                user.IsNom,
                user.Enabled
            );

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();
                        query.Clear();

                        if (rows > 0)
                        {
                            string ID_NEW_USER = "";
                            comm.CommandText = "select top 1 * from Users order by DateReg desc";
                            
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    ID_NEW_USER = reader["ID"].ToString();
                                }
                            }

                            foreach (string mod in user.Modules)
                            {
                                query.Append("insert into UserModules (UserID, ModuleID) \n");
                                query.AppendFormat("values ({0}, {1}) \n", ID_NEW_USER, mod);
                            }

                            foreach (string opt in user.Options)
                            {
                                query.AppendLine("insert into UserOptions (UserID, OptionID) \n");
                                query.AppendFormat("values ({0}, {1}) \n", ID_NEW_USER, opt);
                            }

                            comm.CommandText = query.ToString();
                            rows = comm.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                response.Status = "OK";
                                response.Result = ID_NEW_USER;
                            }
                            else
                            {
                                response.Status = "ERROR";
                                response.Message = "Se ha producido un error al agregar los privilegios del usuario";
                            }
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al agregar el usuario";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }
            finally
            {
                query.Clear();
            }

            return response;
        }

        public ProfitTMResponse editUser(User user)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("update Users \n");
            query.AppendFormat("set Descrip = '{0}', ", user.Descrip);
            query.AppendFormat("CI = {0}, ", StringController.VerifyValueDb(user.CI));
            query.AppendFormat("Email = '{0}', ", user.Email);
            query.AppendFormat("Phone = {0}, ", StringController.VerifyValueDb(user.Phone));
            query.AppendFormat("IsAdm = '{0}', IsCon = '{1}', IsNom = '{2}', Enabled = '{3}' ", user.IsAdm, user.IsCon, user.IsNom, user.Enabled);
            query.AppendFormat("where ID = '{0}'", user.ID);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();
                        query.Clear();

                        if (rows > 0)
                        {
                            string id = "", item = "";
                            
                            List<string> modsDeleted = new List<string>(),
                                         optsDeleted = new List<string>();

                            comm.CommandText = "select ModuleID from UserModules where UserID = " + user.ID;
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    id = reader["ModuleID"].ToString();
                                    item = user.Modules.SingleOrDefault(m => m == id);

                                    if (item == null)
                                        modsDeleted.Add(id);
                                    else
                                        user.Modules.Remove(item);
                                }
                            }

                            comm.CommandText = "select OptionID from UserOptions where UserID = " + user.ID;
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    id = reader["OptionID"].ToString();
                                    item = user.Options.SingleOrDefault(m => m == id);

                                    if (item == null)
                                        optsDeleted.Add(id);
                                    else
                                        user.Options.Remove(item);
                                }
                            }

                            if (user.Modules.Count == 0 && user.Options.Count == 0 && modsDeleted.Count == 0 && optsDeleted.Count == 0)
                            {
                                response.Status = "OK";
                                response.Result = rows;
                            }
                            else
                            {
                                if (user.Modules.Count > 0)
                                {
                                    foreach (string mod in user.Modules)
                                    {
                                        query.Append("insert into UserModules (UserID, ModuleID) \n");
                                        query.AppendFormat("values ({0}, {1}) \n", user.ID, mod);
                                    }
                                }

                                if (user.Options.Count > 0)
                                {
                                    foreach (string opt in user.Options)
                                    {
                                        query.AppendLine("insert into UserOptions (UserID, OptionID) \n");
                                        query.AppendFormat("values ({0}, {1}) \n", user.ID, opt);
                                    }
                                }

                                if (modsDeleted.Count > 0)
                                {
                                    foreach (string m in modsDeleted)
                                    {
                                        query.AppendLine("delete from UserModules \n");
                                        query.AppendFormat("where UserID = {0} and ModuleID = {1} \n", user.ID, m);
                                    }
                                }

                                if (optsDeleted.Count > 0)
                                {
                                    foreach (string o in optsDeleted)
                                    {
                                        query.AppendLine("delete from UserOptions \n");
                                        query.AppendFormat("where UserID = {0} and OptionID = {1} \n", user.ID, o);
                                    }
                                }

                                comm.CommandText = query.ToString();
                                rows = comm.ExecuteNonQuery();

                                if (rows > 0)
                                {
                                    response.Status = "OK";
                                    response.Result = rows;
                                }
                                else
                                {
                                    response.Status = "ERROR";
                                    response.Message = "Se ha producido un error al agregar los privilegios del usuario";
                                }
                            }
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al modificar el usuario";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }
            finally
            {
                query.Clear();
            }

            return response;
        }

        public ProfitTMResponse deleteUser(int id)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.AppendFormat("delete from UserOptions where UserID = {0} \n", id);
            query.AppendFormat("delete from UserModules where UserID = {0} \n", id);
            query.AppendFormat("delete from Users where ID = {0} \n", id);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            response.Status = "OK";
                            response.Result = rows;
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al eliminar el usuario";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }
            finally
            {
                query.Clear();
            }

            return response;
        }
    }
}