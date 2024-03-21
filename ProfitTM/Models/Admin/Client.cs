using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Client : ProfitAdmManager
    {
        public saCliente GetClientByID(string id)
        {
            saCliente client;

            try
            {
                client = db.saCliente.AsNoTracking().Include("saTipoCliente").Include("saSegmento").Include("saVendedor").Include("saCuentaIngEgr")
                    .Include("saPais").Include("saZona").Include("saCondicionPago").SingleOrDefault(c => c.co_cli == id);

                client.saTipoCliente.saCliente = null;
                client.saSegmento.saCliente = null;
                client.saVendedor.saCliente = null;
                client.saCuentaIngEgr.saCliente = null;
                client.saPais.saCliente = null;
                client.saZona.saCliente = null;

                if (client.saCondicionPago != null)
                    client.saCondicionPago.saCliente = null;
            }
            catch (Exception ex)
            {
                client = null;
                Incident.CreateIncident("ERROR BUSCANDO CLIENTE " + id, ex);
            }

            return client;
        }

        public List<saCliente> GetAllClients(bool full)
        {
            List<saCliente> clients;

            try
            {
                if (full)
                {
                    clients = db.saCliente.AsNoTracking().Include("saCondicionPago").Include("saVendedor").Include("saZona").Include("saCuentaIngEgr")
                        .Include("saSegmento").Include("saTipoCliente").Include("saPais").ToList();

                    foreach (saCliente client in clients)
                    {
                        if (client.saCondicionPago != null)
                            client.saCondicionPago.saCliente = null;

                        client.saVendedor.saCliente = null;
                        client.saZona.saCliente = null;
                        client.saCuentaIngEgr.saCliente = null;
                        client.saSegmento.saCliente = null;
                        client.saTipoCliente.saCliente = null;
                        client.saPais.saCliente = null;
                        client.saCliente1 = null;
                        client.saCliente2 = null;
                    }
                }
                else
                {
                    clients = db.saCliente.AsNoTracking().ToList();
                }
            }
            catch (Exception ex)
            {
                clients = null;
                Incident.CreateIncident("ERROR BUSCANDO CLIENTES", ex);
            }

            return clients;
        }

        public List<saCliente> GetMostActiveClients(DateTime fec_d, DateTime fec_h, int number, string sucur)
        {
            List<saCliente> clientes = new List<saCliente>();

            var sp = db.RepClienteMasVenta(fec_d, fec_h, null, null, null, null, null, null, null, number, sucur, null, null, null);
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

        public List<saCliente> GetMostMorousClients(int number)
        {
            List<saCliente> clients = new List<saCliente>();

            DateTime fec_h = DateTime.Now;
            DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

            var sp = db.RepEstadoCuentaCli(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator = sp.GetEnumerator();

            while (enumerator.MoveNext())
            {
                saCliente client = new saCliente();

                client.co_cli = enumerator.Current.co_prov;
                client.cli_des = enumerator.Current.prov_des;
                client.campo1 = ((enumerator.Current.tot_debe ?? 0) - (enumerator.Current.tot_haber ?? 0)).ToString();

                clients.Add(client);
            }

            clients = (from c in clients
                       group c.campo1 by (c.co_cli, c.cli_des) into g
                       select new saCliente
                       {
                           co_cli = g.Key.co_cli,
                           cli_des = g.Key.co_cli + " - " + g.Key.cli_des,
                           campo1 = Math.Round(g.Select(x => double.Parse(x)).Sum(), 2).ToString()

                       }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

            if (clients.Count > number)
                clients.RemoveRange(number, clients.Count - number);

            return clients;
        }

        public List<saDocumentoVenta> GetPendingDocs(string client)
        {
            List<saDocumentoVenta> docs = new List<saDocumentoVenta>();

            var sp = db.pSeleccionarDocumentosCliente(client, true, "");
            var enumerator = sp.GetEnumerator();

            while (enumerator.MoveNext())
            {
                saDocumentoVenta doc = new saDocumentoVenta();

                doc.co_tipo_doc = enumerator.Current.co_tipo_doc;
                doc.nro_doc = enumerator.Current.nro_doc;
                doc.fec_emis = enumerator.Current.fec_emis;
                doc.fec_venc = enumerator.Current.fec_venc;
                doc.total_bruto = enumerator.Current.total_bruto;
                doc.monto_imp = enumerator.Current.monto_imp;
                doc.total_neto = enumerator.Current.total_neto;
                doc.saldo = enumerator.Current.saldo;
                doc.co_mone = enumerator.Current.co_mone;
                doc.tasa = enumerator.Current.tasa;

                docs.Add(doc);
            }

            return docs;
        }
        
        public saCliente Add(saCliente client)
        {
            saCliente n_client;

            if (!client.sincredito)
            {
                client.plaz_pag = new Cond().GetCondByID(client.cond_pag).dias_cred;
            }

            var sp = db.pInsertarCliente(client.co_cli, client.login, client.password, client.salestax, client.cli_des, client.co_seg, client.co_zon, client.co_ven, 
                client.estado, client.inactivo, client.valido, client.sincredito, client.lunes, client.martes, client.miercoles, client.jueves, client.viernes, 
                client.sabado, client.domingo, client.direc1, client.direc2, client.dir_ent2, client.horar_caja, client.frecu_vist, client.telefonos, client.fax, 
                client.respons, client.fecha_reg, client.tip_cli, client.serialp, client.puntaje, client.Id, client.mont_cre, client.co_mone, client.cond_pag, 
                client.plaz_pag, client.desc_ppago, client.desc_glob, null, null, client.rif, client.contrib, client.dis_cen, client.nit, client.email, client.co_cta_ingr_egr,
                client.comentario, client.campo1, client.campo2, client.campo3, client.campo4, client.campo5, client.campo6, client.campo7, client.campo8,
                "PROFIT WEB", "SERVER PROFIT WEB", null, null, null, client.juridico, client.tipo_adi, client.matriz, client.co_tab, client.tipo_per, client.co_pais,
                client.ciudad, client.zip, client.website, client.contribu_e, client.rete_regis_doc, client.porc_esp, null, null, null, client.email_alterno);

            sp.Dispose();
            n_client = GetClientByID(client.co_cli);

            return n_client;
        }

        public saCliente Edit(saCliente client)
        {
            saCliente e_client;

            if (!client.sincredito)
            {
                client.plaz_pag = new Cond().GetCondByID(client.cond_pag).dias_cred;
            }

            var sp = db.pActualizarCliente(client.co_cli, client.co_cli, client.login, client.password, client.salestax, client.cli_des, client.co_seg, client.co_zon, 
                client.co_ven, client.estado, client.inactivo, client.valido, client.sincredito, client.lunes, client.martes, client.miercoles, client.jueves, 
                client.viernes, client.sabado, client.domingo, client.direc1, client.direc2, client.dir_ent2, client.horar_caja, client.frecu_vist, client.telefonos, 
                client.fax, client.respons, client.fecha_reg, client.tip_cli, client.serialp, client.puntaje, client.Id, client.mont_cre, client.co_mone, client.cond_pag, 
                client.plaz_pag, client.desc_ppago, client.desc_glob, client.rif, client.contrib, client.dis_cen, client.nit, client.email, client.co_cta_ingr_egr, 
                client.comentario, client.campo1, client.campo2, client.campo3, client.campo4, client.campo5, client.campo6, client.campo7, client.campo8, "PROFIT WEB", 
                null, "SERVER PROFIT WEB", null, null, null, client.juridico, client.tipo_adi, client.matriz, client.co_tab, client.tipo_per, client.co_pais, client.ciudad, 
                client.zip, client.website, client.contribu_e, client.rete_regis_doc, client.porc_esp, client.validador, client.rowguid, null, null, null, client.email_alterno);

            sp.Dispose();
            e_client = GetClientByID(client.co_cli);

            return e_client;
        }

        public saCliente Delete(string id)
        {
            saCliente client = GetClientByID(id);
            db.pEliminarCliente(client.co_cli, client.validador, "SERVER PROFIT WEB", "PROFIT WEB", null, client.rowguid);

            return client;
        }
    }
}