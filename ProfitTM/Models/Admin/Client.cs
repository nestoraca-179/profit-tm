using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Client : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public static Client GetClient(string connect, string ID)
        //{
        //    Client client;

        //    string query = string.Format("SELECT * FROM saCliente WHERE co_cli = '{0}'", ID);
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
        //                        client = new Client()
        //                        {
        //                            ID = reader["co_cli"].ToString().Trim(),
        //                            Name = reader["cli_des"].ToString(),
        //                            Type = TypePerson.GetTypeAdmin(connect, reader["tip_cli"].ToString(), "C"),
        //                            Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
        //                            Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
        //                            Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
        //                            Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
        //                            RIF = reader["rif"].ToString(),
        //                            Email = reader["email"].ToString(),
        //                            AltEmail = reader["email_alterno"].ToString(),
        //                            Phone = reader["telefonos"].ToString(),
        //                            Address1 = reader["direc1"].ToString(),
        //                            Address2 = reader["direc2"].ToString(),
        //                            DelAddress = reader["dir_ent2"].ToString(),
        //                            Seller = Seller.GetSeller(connect, reader["co_ven"].ToString()),
        //                            Cond = Cond.GetCond(connect, reader["cond_pag"].ToString()),
        //                            Contrib = bool.Parse(reader["contrib"].ToString()),
        //                            DateReg = DateTime.Parse(reader["fecha_reg"].ToString()),
        //                            City = reader["ciudad"].ToString(),
        //                            Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
        //                            CoUsIn = reader["co_us_in"].ToString(),
        //                            CoUsMo = reader["co_us_mo"].ToString(),
        //                            CoSucuIn = reader["co_sucu_in"].ToString(),
        //                            CoSucuMo = reader["co_sucu_mo"].ToString(),
        //                            DateIn = DateTime.Parse(reader["fe_us_in"].ToString()),
        //                            DateMo = DateTime.Parse(reader["fe_us_mo"].ToString()),
        //                            CoTab = reader["co_tab"].ToString().Trim(),
        //                            ContribuE = bool.Parse(reader["contribu_e"].ToString()),
        //                            Fax = reader["fax"].ToString(),
        //                            Disabled = bool.Parse(reader["inactivo"].ToString()),
        //                            Juridic = bool.Parse(reader["juridico"].ToString()),
        //                            Valid = bool.Parse(reader["valido"].ToString()),
        //                            Salestax = reader["salestax"].ToString(),
        //                            State = reader["estado"].ToString(),
        //                            Monday = bool.Parse(reader["lunes"].ToString()),
        //                            Tuesday = bool.Parse(reader["martes"].ToString()),
        //                            Wednesday = bool.Parse(reader["miercoles"].ToString()),
        //                            Thursday = bool.Parse(reader["jueves"].ToString()),
        //                            Friday = bool.Parse(reader["viernes"].ToString()),
        //                            Saturday = bool.Parse(reader["sabado"].ToString()),
        //                            Sunday = bool.Parse(reader["domingo"].ToString()),
        //                            MontCre = double.Parse(reader["mont_cre"].ToString()),
        //                            DescPPago = double.Parse(reader["desc_ppago"].ToString()),
        //                            DescGlob = double.Parse(reader["desc_glob"].ToString()),
        //                            HorarCaja = reader["horar_caja"].ToString(),
        //                            FrecuVist = reader["frecu_vist"].ToString(),
        //                            TipPer = reader["tipo_per"].ToString(),
        //                            Comment = reader["comentario"].ToString(),
        //                            Respons = reader["respons"].ToString(),
        //                            ReteRegisDoc = bool.Parse(reader["rete_regis_doc"].ToString()),
        //                            PorcEsp = double.Parse(reader["porc_esp"].ToString()),
        //                            TipAdi = reader["tipo_adi"].ToString(),
        //                            Matriz = reader["matriz"].ToString(),
        //                            CodID = int.Parse(reader["id"].ToString())
        //                        };
        //                    }
        //                    else
        //                    {
        //                        client = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        client = null;
        //    }

        //    return client;
        //}

        //public static List<Client> GetAllClients(string connect)
        //{
        //    List<Client> clients = new List<Client>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saCliente", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Client client = new Client()
        //                        {
        //                            ID = reader["co_cli"].ToString().Trim(),
        //                            Name = reader["cli_des"].ToString(),
        //                            Type = TypePerson.GetTypeAdmin(connect, reader["tip_cli"].ToString(), "C"),
        //                            Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
        //                            Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
        //                            Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
        //                            Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
        //                            RIF = reader["rif"].ToString(),
        //                            Email = reader["email"].ToString(),
        //                            AltEmail = reader["email_alterno"].ToString(),
        //                            Phone = reader["telefonos"].ToString(),
        //                            Address1 = reader["direc1"].ToString(),
        //                            Address2 = reader["direc2"].ToString(),
        //                            DelAddress = reader["dir_ent2"].ToString(),
        //                            Seller = Seller.GetSeller(connect, reader["co_ven"].ToString()),
        //                            Cond = Cond.GetCond(connect, reader["cond_pag"].ToString()),
        //                            Contrib = bool.Parse(reader["contrib"].ToString()),
        //                            DateReg = DateTime.Parse(reader["fecha_reg"].ToString()),
        //                            City = reader["ciudad"].ToString(),
        //                            Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
        //                            CoUsIn = reader["co_us_in"].ToString(),
        //                            CoUsMo = reader["co_us_mo"].ToString(),
        //                            CoSucuIn = reader["co_sucu_in"].ToString(),
        //                            CoSucuMo = reader["co_sucu_mo"].ToString(),
        //                            DateIn = DateTime.Parse(reader["fe_us_in"].ToString()),
        //                            DateMo = DateTime.Parse(reader["fe_us_mo"].ToString()),
        //                            CoTab = reader["co_tab"].ToString().Trim(),
        //                            ContribuE = bool.Parse(reader["contribu_e"].ToString()),
        //                            Fax = reader["fax"].ToString(),
        //                            Disabled = bool.Parse(reader["inactivo"].ToString()),
        //                            Juridic = bool.Parse(reader["juridico"].ToString()),
        //                            Valid = bool.Parse(reader["valido"].ToString()),
        //                            Salestax = reader["salestax"].ToString(),
        //                            State = reader["estado"].ToString(),
        //                            Monday = bool.Parse(reader["lunes"].ToString()),
        //                            Tuesday = bool.Parse(reader["martes"].ToString()),
        //                            Wednesday = bool.Parse(reader["miercoles"].ToString()),
        //                            Thursday = bool.Parse(reader["jueves"].ToString()),
        //                            Friday = bool.Parse(reader["viernes"].ToString()),
        //                            Saturday = bool.Parse(reader["sabado"].ToString()),
        //                            Sunday = bool.Parse(reader["domingo"].ToString()),
        //                            MontCre = double.Parse(reader["mont_cre"].ToString()),
        //                            DescPPago = double.Parse(reader["desc_ppago"].ToString()),
        //                            DescGlob = double.Parse(reader["desc_glob"].ToString()),
        //                            HorarCaja = reader["horar_caja"].ToString(),
        //                            FrecuVist = reader["frecu_vist"].ToString(),
        //                            TipPer = reader["tipo_per"].ToString(),
        //                            Comment = reader["comentario"].ToString(),
        //                            Respons = reader["respons"].ToString(),
        //                            ReteRegisDoc = bool.Parse(reader["rete_regis_doc"].ToString()),
        //                            PorcEsp = double.Parse(reader["porc_esp"].ToString()),
        //                            TipAdi = reader["tipo_adi"].ToString(),
        //                            Matriz = reader["matriz"].ToString(),
        //                            CodID = int.Parse(reader["id"].ToString())
        //                        };

        //                        client.ExtraFields = new List<string>();
        //                        for (int i = 1; i < 9; i++)
        //                        {
        //                            client.ExtraFields.Add(reader["campo" + i].ToString());
        //                        }

        //                        clients.Add(client);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clients = null;
        //    }

        //    return clients;
        //}
        #endregion
        
        public saCliente GetClientByID(string id)
        {
            saCliente client = new saCliente();

            try
            {
                client = db.saCliente.SingleOrDefault(c => c.co_cli == id);
                client.saCliente1 = null;
                client.saCliente2 = null;
            }
            catch (Exception ex)
            {
                client = null;
            }

            return client;
        }

        public List<saCliente> GetAllClients()
        {
            List<saCliente> clients = new List<saCliente>();

            try
            {
                clients = db.saCliente.ToList();
                clients.ForEach(delegate(saCliente c) {
                    c.saCliente1 = null;
                    c.saCliente2 = null;
                });
            }
            catch (Exception ex)
            {
                clients = null;
            }

            return clients;
        }

        public ProfitTMResponse GetMostActiveClients(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<saCliente> clientes = new List<saCliente>();

            try
            {
                var sp = db.RepClienteMasVenta(null, null, null, null, null, null, null, null, null, number, null, null, null, null);
                var enumerator = sp.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    saCliente cliente = new saCliente();

                    cliente.co_cli = enumerator.Current.co_cli;
                    cliente.cli_des = enumerator.Current.cli_des;
                    cliente.campo1 = enumerator.Current.Venta.ToString();

                    clientes.Add(cliente);
                }

                response.Status = "OK";
                response.Result = clientes;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = "MAC - " + ex.Message;
            }

            return response;
        }

        public ProfitTMResponse GetMostMorousClients(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<saCliente> clientes = new List<saCliente>();

            try
            {
                var sp = db.RepEstadoCuentaCli(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                var enumerator = sp.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    saCliente cliente = new saCliente();

                    cliente.co_cli = enumerator.Current.co_prov;
                    cliente.cli_des = enumerator.Current.prov_des;
                    cliente.campo1 = ((enumerator.Current.tot_debe ?? 0) - (enumerator.Current.tot_haber ?? 0)).ToString();

                    clientes.Add(cliente);
                }

                clientes = (from c in clientes
                            group c.campo1 by (c.co_cli, c.cli_des) into g
                            select new saCliente
                            {

                                co_cli = g.Key.co_cli,
                                cli_des = g.Key.cli_des,
                                campo1 = Math.Round(g.Select(x => double.Parse(x)).Sum(), 2).ToString()

                            }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

                if (clientes.Count > number)
                    clientes.RemoveRange(number, clientes.Count - number);

                response.Status = "OK";
                response.Result = clientes;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = "MMC - " + ex.Message;
            }

            return response;
        }

        public saCliente Add(saCliente client)
        {
            saCliente newClient = new saCliente();

            if (!client.sincredito)
            {
                client.plaz_pag = new Cond().GetCondByID(client.cond_pag).dias_cred;
            }

            var sp = db.pInsertarCliente(client.co_cli, client.login, client.password, client.salestax, client.cli_des, client.co_seg, client.co_zon, client.co_ven, client.estado,
                                         client.inactivo, client.valido, client.sincredito, client.lunes, client.martes, client.miercoles, client.jueves, client.viernes, client.sabado,
                                         client.domingo, client.direc1, client.direc2, client.dir_ent2, client.horar_caja, client.frecu_vist, client.telefonos, client.fax, client.respons,
                                         client.fecha_reg, client.tip_cli, client.serialp, client.puntaje, client.Id, client.mont_cre, client.co_mone, client.cond_pag, client.plaz_pag,
                                         client.desc_ppago, client.desc_glob, null, null, client.rif, client.contrib, client.dis_cen, client.nit, client.email, client.co_cta_ingr_egr,
                                         client.comentario, client.campo1, client.campo2, client.campo3, client.campo4, client.campo5, client.campo6, client.campo7, client.campo8,
                                         "PROFIT WEB", "SERVER PROFIT WEB", null, null, null, client.juridico, client.tipo_adi, client.matriz, client.co_tab, client.tipo_per, client.co_pais,
                                         client.ciudad, client.zip, client.website, client.contribu_e, client.rete_regis_doc, client.porc_esp, null, null, null, client.email_alterno);
            var enumerator = sp.GetEnumerator();

            if (enumerator.MoveNext())
            {
                Guid rowguid = enumerator.Current.rowguid.Value;
                newClient = db.saCliente.SingleOrDefault(c => c.rowguid == rowguid);
            }

            return newClient;
        }

        public saCliente Edit(saCliente client)
        {
            saCliente editClient = new saCliente();

            if (!client.sincredito)
            {
                client.plaz_pag = new Cond().GetCondByID(client.cond_pag).dias_cred;
            }

            var sp = db.pActualizarCliente(client.co_cli, client.co_cli, client.login, client.password, client.salestax, client.cli_des, client.co_seg, client.co_zon, client.co_ven, client.estado,
                                           client.inactivo, client.valido, client.sincredito, client.lunes, client.martes, client.miercoles, client.jueves, client.viernes, client.sabado,
                                           client.domingo, client.direc1, client.direc2, client.dir_ent2, client.horar_caja, client.frecu_vist, client.telefonos, client.fax, client.respons,
                                           client.fecha_reg, client.tip_cli, client.serialp, client.puntaje, client.Id, client.mont_cre, client.co_mone, client.cond_pag, client.plaz_pag,
                                           client.desc_ppago, client.desc_glob, client.rif, client.contrib, client.dis_cen, client.nit, client.email, client.co_cta_ingr_egr, client.comentario,
                                           client.campo1, client.campo2, client.campo3, client.campo4, client.campo5, client.campo6, client.campo7, client.campo8, "PROFIT WEB", null, "SERVER PROFIT WEB",
                                           null, null, null, client.juridico, client.tipo_adi, client.matriz, client.co_tab, client.tipo_per, client.co_pais, client.ciudad, client.zip, client.website,
                                           client.contribu_e, client.rete_regis_doc, client.porc_esp, client.validador, client.rowguid, null, null, null, client.email_alterno);
            var enumerator = sp.GetEnumerator();

            if (enumerator.MoveNext())
            {
                Guid rowguid = enumerator.Current.rowguid.Value;
                editClient = db.saCliente.SingleOrDefault(c => c.rowguid == rowguid);
            }

            return editClient;
        }

        public saCliente Delete(string id)
        {
            saCliente client = GetClientByID(id);
            db.pEliminarCliente(client.co_cli, client.validador, "SERVER PROFIT WEB", "PROFIT WEB", null, client.rowguid);

            return client;
        }
    }
}