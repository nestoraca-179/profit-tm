using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Supplier : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public static Supplier GetSupplier(string connect, string ID)
        //{
        //    Supplier supplier;

        //    string query = string.Format("SELECT * FROM saProveedor WHERE co_prov = '{0}'", ID);
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
        //                        supplier = new Supplier()
        //                        {
        //                            ID = reader["co_prov"].ToString().Trim(),
        //                            Name = reader["prov_des"].ToString(),
        //                            Type = TypePerson.GetTypeAdmin(connect, reader["tip_pro"].ToString(), "P"),
        //                            Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
        //                            Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
        //                            Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
        //                            Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
        //                            RIF = reader["rif"].ToString(),
        //                            Email = reader["telefonos"].ToString(),
        //                            Phone = reader["email"].ToString(),
        //                            Address1 = reader["direc1"].ToString(),
        //                            Contrib = bool.Parse(reader["contribu_e"].ToString())
        //                        };
        //                    }
        //                    else
        //                    {
        //                        supplier = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        supplier = null;
        //    }

        //    return supplier;
        //}

        //public static List<Supplier> GetAllSuppliers(string connect)
        //{
        //    List<Supplier> suppliers = new List<Supplier>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saProveedor", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        suppliers.Add(new Supplier()
        //                        {
        //                            ID = reader["co_prov"].ToString(),
        //                            Name = reader["prov_des"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        suppliers = null;
        //    }

        //    return suppliers;
        //}
        #endregion

        public static saProveedor GetSupplierByID(string id)
        {
            saProveedor proveedor = new saProveedor();

            try
            {
                proveedor = db.saProveedor.SingleOrDefault(c => c.co_prov == id);
            }
            catch (Exception ex)
            {
                proveedor = null;
            }

            return proveedor;
        }

        public static List<saProveedor> GetAllSuppliers()
        {
            List<saProveedor> proveedores = new List<saProveedor>();

            try
            {
                proveedores = db.saProveedor.ToList();
            }
            catch (Exception ex)
            {
                proveedores = null;
            }

            return proveedores;
        }

        public ProfitTMResponse GetMostActiveSuppliers(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<saProveedor> proveedores = new List<saProveedor>();

            try
            {
                DateTime fec_h = DateTime.Now;
                DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

                var sp = db.RepProveedorMasCompra(fec_d, fec_h, null, null, number, null, null, null, null, null);
                var enumerator = sp.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    saProveedor proveedor = new saProveedor();

                    proveedor.co_prov = enumerator.Current.co_prov;
                    proveedor.prov_des = enumerator.Current.co_prov + " - " + enumerator.Current.prov_des;
                    proveedor.campo1 = enumerator.Current.Compra.ToString();

                    proveedores.Add(proveedor);
                }

                response.Status = "OK";
                response.Result = proveedores;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = "MAS - " + ex.Message;
            }

            return response;
        }

        public ProfitTMResponse GetMostMorousSuppliers(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<saProveedor> proveedores = new List<saProveedor>();

            try
            {
                DateTime fec_h = DateTime.Now;
                DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

                var sp = db.RepEstadoCuentaProv(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
                var enumerator = sp.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    saProveedor proveedor = new saProveedor();

                    proveedor.co_prov = enumerator.Current.co_prov;
                    proveedor.prov_des = enumerator.Current.prov_des;
                    proveedor.campo1 = ((enumerator.Current.tot_debe ?? 0) - (enumerator.Current.tot_haber ?? 0)).ToString();

                    proveedores.Add(proveedor);
                }

                proveedores = (from c in proveedores
                               group c.campo1 by (c.co_prov, c.prov_des) into g
                               select new saProveedor
                               {

                                   co_prov = g.Key.co_prov,
                                   prov_des = g.Key.co_prov + " - " + g.Key.prov_des,
                                   campo1 = Math.Round(g.Select(x => double.Parse(x)).Sum(), 2).ToString()

                               }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

                if (proveedores.Count > number)
                    proveedores.RemoveRange(number, proveedores.Count - number);

                response.Status = "OK";
                response.Result = proveedores;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = "MMS - " + ex.Message;
            }

            return response;
        }

        public saProveedor Add(saProveedor supplier)
        {
            saProveedor newSupplier = new saProveedor();
            supplier.plaz_pag = new Cond().GetCondByID(supplier.cond_pag).dias_cred;

            var sp = db.pInsertarProveedor(supplier.co_prov, supplier.prov_des, supplier.co_seg, supplier.co_zon, supplier.inactivo, supplier.direc1, supplier.direc2, supplier.telefonos,
                                           supplier.fax, supplier.respons, supplier.fecha_reg, supplier.tip_pro, supplier.mont_cre, supplier.co_mone, supplier.cond_pag, supplier.plaz_pag,
                                           supplier.desc_ppago, supplier.desc_glob, supplier.rif, supplier.nacional, supplier.dis_cen, supplier.nit, supplier.email, supplier.co_cta_ingr_egr,
                                           supplier.comentario, supplier.tipo_adi, supplier.matriz, supplier.co_tab, supplier.tipo_per, supplier.co_pais, supplier.ciudad, supplier.zip,
                                           supplier.website, supplier.formtype, supplier.taxid, supplier.contribu_e, supplier.rete_regis_doc, supplier.porc_esp, supplier.campo1, supplier.campo2,
                                           supplier.campo3, supplier.campo4, supplier.campo5, supplier.campo6, supplier.campo7, supplier.campo8, "PROFIT WEB", null, "SERVER PROFIT WEB", null,
                                           null, null, null, supplier.email_alterno, supplier.sujeto_obj_retenISLR_auto);
            var enumerator = sp.GetEnumerator();

            if (enumerator.MoveNext())
            {
                Guid rowguid = enumerator.Current.rowguid.Value;
                newSupplier = db.saProveedor.SingleOrDefault(c => c.rowguid == rowguid);
            }

            return newSupplier;
        }

        public saProveedor Edit(saProveedor supplier)
        {
            saProveedor editSupplier = new saProveedor();
            supplier.plaz_pag = new Cond().GetCondByID(supplier.cond_pag).dias_cred;

            var sp = db.pActualizarProveedor(supplier.co_prov, supplier.co_prov, supplier.prov_des, supplier.co_seg, supplier.co_zon, supplier.inactivo, supplier.direc1, supplier.direc2,
                                             supplier.telefonos, supplier.fax, supplier.respons, supplier.fecha_reg, supplier.tip_pro, supplier.mont_cre, supplier.co_mone, supplier.cond_pag,
                                             supplier.plaz_pag, supplier.desc_ppago, supplier.desc_glob, supplier.rif, supplier.nacional, supplier.dis_cen, supplier.nit, supplier.email,
                                             supplier.co_cta_ingr_egr, supplier.comentario, supplier.tipo_adi, supplier.matriz, supplier.co_tab, supplier.tipo_per, supplier.co_pais,
                                             supplier.ciudad, supplier.zip, supplier.website, supplier.formtype, supplier.taxid, supplier.contribu_e, supplier.rete_regis_doc, supplier.porc_esp,
                                             supplier.campo1, supplier.campo2, supplier.campo3, supplier.campo4, supplier.campo5, supplier.campo6, supplier.campo7, supplier.campo8, "PROFIT WEB",
                                             null, "SERVER PROFIT WEB", null, null, null, supplier.validador, supplier.rowguid, null, null, supplier.email_alterno, supplier.sujeto_obj_retenISLR_auto);
            var enumerator = sp.GetEnumerator();

            if (enumerator.MoveNext())
            {
                Guid rowguid = enumerator.Current.rowguid.Value;
                editSupplier = db.saProveedor.SingleOrDefault(c => c.rowguid == rowguid);
            }

            return editSupplier;
        }

        public saProveedor Delete(string id)
        {
            saProveedor supplier = GetSupplierByID(id);
            db.pEliminarProveedor(supplier.co_prov, supplier.validador, "SERVER PROFIT WEB", "PROFIT WEB", null, supplier.rowguid);

            return supplier;
        }
    }
}