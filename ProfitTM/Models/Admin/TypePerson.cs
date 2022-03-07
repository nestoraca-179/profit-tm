using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class TypePerson : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static TypePerson GetTypeAdmin(string connect, string ID, string typePerson)
        //{
        //    TypePerson type;
        //    string table = "", field = "";

        //    switch (typePerson)
        //    {
        //        case "C":
        //            table = "saTipoCliente";
        //            field = "tip_cli";

        //            break;
        //        case "P":
        //            table = "saTipoProveedor";
        //            field = "tip_pro";

        //            break;
        //    }

        //    string query = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", table, field, ID);
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(query, conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        type = new TypePerson()
        //                        {
        //                            ID = reader[field].ToString().Trim(),
        //                            Name = reader["des_tipo"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        type = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        type = null;
        //    }

        //    return type;
        //}

        //public static List<TypePerson> GetAllTypesAdmin(string connect, string typePerson)
        //{
        //    List<TypePerson> types = new List<TypePerson>();
        //    string table = "", field = "";

        //    switch (typePerson)
        //    {
        //        case "C":
        //            table = "saTipoCliente";
        //            field = "tip_cli";

        //            break;
        //        case "P":
        //            table = "saTipoProveedor";
        //            field = "tip_pro";

        //            break;
        //    }

        //    string query = string.Format("select * from {0}", table);
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(query, conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        types.Add(new TypePerson()
        //                        {
        //                            ID = reader[field].ToString(),
        //                            Name = reader["des_tipo"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        types = null;
        //    }

        //    return types;
        //}
        #endregion

        public saTipoCliente GetTypeClientByID(string id)
        {
            saTipoCliente typeClient = new saTipoCliente();

            try
            {
                typeClient = db.saTipoCliente.SingleOrDefault(t => t.tip_cli == id);
            }
            catch (Exception ex)
            {
                typeClient = null;
            }

            return typeClient;
        }

        public saTipoProveedor GetTypeSupplierByID(string id)
        {
            saTipoProveedor typeSupplier = new saTipoProveedor();

            try
            {
                typeSupplier = db.saTipoProveedor.SingleOrDefault(t => t.tip_pro == id);
            }
            catch (Exception ex)
            {
                typeSupplier = null;
            }

            return typeSupplier;
        }

        public List<saTipoCliente> GetAllTypeClients()
        {
            List<saTipoCliente> typeClients = new List<saTipoCliente>();

            try
            {
                typeClients = db.saTipoCliente.ToList();
            }
            catch (Exception ex)
            {
                typeClients = null;
            }

            return typeClients;
        }

        public List<saTipoProveedor> GetAllTypeSuppliers()
        {
            List<saTipoProveedor> typeSuppliers = new List<saTipoProveedor>();

            try
            {
                typeSuppliers = db.saTipoProveedor.ToList();
            }
            catch (Exception ex)
            {
                typeSuppliers = null;
            }

            return typeSuppliers;
        }
    }
}