using System;
using System.Linq;
using System.Data.Entity;
using System.Globalization;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Collect : ProfitAdmManager
    {
        public static saCobro GetCollectByID(string id)
        {
            saCobro invoice;

            try
            {
                invoice = db.saCobro.AsNoTracking().Single(c => c.cob_num == id);
            }
            catch (Exception ex)
            {
                invoice = null;
                Incident.CreateIncident("ERROR BUSCANDO COBRO " + id, ex);
            }

            return invoice;
        }

        public List<saCobro> GetAllCollects(int number, string sucur)
        {
            List<saCobro> collects;

            try
            {
                collects = db.saCobro.AsNoTracking().Where(c => c.co_sucu_in == sucur).Include("saCobroDocReng").Include("saCobroTPReng").Include("saCliente")
                    .Include("saVendedor").Include("saMoneda").OrderByDescending(c => c.fe_us_in).ThenBy(c => c.cob_num).Take(number).ToList();

                foreach (saCobro collect in collects)
                {
                    collect.saCliente.saCobro = null;
                    collect.saVendedor.saCobro = null;
                    collect.saMoneda.saCobro = null;
                    foreach (saCobroDocReng reng in collect.saCobroDocReng)
                    {
                        reng.saCobro = null;
                        reng.saDocumentoVenta = db.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == reng.co_tipo_doc && d.nro_doc == reng.nro_doc);
                        reng.saDocumentoVenta.saTipoDocumento = db.saTipoDocumento.AsNoTracking().Single(t => t.co_tipo_doc == reng.co_tipo_doc);
                    }
                    foreach (saCobroTPReng reng in collect.saCobroTPReng)
                    {
                        reng.saCobro = null;
                        reng.saCaja = db.saCaja.AsNoTracking().SingleOrDefault(c => c.cod_caja == reng.cod_caja);
                        reng.saCuentaBancaria = db.saCuentaBancaria.AsNoTracking().SingleOrDefault(c => c.cod_cta == reng.cod_cta);
                    }
                }
            }
            catch (Exception ex)
            {
                collects = null;
                Incident.CreateIncident("ERROR BUSCANDO COBROS", ex);
            }

            return collects;
        }

        public List<saCobroDocReng> GetCollectDocs(string co_cli)
        {
            List<saCobroDocReng> rengs = new List<saCobroDocReng>();
            saCobroDocReng reng = new saCobroDocReng();

            var sp = db.pObtenerDocumentosVenta(co_cli);
            var enumerator = sp.GetEnumerator();

            int i = 1;
            while (enumerator.MoveNext())
            {
                reng.reng_num = i;
                reng.co_tipo_doc = enumerator.Current.co_tipo_doc;
                reng.nro_doc = enumerator.Current.nro_doc;
                reng.mont_cob = 0;
                reng.saDocumentoVenta = db.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == reng.co_tipo_doc && d.nro_doc == reng.nro_doc);

                i++;
                rengs.Add(reng);
            }

            reng = null;
            return rengs;
        }

        public saCobro AddCollectFromInvoice(string doc_num, saCobroTPReng reng, saCobroRetenIvaReng r_iva, saCobroRentenReng r_islr, string user, string sucur, int conn)
        {
            saCobro new_collect = new saCobro();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        saFacturaVenta fact = context.saFacturaVenta.AsNoTracking().FirstOrDefault(f => f.doc_num == doc_num);
                        saDocumentoVenta doc_v = context.saDocumentoVenta.AsNoTracking().FirstOrDefault(d => d.co_tipo_doc == "FACT" && d.nro_doc == doc_num);

                        decimal igtf = Convert.ToDecimal(reng.co_us_in, new CultureInfo("en-US"));
                        decimal igtf_bs = 0;

                        bool isBox;
                        string n_coll = "", n_ajpm = "", n_move = "";
                        string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>7.1.01.01.001</CuentaContable></Carpeta01></InformacionContable>";

                        // SERIE COBRO
                        var sp_n_coll = context.pConsecutivoProximo(sucur, "COBRO").GetEnumerator();
                        if (sp_n_coll.MoveNext())
                            n_coll = sp_n_coll.Current;

                        sp_n_coll.Dispose();

                        // MOVIMIENTO DE CAJA / BANCO
                        if (reng.forma_pag == "EF") // EF (EFECTIVO)
                        {
                            isBox = true;

                            // ACTUALIZAR SALDO
                            var sp_s = context.pSaldoActualizar(user, "EF", "EF", reng.mont_doc, true, "COBRO", false);
                            sp_s.Dispose();

                            // SERIE MOVIMIENTO CAJA
                            var sp_n_move = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                            if (sp_n_move.MoveNext())
                                n_move = sp_n_move.Current;

                            sp_n_move.Dispose();

                            // INSERTAR MOVIMIENTO CAJA
                            var sp_m = context.pInsertarMovimientoCaja(n_move, DateTime.Now, "MOVIMIENTO CAJA COBRO " + n_coll, user.ToUpper(), fact.tasa, "I", "EF",
                                null, null, null, null, "110301001", reng.mont_doc, false, "COBRO", n_coll, null, false, false, false, false, null, DateTime.Now, null, 
                                null, null, null, null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                            sp_m.Dispose();

                            // AGREGAR VENTA A CAJA
                            Box.AddSale(fact.doc_num, reng.mont_doc, user, conn);
                        }
                        else // TP - DP (TRANSFERENCIA - DEPOSITO)
                        {
                            isBox = false;

                            // ACTUALIZAR SALDO
                            var sp_s = context.pSaldoActualizar(reng.cod_cta, reng.forma_pag, "TF", reng.mont_doc, true, "COBRO", false);
                            sp_s.Dispose();

                            // SERIE MOVIMIENTO BANCO
                            var sp_n_move = context.pConsecutivoProximo(sucur, "MOVB_NUM").GetEnumerator();
                            if (sp_n_move.MoveNext())
                                n_move = sp_n_move.Current;

                            sp_n_move.Dispose();

                            // INSERTAR MOVIMIENTO BANCO
                            decimal tasa = igtf > 0 ? fact.tasa : 1;

                            var sp_m = context.pInsertarMovimientoBanco(n_move, "MOVIMIENTO BANCO COBRO " + n_coll, reng.cod_cta, DateTime.Now, tasa, "TP", reng.num_doc, 
                                reng.mont_doc, "110301001", "COBRO", n_coll, 0, null, false, false, false, false, 0, null, null, DateTime.Now, null, null, null, null,
                                null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                            sp_m.Dispose();
                        }

                        // CREAR DOCUMENTO IGTF
                        if (igtf > 0)
                        {
                            igtf_bs = igtf * fact.tasa;

                            // SERIE AJPM
                            var sp_n_ajpm = context.pConsecutivoProximo(sucur, "DOC_VEN_AJPM").GetEnumerator();
                            if (sp_n_ajpm.MoveNext())
                                n_ajpm = sp_n_ajpm.Current;

                            sp_n_ajpm.Dispose();

                            // INSERTAR AJPM
                            var sp_i = context.pInsertarDocumentoVenta("AJPM", n_ajpm, fact.co_cli, fact.co_ven, "US$", null, null, fact.tasa, "RECARGO 3% IGTF FACT N° " + fact.doc_num,
                                DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, null, null, null, 0, 0, igtf_bs, 0, null, null, 0, igtf_bs, 0, 0, "7", 0, 0, 0,
                                0, null, null, dis_cen, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null,
                                sucur, user, "SERVER PROFIT WEB");

                            sp_i.Dispose();
                        }

                        decimal t_fact = reng.mont_doc;
                        decimal amount = igtf > 0 ? Math.Round(reng.mont_doc * fact.tasa, 2) : reng.mont_doc;

                        if (igtf > 0)
                            t_fact = Math.Round((t_fact - igtf) * fact.tasa, 2);

                        // ACTUALIZAR FACTURA
                        fact.saldo -= t_fact;
                        fact.status = fact.saldo > 0 ? "1" : "2";
                        context.Entry(fact).State = EntityState.Modified;

                        // ACTUALIZAR DOCUMENTO
                        doc_v.saldo -= t_fact;
                        context.Entry(doc_v).State = EntityState.Modified;

                        // INSERTAR COBRO
                        var sp_c = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, amount, null,
                            "COBRO FACT " + doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        sp_c.Dispose();

                        // INSERTAR DOC COBRO
                        int r = 1;

                        if (igtf > 0)
                        {
                            var sp_d1 = context.pInsertarRenglonesDocCobro(r, n_coll, "AJPM", n_ajpm, igtf_bs, 0, 0, 0, 0, null, null, null, null,
                                Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                            sp_d1.Dispose();
                            r++;
                        }

                        var sp_d2 = context.pInsertarRenglonesDocCobro(r, n_coll, "FACT", fact.doc_num, t_fact, 0, 0, 0, 0, null, null, null, null,
                            Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                        sp_d2.Dispose();

                        // INSERTAR TP COBRO
                        string mov_c = isBox ? n_move : null;
                        string mov_b = !isBox ? n_move : null;
                        string n_doc = !isBox ? reng.num_doc : null;
                        string c_caj = isBox ? user.ToUpper() : null;
                        string c_cta = !isBox ? reng.cod_cta : null;
                        string c_ban = !isBox ? new Account().GetBankAccountByID(reng.cod_cta).co_ban : null;

                        var sp_t = context.pInsertarRenglonesTPCobro(1, n_coll, reng.forma_pag, mov_c, mov_b, n_doc, false, amount, c_cta, c_ban, null, null, 
                            c_caj, DateTime.Now, sucur, user, null, null, "SERVER PROFIT WEB");

                        sp_t.Dispose();

                        // AGREGANDO RETENCIONES
                        if (r_iva != null || r_islr != null)
                        {
                            int nr = 1;
                            Guid f_guid = Guid.NewGuid();
                            string n_ret = "", n_ivan = "", n_islr = "";
                            decimal mont_cob = (r_iva != null ? r_iva.monto_ret_imp : 0) + (r_islr != null ? r_islr.monto_reten : 0);

                            // SERIE COBRO RET
                            var sp_n_ret = context.pConsecutivoProximo(sucur, "COBRO").GetEnumerator();
                            if (sp_n_ret.MoveNext())
                                n_ret = sp_n_ret.Current;

                            sp_n_ret.Dispose();

                            // INSERTAR COBRO RET
                            var sp_c_ret = context.pInsertarCobro(n_ret, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, 0, null,
                                "RET IVA-ISLR FACT " + fact.doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                            sp_c_ret.Dispose();

                            // INSERTAR DOC FACT COBRO RET
                            var sp_d_ret_1 = context.pInsertarRenglonesDocCobro(nr, n_ret, "FACT", fact.doc_num, mont_cob, 0, 0, r_iva != null ? r_iva.monto_ret_imp : 0,
                                r_islr != null ? r_islr.monto_reten : 0, null, null, null, null, f_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                            sp_d_ret_1.Dispose();

                            // RETENCION DE IVA
                            if (r_iva != null)
                            {
                                string rif_c = new Client().GetClientByID(fact.co_cli).rif;
                                Guid iva_guid = Guid.NewGuid();
                                nr++;

                                // SERIE DOC IVAN
                                var sp_n_ivan = context.pConsecutivoProximo(sucur, "DOC_VEN_IVAN").GetEnumerator();
                                if (sp_n_ivan.MoveNext())
                                    n_ivan = sp_n_ivan.Current;

                                sp_n_ivan.Dispose();

                                // INSERTAR DOC IVAN
                                var sp_d_ivan = context.pInsertarDocumentoVenta("IVAN", n_ivan, fact.co_cli, fact.co_ven, fact.co_mone, null, null, fact.tasa,
                                    "RET IVA FACT " + fact.doc_num, DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, "COBRO", n_coll, null, 0, 0,
                                    r_iva.monto_ret_imp, 0, null, null, 0, r_iva.monto_ret_imp, 0, 0, "7", 0, 0, 0, 0, r_iva.num_comprobante, null, null, 0, 0, 0,
                                    0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, sucur, user,
                                    "SERVER PROFIT WEB");

                                sp_d_ivan.Dispose();

                                // INSERTAR DOC IVAN COBRO RET
                                var sp_d_ret_2 = context.pInsertarRenglonesDocCobro(nr, n_ret, "IVAN", n_ivan, r_iva.monto_ret_imp, 0, 0, 0, 0, null, null, f_guid, null,
                                    iva_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                                sp_d_ret_2.Dispose();

                                // INSERTAR RENG IVA COBRO RET
                                var sp_r_iva = context.pInsertarRenglonesRetenIvaCobro(iva_guid, 1, Connection.GetConnByID(conn.ToString()).RIF.Replace("-", ""), r_iva.periodo_impositivo,
                                    r_iva.fecha_documento, "C", "FACT", rif_c, fact.doc_num, fact.n_control, fact.total_neto, fact.total_bruto, r_iva.monto_ret_imp, "0", r_iva.num_comprobante,
                                    0, 16, "0", false, null, null, sucur, user, "SERVER PROFIT WEB");

                                sp_r_iva.Dispose();
                            }

                            // RETENCION DE ISLR
                            if (r_islr != null)
                            {
                                Guid islr_guid = Guid.NewGuid();
                                nr++;

                                // SERIE DOC ISLR
                                var sp_n_islr = context.pConsecutivoProximo(sucur, "DOC_VEN_ISLR").GetEnumerator();
                                if (sp_n_islr.MoveNext())
                                    n_islr = sp_n_islr.Current;

                                sp_n_islr.Dispose();

                                // INSERTAR DOC ISLR
                                var sp_d_islr = context.pInsertarDocumentoVenta("ISLR", n_islr, fact.co_cli, fact.co_ven, fact.co_mone, null, null, fact.tasa,
                                    "RET IVA FACT " + fact.doc_num, DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, "COBRO", n_coll, null, 0, 0,
                                    r_islr.monto_reten, 0, null, null, 0, r_islr.monto_reten, 0, 0, "7", 0, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0, null,
                                    false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, sucur, user, "SERVER PROFIT WEB");

                                sp_d_islr.Dispose();

                                // INSERTAR DOC ISLR COBRO RET
                                var sp_d_ret_3 = context.pInsertarRenglonesDocCobro(nr, n_ret, "ISLR", n_islr, r_islr.monto_reten, 0, 0, 0, 0, null, null, f_guid, null,
                                    islr_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                                sp_d_ret_3.Dispose();

                                // INSERTAR RENG IVA COBRO RET
                                var sp_r_islr = context.pInsertarRenglonesRetenCobro(islr_guid, fact.total_bruto, r_islr.monto_reten, 0, r_islr.porc_retn, fact.total_bruto,
                                    false, r_islr.co_islr, 1, null, null, sucur, user, "SERVER PROFIT WEB", null);

                                sp_r_islr.Dispose();
                            }

                            // INSERTAR TP FACT COBRO RET
                            var sp_t_ret = context.pInsertarRenglonesTPCobro(1, n_ret, "EF", null, null, null, false, 0, null, null, null, null, "001", DateTime.Now, sucur,
                                user, null, null, "SERVER PROFIT WEB");

                            sp_t_ret.Dispose();

                            // ACTUALIZAR FACTURA
                            fact.saldo -= mont_cob;
                            fact.status = fact.saldo > 0 ? "1" : "2";
                            context.Entry(fact).State = EntityState.Modified;

                            // ACTUALIZAR DOCUMENTO
                            doc_v.saldo -= mont_cob;
                            context.Entry(doc_v).State = EntityState.Modified;
                        }

                        context.SaveChanges();
                        tran.Commit();

                        new_collect = GetCollectByID(n_coll);
                        new_collect.campo1 = fact.status; // STATUS DE FACTURA
                        new_collect.campo2 = fact.saldo.ToString(); // SALDO RESTANTE EN BSD
                        new_collect.campo3 = Math.Round(fact.saldo / fact.tasa, 2).ToString(); // SALDO RESTANTE EN $USD
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO COBRO DE CONTADO", ex);

                        throw ex;
                    }
                }
            }
            
            return new_collect;
        }
    
        public saCobro AddCollectFromInvoice2(saCobro cob, string user, string sucur, int conn)
        {
            saCobro collect = new saCobro();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        decimal total_cob = 0, total_fact = 0, igtf = 0;
                        string n_coll = "", n_adel = "", n_ajpm = "", n_mov_c = "", n_mov_b = "", n_ret = "";

                        // DOCUMENTOS DE FACTURA
                        saFacturaVenta fact = context.saFacturaVenta.AsNoTracking().FirstOrDefault(f => f.doc_num == cob.campo1);
                        saDocumentoVenta doc_v = context.saDocumentoVenta.AsNoTracking().FirstOrDefault(d => d.co_tipo_doc == "FACT" && d.nro_doc == cob.campo1);

                        // SALDO RESTANTE DE FACTURA
                        decimal remaining = Convert.ToDecimal(cob.campo2, new CultureInfo("en-US"));

                        // CAMBIO DE DEVOLUCION
                        decimal change = Math.Round(Convert.ToDecimal(cob.campo3, new CultureInfo("en-US")), 2);
                        bool isAdvance = Convert.ToBoolean(cob.campo4);

                        // APLICAR DIFERENCIA DE RETENCIONES
                        bool calcRetIva = Convert.ToBoolean(cob.campo5);
                        bool calcRetIslr = Convert.ToBoolean(cob.campo6);

                        // RENGLONES DE PAGO
                        List<saCobroTPReng> rengs = cob.saCobroTPReng.Where(re => re.forma_pag != "IVAN" && re.forma_pag != "ISLR").ToList();

                        #region REGISTRO DE ADELANTO
                        if (rengs.Count > 0)
                        {
                            // SERIE COBRO ADELANTO
                            n_coll = GetNextConsec(sucur, "COBRO");
                            total_cob = Math.Round(rengs.Select(re => re.mont_doc).Sum(), 2);

                            if (change > 0 && !isAdvance)
                                total_cob -= change;

                            foreach (saCobroTPReng reng in rengs)
                            {
                                // total_cob += reng.mont_doc;
                                if (reng.forma_pag == "EF") // EFECTIVO
                                {
                                    decimal mont_doc = change > 0 && !isAdvance ? reng.mont_doc - change : reng.mont_doc;
                                    decimal mont_doc_usd = Math.Round((mont_doc) / fact.tasa, 2); // MONTO EN USD
                                    string co_cta_ingr_egr = context.saCaja.AsNoTracking().Single(b => b.cod_caja == user.ToUpper()).campo1;
                                    // string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>7.1.01.01.001</CuentaContable></Carpeta01></InformacionContable>";

                                    // ACTUALIZAR SALDO CAJA
                                    var sp_s = context.pSaldoActualizar(user, "EF", "EF", mont_doc_usd, true, "COBRO", false);
                                    sp_s.Dispose();

                                    // SERIE MOVIMIENTO CAJA
                                    n_mov_c = GetNextConsec(sucur, "MOVC_NUM");

                                    // INSERTAR MOVIMIENTO CAJA
                                    var sp_m = context.pInsertarMovimientoCaja(n_mov_c, DateTime.Now, "MOVIMIENTO CAJA COBRO " + n_coll, user.ToUpper(), fact.tasa, "I", "EF",
                                        null, null, null, null, co_cta_ingr_egr, mont_doc_usd, false, "COBRO", n_coll, null, false, false, false, false, null, DateTime.Now,
                                        null, null, null, null, null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                                    sp_m.Dispose();

                                    // AGREGAR VENTA A CAJA
                                    Box.AddSale(string.Format("{0} (COB. {1})", fact.doc_num.Trim(), n_coll.Trim()), mont_doc_usd, user, conn);

                                    // MONTO AJPM DE 3% IGTF
                                    igtf = Convert.ToDecimal(reng.mov_num_c, new CultureInfo("en-US"));

                                    // SERIE AJPM
                                    // n_ajpm = GetNextConsec(sucur, "DOC_VEN_AJPM");

                                    // INSERTAR AJPM
                                    /*var sp_i = context.pInsertarDocumentoVenta("AJPM", n_ajpm, fact.co_cli, fact.co_ven, "US$", null, null, fact.tasa, "RECARGO 3% IGTF FACT N° " + fact.doc_num,
                                        DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, null, null, null, 0, 0, igtf, 0, null, null, 0, igtf, 0, 0, "7",
                                        0, 0, 0, 0, null, null, dis_cen, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null,
                                        null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                                    sp_i.Dispose();*/
                                }
                                else if (reng.forma_pag == "TP") // TRANSFERENCIA
                                {
                                    // ACTUALIZAR SALDO BANCO
                                    var sp_s = context.pSaldoActualizar(reng.cod_cta, reng.forma_pag, "TF", reng.mont_doc, true, "COBRO", false);
                                    sp_s.Dispose();

                                    // SERIE MOVIMIENTO BANCO
                                    n_mov_b = GetNextConsec(sucur, "MOVB_NUM");

                                    // INSERTAR MOVIMIENTO BANCO
                                    var sp_m = context.pInsertarMovimientoBanco(n_mov_b, "MOVIMIENTO BANCO COBRO " + n_coll, reng.cod_cta, reng.fecha_che, 1, "TP", reng.num_doc,
                                        reng.mont_doc, "110301001", "COBRO", n_coll, 0, null, false, false, false, false, 0, null, null, reng.fecha_che, null, null, null, null,
                                        null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                                    sp_m.Dispose();

                                    // AGREGAR TRANSFERENCIA A CAJA
                                    string descrip = string.Format("{0} (COB. {1})", fact.doc_num.Trim(), n_coll.Trim());
                                    Transfer.AddTransfer(user, reng.mont_doc, reng.cod_cta, reng.num_doc, reng.fecha_che, descrip, conn);
                                }
                            }

                            // total_fact = total_cob;

                            // if (igtf > 0) // RESTANDO IGTF DEL TOTAL A ABONAR A LA FACTURA
                            //     total_fact = total_cob - igtf;

                            // ACTUALIZAR FACTURA
                            // fact.saldo -= total_fact;
                            // fact.status = fact.saldo > 0 ? "1" : "2";
                            // context.Entry(fact).State = EntityState.Modified;

                            // ACTUALIZAR DOCUMENTO
                            // doc_v.saldo -= total_fact;
                            // context.Entry(doc_v).State = EntityState.Modified;

                            // SERIE ADELANTO
                            n_adel = GetNextConsec(sucur, "DOC_VEN_ADEL");

                            // INSERTAR ADELANTO
                            var sp_a = context.pInsertarDocumentoVenta("ADEL", n_adel, fact.co_cli, fact.co_ven, "US$", null, null, fact.tasa, "ADELANTO DE FACTURA " + fact.doc_num,
                                DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, null, null, null, 0, total_cob, total_cob, 0, null, null, 0, total_cob, 0, 0, "7",
                                0, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null,
                                null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                            sp_a.Dispose();

                            // INSERTAR COBRO ADELANTO
                            var sp_c = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, total_cob, null,
                                "ADELANTO FACT " + fact.doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                            sp_c.Dispose();

                            // INSERTAR DOC COBRO ADELANTO
                            // int r = 1;
                            var sp_d1 = context.pInsertarRenglonesDocCobro(1, n_coll, "ADEL", n_adel, 0 /*MONTO*/, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(),
                                null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                            sp_d1.Dispose();

                            //if (igtf > 0)
                            //{
                            //    var sp_d1 = context.pInsertarRenglonesDocCobro(r, n_coll, "AJPM", n_ajpm, igtf, 0, 0, 0, 0, null, null, null, null,
                            //        Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                            //    sp_d1.Dispose();
                            //    r++;
                            //}

                            //var sp_d2 = context.pInsertarRenglonesDocCobro(r, n_coll, "FACT", fact.doc_num, total_fact, 0, 0, 0, 0, null, null, null, null,
                            //    Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                            //sp_d2.Dispose();

                            // INSERTAR TP COBRO ADELANTO
                            int r = 1;
                            foreach (saCobroTPReng reng in rengs)
                            {
                                bool isBox = reng.forma_pag == "EF";

                                string mov_c = isBox ? n_mov_c : null;
                                string mov_b = !isBox ? n_mov_b : null;
                                string n_doc = !isBox ? reng.num_doc : null;
                                string c_caj = isBox ? user.ToUpper() : null;
                                string c_cta = !isBox ? reng.cod_cta : null;
                                string c_ban = !isBox ? new Account().GetBankAccountByID(reng.cod_cta).co_ban : null;
                                decimal mont_doc = isBox && change > 0 && !isAdvance ? reng.mont_doc - change : reng.mont_doc;
                                DateTime fecha = isBox ? DateTime.Now : reng.fecha_che;

                                var sp_t = context.pInsertarRenglonesTPCobro(r, n_coll, reng.forma_pag, mov_c, mov_b, n_doc, false, mont_doc, c_cta, c_ban, null, null,
                                    c_caj, fecha, sucur, user, null, null, "SERVER PROFIT WEB");
                                sp_t.Dispose();

                                r++;
                            }

                            total_fact = total_cob;
                        }
                        #endregion

                        #region REGISTRO DE IGTF
                        if (igtf > 0)
                        {
                            // CREACION DE DOCUMENTO AJPM DE 3% IGTF
                            string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>7.1.01.01.001</CuentaContable></Carpeta01></InformacionContable>";

                            // SERIE AJPM
                            n_ajpm = GetNextConsec(sucur, "DOC_VEN_AJPM");

                            // INSERTAR AJPM
                            var sp_i = context.pInsertarDocumentoVenta("AJPM", n_ajpm, fact.co_cli, fact.co_ven, "US$", null, null, fact.tasa, "RECARGO 3% IGTF FACT N° " + fact.doc_num,
                                DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, null, null, null, 0, 0, igtf, 0, null, null, 0, igtf, 0, 0, "7",
                                0, 0, 0, 0, null, null, dis_cen, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null,
                                null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                            sp_i.Dispose();

                            total_fact = total_cob - igtf; // RESTANDO IGTF DEL TOTAL A ABONAR A LA FACTURA
                        }
                        #endregion

                        #region REGISTRO DE CRUCE DE AJPM - FACT - ADEL
                        if (rengs.Count > 0)
                        {
                            decimal amount = fact.saldo - GetAmountRets(fact, calcRetIva, calcRetIslr); // TOTAL A COBRAR MENOS RETENCIONES
                            decimal diff = total_fact - amount; // (RECIBIDO SIN IGTF - TOTAL FACTURA CON/SIN RETENCIONES)

                            if (diff >= 0) // PAGO JUSTO O SALDO A FAVOR DEL CIENTE
                                total_fact = amount;

                            // SERIE COBRO CRUCE
                            n_coll = GetNextConsec(sucur, "COBRO");

                            // ACTUALIZAR FACTURA
                            fact.saldo -= total_fact;
                            fact.status = fact.saldo > 0 ? "1" : "2";
                            context.Entry(fact).State = EntityState.Modified;

                            // ACTUALIZAR DOCUMENTO
                            doc_v.saldo -= total_fact;
                            context.Entry(doc_v).State = EntityState.Modified;

                            // INSERTAR COBRO CRUCE
                            var sp_cc = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, 0, null,
                                "CRUCE FACT " + fact.doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                            sp_cc.Dispose();

                            // INSERTAR DOC COBRO CRUCE
                            int r = 1;

                            if (igtf > 0)
                            {
                                var sp_dd1 = context.pInsertarRenglonesDocCobro(r, n_coll, "AJPM", n_ajpm, igtf, 0, 0, 0, 0, null, null, null, null,
                                    Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                                sp_dd1.Dispose();
                                r++;
                            }

                            var sp_dd2 = context.pInsertarRenglonesDocCobro(r, n_coll, "FACT", fact.doc_num, total_fact, 0, 0, 0, 0, null, null, null, null,
                                Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                            sp_dd2.Dispose();

                            r++;
                            var sp_dd3 = context.pInsertarRenglonesDocCobro(r, n_coll, "ADEL", n_adel, total_fact + igtf, 0, 0, 0, 0, null, null, null, null,
                                Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                            sp_dd3.Dispose();

                            // INSERTAR TP COBRO CRUCE
                            var sp_tt = context.pInsertarRenglonesTPCobro(1, n_coll, "EF", null, null, null, false, 0, null, null, null, null, "001", DateTime.Now,
                                sucur, user, null, null, "SERVER PROFIT WEB");
                            sp_tt.Dispose();

                            // ACTUALIZAR ADELANTO
                            saDocumentoVenta doc_adel = context.saDocumentoVenta.AsNoTracking().First(d => d.co_tipo_doc == "ADEL" && d.nro_doc == n_adel);
                            doc_adel.saldo -= (total_fact + igtf);
                            context.Entry(doc_adel).State = EntityState.Modified;
                        }
                        #endregion

                        #region REGISTRO DE RETENCIONES
                        // RENGLONES DE RETENCIONES
                        List<saCobroTPReng> rets = cob.saCobroTPReng.Where(re => re.forma_pag == "IVAN" || re.forma_pag == "ISLR").ToList();
                        if (rets.Count > 0)
                        {
                            int nr = 1;
                            Guid f_guid = Guid.NewGuid();
                            string n_ivan = "", n_islr = "";

                            saCobroTPReng r_iva = rets.FirstOrDefault(re => re.forma_pag == "IVAN");
                            saCobroTPReng r_islr = rets.FirstOrDefault(re => re.forma_pag == "ISLR");
                            decimal mont_cob = (r_iva != null ? r_iva.mont_doc : 0) + (r_islr != null ? r_islr.mont_doc : 0);

                            // SERIE COBRO RETENCIONES
                            n_ret = GetNextConsec(sucur, "COBRO");

                            // INSERTAR COBRO RETENCIONES
                            var sp_c_ret = context.pInsertarCobro(n_ret, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, 0, null,
                                "RETENCIONES FACT " + fact.doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                            sp_c_ret.Dispose();

                            // INSERTAR DOC FACT COBRO RETENCIONES
                            var sp_d_ret_1 = context.pInsertarRenglonesDocCobro(nr, n_ret, "FACT", fact.doc_num, mont_cob, 0, 0, r_iva != null ? r_iva.mont_doc : 0,
                                r_islr != null ? r_islr.mont_doc : 0, null, null, null, null, f_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                            sp_d_ret_1.Dispose();

                            // RETENCION DE IVA
                            if (r_iva != null)
                            {
                                nr++;
                                Guid iva_guid = Guid.NewGuid();
                                string rif_c = new Client().GetClientByID(fact.co_cli).rif, periodImp = r_iva.mov_num_c;

                                // SERIE DOC IVAN
                                n_ivan = GetNextConsec(sucur, "DOC_VEN_IVAN");

                                // INSERTAR DOC IVAN
                                var sp_d_ivan = context.pInsertarDocumentoVenta("IVAN", n_ivan, fact.co_cli, fact.co_ven, fact.co_mone, null, null, fact.tasa,
                                    "RET IVA FACT " + fact.doc_num, DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, "COBRO", n_ret, null, 0, 0,
                                    r_iva.mont_doc, 0, null, null, 0, r_iva.mont_doc, 0, 0, "7", 0, 0, 0, 0, r_iva.num_doc, null, null, 0, 0, 0, 0, 0, 0, 0, null, 
                                    false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                                sp_d_ivan.Dispose();

                                // INSERTAR DOC IVAN COBRO RET
                                var sp_d_ret_2 = context.pInsertarRenglonesDocCobro(nr, n_ret, "IVAN", n_ivan, r_iva.mont_doc, 0, 0, 0, 0, null, null, f_guid, null,
                                    iva_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                                sp_d_ret_2.Dispose();

                                // INSERTAR RENG IVA COBRO RET
                                var sp_r_iva = context.pInsertarRenglonesRetenIvaCobro(iva_guid, 1, Connection.GetConnByID(conn.ToString()).RIF.Replace("-", ""), 
                                    decimal.Parse(periodImp), DateTime.Now, "C", "FACT", rif_c, fact.doc_num, fact.n_control, fact.total_neto, fact.total_bruto, 
                                    r_iva.mont_doc, "0", r_iva.num_doc, 0, 16, "0", false, null, null, sucur, user, "SERVER PROFIT WEB");
                                sp_r_iva.Dispose();
                            }

                            // RETENCION DE ISLR
                            if (r_islr != null)
                            {
                                nr++;
                                Guid islr_guid = Guid.NewGuid();

                                // SERIE DOC ISLR
                                n_islr = GetNextConsec(sucur, "DOC_VEN_ISLR");

                                // INSERTAR DOC ISLR
                                var sp_d_islr = context.pInsertarDocumentoVenta("ISLR", n_islr, fact.co_cli, fact.co_ven, fact.co_mone, null, null, fact.tasa,
                                    "RET ISLR FACT " + fact.doc_num, DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, "COBRO", n_ret, null, 0, 0,
                                    r_islr.mont_doc, 0, null, null, 0, r_islr.mont_doc, 0, 0, "7", 0, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0, null,
                                    false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                                sp_d_islr.Dispose();

                                // INSERTAR DOC ISLR COBRO RET
                                var sp_d_ret_3 = context.pInsertarRenglonesDocCobro(nr, n_ret, "ISLR", n_islr, r_islr.mont_doc, 0, 0, 0, 0, null, null, f_guid, null,
                                    islr_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                                sp_d_ret_3.Dispose();

                                // INSERTAR RENG ISLR COBRO RET
                                var sp_r_islr = context.pInsertarRenglonesRetenCobro(islr_guid, fact.total_bruto, r_islr.mont_doc, 0, 2, fact.total_bruto,
                                    false, "055", 1, null, null, sucur, user, "SERVER PROFIT WEB", null);
                                sp_r_islr.Dispose();
                            }
                            
                            // INSERTAR TP FACT COBRO RETENCIONES
                            var sp_t_ret = context.pInsertarRenglonesTPCobro(1, n_ret, "EF", null, null, null, false, 0, null, null, null, null, "001", DateTime.Now, sucur,
                                user, null, null, "SERVER PROFIT WEB");
                            sp_t_ret.Dispose();

                            // ACTUALIZAR FACTURA
                            fact.saldo -= mont_cob;
                            fact.status = fact.saldo > 0 ? "1" : "2";
                            context.Entry(fact).State = EntityState.Modified;

                            // ACTUALIZAR DOCUMENTO
                            doc_v.saldo -= mont_cob;
                            context.Entry(doc_v).State = EntityState.Modified;
                        }
                        #endregion

                        context.SaveChanges();
                        tran.Commit();

                        collect = GetCollectByID(rengs.Count > 0 ? n_coll : n_ret);
                        collect.campo1 = fact.status; // STATUS DE FACTURA
                        collect.campo2 = fact.saldo.ToString(); // SALDO RESTANTE EN BSD
                        collect.campo3 = Math.Round(fact.saldo / fact.tasa, 2).ToString(); // SALDO RESTANTE EN $USD
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO COBRO DE CONTADO 2", ex);

                        throw ex;
                    }
                }
            }

            return collect;
        }

        private decimal GetAmountRets(saFacturaVenta fact, bool calcRetIva, bool calcRetIslr)
        {
            decimal amount = 0;

            if (calcRetIva)
            {
                decimal porc = db.saCliente.AsNoTracking().First(c => c.co_cli == fact.co_cli).porc_esp;
                decimal ret_ivan = Math.Round((fact.monto_imp * porc) / 100, 2);
                amount += ret_ivan;
            }

            if (calcRetIslr)
            {
                decimal ret_islr = Math.Round((fact.total_bruto * 2) / 100, 2);
                amount += ret_islr;
            }

            return amount;
        }
    }
}