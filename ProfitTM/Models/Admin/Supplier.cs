using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Supplier : ProfitAdmManager
    {
        public saProveedor GetSupplierByID(string id)
        {
            saProveedor supplier;

            try
            {
                supplier = db.saProveedor.AsNoTracking().SingleOrDefault(c => c.co_prov == id);
            }
            catch (Exception ex)
            {
                supplier = null;
                Incident.CreateIncident("ERROR BUSCANDO PROVEEDOR " + id, ex);
            }

            return supplier;
        }

        public List<saProveedor> GetAllSuppliers()
        {
            List<saProveedor> suppliers;

            try
            {
                suppliers = db.saProveedor.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                suppliers = null;
                Incident.CreateIncident("ERROR BUSCANDO PROVEEDORES", ex);
            }

            return suppliers;
        }

        public List<saProveedor> GetMostActiveSuppliers(DateTime fec_d, DateTime fec_h, int number)
        {
            List<saProveedor> suppliers = new List<saProveedor>();

            var sp = db.RepProveedorMasCompra(fec_d, fec_h, null, null, number, null, null, null, null, null);
            var enumerator = sp.GetEnumerator();

            while (enumerator.MoveNext())
            {
                saProveedor supplier = new saProveedor();

                supplier.co_prov = enumerator.Current.co_prov.Trim();
                supplier.prov_des = enumerator.Current.prov_des.Trim();
                supplier.campo1 = Convert.ToDouble(enumerator.Current.Compra).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
                supplier.campo2 = Math.Round(Convert.ToDouble((enumerator.Current.Compra * 100) / enumerator.Current.Compra_total), 2).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));

                suppliers.Add(supplier);
            }

            return suppliers;
        }

        public List<saProveedor> GetMostMorousSuppliers(int number)
        {
            List<saProveedor> suppliers = new List<saProveedor>();

            DateTime fec_h = DateTime.Now;
            DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

            var sp = db.RepEstadoCuentaProv(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator = sp.GetEnumerator();

            while (enumerator.MoveNext())
            {
                saProveedor supplier = new saProveedor();

                supplier.co_prov = enumerator.Current.co_prov;
                supplier.prov_des = enumerator.Current.prov_des;
                supplier.campo1 = ((enumerator.Current.tot_debe ?? 0) - (enumerator.Current.tot_haber ?? 0)).ToString();

                suppliers.Add(supplier);
            }

            suppliers = (from s in suppliers
                        group s.campo1 by (s.co_prov, s.prov_des) into g
                        select new saProveedor
                        {
                            co_prov = g.Key.co_prov,
                            prov_des = g.Key.co_prov + " - " + g.Key.prov_des,
                            campo1 = Math.Round(g.Select(x => double.Parse(x)).Sum(), 2).ToString()

                        }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

            if (suppliers.Count > number)
                suppliers.RemoveRange(number, suppliers.Count - number);

            return suppliers;
        }

        public List<saDocumentoVenta> GetPendingDocs(string supplier)
        {
            List<saDocumentoVenta> docs = new List<saDocumentoVenta>();

            var sp = db.pSeleccionarDocumentosProveedor(supplier, true, "");
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

        public saProveedor Add(saProveedor supplier)
        {
            saProveedor n_supplier = new saProveedor();
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
                n_supplier = db.saProveedor.SingleOrDefault(c => c.rowguid == rowguid);
            }

            return n_supplier;
        }

        public saProveedor Edit(saProveedor supplier)
        {
            saProveedor e_supplier = new saProveedor();
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
                e_supplier = db.saProveedor.SingleOrDefault(c => c.rowguid == rowguid);
            }

            return e_supplier;
        }

        public saProveedor Delete(string id)
        {
            saProveedor supplier = GetSupplierByID(id);
            db.pEliminarProveedor(supplier.co_prov, supplier.validador, "SERVER PROFIT WEB", "PROFIT WEB", null, supplier.rowguid);

            return supplier;
        }
    }
}