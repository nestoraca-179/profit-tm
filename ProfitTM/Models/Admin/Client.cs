using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Client : ProfitAdmManager
    {
        public string co_cli { get; set; }
        public string cli_des { get; set; }

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

        public List<saCliente> GetMostActiveClients(DateTime fec_d, DateTime fec_h, int number)
        {
            List<saCliente> clientes = new List<saCliente>();

            var sp = db.RepClienteMasVenta(fec_d, fec_h, null, null, null, null, null, null, null, number, null, null, null, null);
            var enumerator = sp.GetEnumerator();

            while (enumerator.MoveNext())
            {
                saCliente cliente = new saCliente();

                cliente.co_cli = enumerator.Current.co_cli.Trim();
                cliente.cli_des = enumerator.Current.cli_des.Trim();
                cliente.campo1 = Convert.ToDouble(enumerator.Current.Venta).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
                cliente.campo2 = Math.Round(Convert.ToDouble((enumerator.Current.Venta * 100) / enumerator.Current.Venta_total), 2).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));

                clientes.Add(cliente);
            }

            return clientes;
        }

        public ProfitTMResponse GetMostMorousClients(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<saCliente> clientes = new List<saCliente>();

            try
            {
                DateTime fec_h = DateTime.Now;
                DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

                var sp = db.RepEstadoCuentaCli(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
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
                                cli_des = g.Key.co_cli + " - " + g.Key.cli_des,
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