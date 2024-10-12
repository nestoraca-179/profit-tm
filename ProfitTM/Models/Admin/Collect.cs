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
            saCobro collect;

            try
            {
                collect = db.saCobro.AsNoTracking().Single(c => c.cob_num == id);
            }
            catch (Exception ex)
            {
                collect = null;
                Incident.CreateIncident("ERROR BUSCANDO COBRO " + id, ex);
            }

            return collect;
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

        public saCobro AddCollectFromInvoice(saCobro cob, string user, string sucur, int conn)
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

                        const string CO_MONE = "USD"; // CAMBIAR
                        const string COD_CAJA = "01"; // CAMBIAR

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

                        if (fact.fec_emis > cob.fecha)
                            throw new Exception(string.Format("La Fecha del Cobro ({0}) no puede ser menor a la Fecha de la Factura ({1})", 
                                cob.fecha.ToString("dd/MM/yyyy"), fact.fec_emis.ToString("dd/MM/yyyy")));

                        if (fact.saldo > 0)
                        {
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
                                        string dis_cen_m = "<InformacionContable><Carpeta01><CuentaContable>1.1.01.01.002</CuentaContable></Carpeta01></InformacionContable>";
                                        var sp_m = context.pInsertarMovimientoCaja(n_mov_c, DateTime.Now, "MOVIMIENTO CAJA COBRO " + n_coll, user.ToUpper(), fact.tasa, "I", "EF",
                                            null, null, null, null, co_cta_ingr_egr, mont_doc_usd, false, "COBRO", n_coll, null, false, false, false, false, null, DateTime.Now,
                                            null, null, null, null, dis_cen_m, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                                        sp_m.Dispose();

                                        // AGREGAR VENTA A CAJA
                                        Box.AddSale(string.Format("{0} (COB. {1})", fact.doc_num.Trim(), n_coll.Trim()), mont_doc_usd, user, conn);

                                        // MONTO AJPM DE 3% IGTF
                                        igtf = Convert.ToDecimal(reng.mov_num_c, new CultureInfo("en-US"));

										#region CODIGO ANTERIOR
										// SERIE AJPM
										// n_ajpm = GetNextConsec(sucur, "DOC_VEN_AJPM");

										// INSERTAR AJPM
										/*var sp_i = context.pInsertarDocumentoVenta("AJPM", n_ajpm, fact.co_cli, fact.co_ven, CO_MONE, null, null, fact.tasa, "RECARGO 3% IGTF FACT N° " + fact.doc_num,
                                            DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, null, null, null, 0, 0, igtf, 0, null, null, 0, igtf, 0, 0, "7",
                                            0, 0, 0, 0, null, null, dis_cen, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null,
                                            null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                                        sp_i.Dispose();*/
										#endregion
									}
									else if (reng.forma_pag == "TP") // TRANSFERENCIA
                                    {
                                        // ACTUALIZAR SALDO BANCO
                                        var sp_s = context.pSaldoActualizar(reng.cod_cta, reng.forma_pag, "TF", reng.mont_doc, true, "COBRO", false);
                                        sp_s.Dispose();

                                        // SERIE MOVIMIENTO BANCO
                                        n_mov_b = GetNextConsec(sucur, "MOVB_NUM");

                                        // VERIFICACION MOVIMIENTO EXISTENTE
                                        saMovimientoBanco exist = context.saMovimientoBanco.AsNoTracking().FirstOrDefault(m => m.cod_cta == reng.cod_cta && m.doc_num == reng.num_doc);

                                        if (exist != null)
                                            throw new Exception("La transferencia ha sido agregada anteriormente bajo el Movimiento de Banco Nro. " + exist.mov_num.Trim());

                                        // VERIFICACION DE ULT. CONCILIACION
                                        DateTime fec_ult_conc = context.saMovimientoBanco.AsNoTracking().Where(m => m.conciliado && !m.anulado && m.cod_cta == reng.cod_cta)
                                            .Select(m => m.fec_con ?? new DateTime()).Max();

                                        if (reng.fecha_che < fec_ult_conc)
											throw new Exception(string.Format("La fecha de la transferencia ({0}) es menor a la ultima fecha de conciliacion de la cuenta bancaria ({1}).", 
                                                reng.fecha_che.ToString("dd/MM/yyyy"), fec_ult_conc.ToString("dd/MM/yyyy")));

                                        // VERIFICACION DE ULT. CONTABILIZACION
                                        DateTime fec_ult_cont = context.par_emp.AsNoTracking().First().fec_cont;

										if (reng.fecha_che < fec_ult_cont)
											throw new Exception(string.Format("La fecha de la transferencia ({0}) es menor a la ultima fecha de contabilizacion ({1}).",
                                                reng.fecha_che.ToString("dd/MM/yyyy"), fec_ult_cont.ToString("dd/MM/yyyy")));

                                        if (cob.fecha < fec_ult_cont)
                                            throw new Exception(string.Format("La fecha del cobro ({0}) es menor a la ultima fecha de contabilizacion ({1}).",
                                                cob.fecha.ToString("dd/MM/yyyy"), fec_ult_cont.ToString("dd/MM/yyyy")));

                                        // INSERTAR MOVIMIENTO BANCO
                                        var sp_m = context.pInsertarMovimientoBanco(n_mov_b, "MOVIMIENTO BANCO COBRO " + n_coll, reng.cod_cta, reng.fecha_che, 1, "TP", reng.num_doc,
                                            reng.mont_doc, "110301001", "COBRO", n_coll, 0, null, false, false, false, false, 0, null, null, reng.fecha_che, null, null, null, null,
                                            null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                                        sp_m.Dispose();

                                        // AGREGAR TRANSFERENCIA A CAJA
                                        string descrip = string.Format("{0} (COB. {1})", fact.doc_num.Trim(), n_coll.Trim());
                                        // Transfer.AddTransfer(user, reng.mont_doc, reng.cod_cta, reng.num_doc, reng.fecha_che, descrip, conn); QUITAR COMENTARIO
                                    }
                                }

								#region CODIGO ANTERIOR
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
								#endregion

								// SERIE ADELANTO
								n_adel = GetNextConsec(sucur, "DOC_VEN_ADEL");

                                // INSERTAR ADELANTO
                                var sp_a = context.pInsertarDocumentoVenta("ADEL", n_adel, fact.co_cli, fact.co_ven, CO_MONE, null, null, fact.tasa, "ADELANTO DE FACTURA " + fact.doc_num,
                                    cob.fecha, cob.fecha, cob.fecha, false, true, false, "COBRO", n_coll, null, 0, total_cob, total_cob, 0, null, null, 0, total_cob, 0, 0, "7",
                                    0, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null,
                                    null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                                sp_a.Dispose();

                                // INSERTAR COBRO ADELANTO
                                var sp_c = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, cob.fecha, false, total_cob, null,
                                    "ADELANTO FACT " + fact.doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                                sp_c.Dispose();

                                // INSERTAR DOC COBRO ADELANTO
                                // int r = 1;
                                var sp_d1 = context.pInsertarRenglonesDocCobro(1, n_coll, "ADEL", n_adel, 0 /*MONTO*/, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(),
                                    null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                                sp_d1.Dispose();

								#region CODIGO ANTERIOR
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
								#endregion

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
                                string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>2.1.05.01.005</CuentaContable></Carpeta01></InformacionContable>";
                                // string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>7.1.01.01.001</CuentaContable></Carpeta01></InformacionContable>";

                                // SERIE AJPM
                                n_ajpm = GetNextConsec(sucur, "DOC_VEN_AJPM");

                                // INSERTAR AJPM
                                var sp_i = context.pInsertarDocumentoVenta("AJPM", n_ajpm, fact.co_cli, fact.co_ven, CO_MONE, null, null, fact.tasa, "RECARGO 3% IGTF FACT N° " + fact.doc_num,
                                    DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, "COBRO", n_coll, null, 0, 0, igtf, 0, null, null, 0, igtf, 0, 0, "7",
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
                                // fact.status = fact.saldo > 0 ? "1" : "2";
                                context.Entry(fact).State = EntityState.Modified;

                                // ACTUALIZAR DOCUMENTO
                                doc_v.saldo -= total_fact;
                                context.Entry(doc_v).State = EntityState.Modified;

                                // INSERTAR COBRO CRUCE
                                var sp_cc = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, cob.fecha, false, 0, null,
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
                                var sp_tt = context.pInsertarRenglonesTPCobro(1, n_coll, "EF", null, null, null, false, 0, null, null, null, null, COD_CAJA, DateTime.Now,
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
                                var sp_c_ret = context.pInsertarCobro(n_ret, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, cob.fecha, false, 0, null,
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
                                        "RET IVA FACT " + fact.doc_num, DateTime.Now, r_iva.fecha_che, r_iva.fecha_che, false, true, false, "COBRO", n_ret, null, 0, 0,
                                        r_iva.mont_doc, 0, null, null, 0, r_iva.mont_doc, 0, 0, "7", 0, 0, 0, 0, r_iva.num_doc, null, null, 0, 0, 0, 0, 0, 0, 0, null,
                                        false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                                    sp_d_ivan.Dispose();

                                    // INSERTAR DOC IVAN COBRO RET
                                    var sp_d_ret_2 = context.pInsertarRenglonesDocCobro(nr, n_ret, "IVAN", n_ivan, r_iva.mont_doc, 0, 0, 0, 0, null, null, f_guid, null,
                                        iva_guid, null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                                    sp_d_ret_2.Dispose();

                                    // INSERTAR RENG IVA COBRO RET
                                    var sp_r_iva = context.pInsertarRenglonesRetenIvaCobro(iva_guid, 1, Connection.GetConnByID(conn.ToString()).RIF.Replace("-", ""),
                                        decimal.Parse(periodImp), r_iva.fecha_che, "C", "FACT", rif_c, fact.doc_num, fact.n_control, fact.total_neto, fact.total_bruto,
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
                                var sp_t_ret = context.pInsertarRenglonesTPCobro(1, n_ret, "EF", null, null, null, false, 0, null, null, null, null, COD_CAJA, DateTime.Now, sucur,
                                    user, null, null, "SERVER PROFIT WEB");
                                sp_t_ret.Dispose();

                                // ACTUALIZAR FACTURA
                                fact.saldo -= mont_cob;
                                // fact.status = fact.saldo > 0 ? "1" : "2";
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
                        else
                        {
                            throw new Exception("La factura ya ha sido cobrada en su totalidad. Por favor, actualiza la pagina");
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO COBRO DE CONTADO", ex);

                        throw ex;
                    }
                }
            }

            return collect;
        }

        public void ConcilCollect(string id, string cob_num, string user)
        {
            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        // RENGLON TRANSFERENCIA ADELANTO
                        saCobroTPReng reng = context.saCobroTPReng.AsNoTracking().Single(r => r.cob_num == cob_num && r.forma_pag == "TP");

                        // MOVIMIENTO DE BANCO
                        saMovimientoBanco mov = context.saMovimientoBanco.AsNoTracking().Single(m => m.mov_num == reng.mov_num_b);

                        // ACTUALIZACION DE SALDO CONCILIADO
                        var sp = context.pActualizarSaldoBanco(mov.cod_cta, mov.cod_cta, "CF", "CF", reng.mont_doc, null);

                        mov.conciliado = true;
                        mov.fec_con = DateTime.Now;
                        context.Entry(mov).State = EntityState.Modified;

                        Transfer.ConcilTransf(id, user);

                        context.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR CONCILIANDO MOVIMIENTO DE COBRO " + cob_num, ex);

                        throw ex;
                    }
                }
            }
        }
        
        public int CancelCollect(string id, string cob_num, string user)
        {
            int result = 0;

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        // ADELANTO
                        saDocumentoVenta doc_a = new saDocumentoVenta();

                        // RENGLON DE ADELANTO DE COBRO ORIGINAL
                        saCobroDocReng reng_adel = context.saCobroDocReng.AsNoTracking().Single(r => r.cob_num == cob_num && r.co_tipo_doc == "ADEL");

                        // BUSQUEDA DE CRUCE
                        List<saCobroDocReng> rengs_c = context.saCobroDocReng.AsNoTracking()
                            .Where(r => r.co_tipo_doc == "ADEL" && r.nro_doc == reng_adel.nro_doc && r.mont_cob > 0).ToList();

                        // ANULACION DE CRUCE
                        if (rengs_c.Count == 1)
                        {
                            string cob_num_c = rengs_c[0].cob_num; // NRO. COBRO CRUCE
                            saCobro cob_orig = context.saCobro.AsNoTracking().Single(c => c.cob_num == cob_num_c);
                            
                            if (!cob_orig.anulado)
                            {
                                List<saCobroDocReng> rengs_doc_c = context.saCobroDocReng.AsNoTracking().Where(r => r.cob_num == cob_num_c).ToList(); // RENGS. COBRO CRUCE

                                if (rengs_doc_c.Count == 2) // ADEL-FACT
                                {
                                    // VALIDAR SI SOLO HAY DOCS ADEL Y FACT
                                    if (rengs_doc_c.Exists(r => r.co_tipo_doc.Trim() == "ADEL") && rengs_doc_c.Exists(r => r.co_tipo_doc.Trim() == "FACT"))
                                    {
                                        // FACTURA
                                        saCobroDocReng r_fact = rengs_doc_c.Single(r => r.co_tipo_doc.Trim() == "FACT");
                                        saDocumentoVenta doc_f = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "FACT" && d.nro_doc == r_fact.nro_doc);
                                        saFacturaVenta fact = context.saFacturaVenta.AsNoTracking().Single(f => f.doc_num == r_fact.nro_doc);

                                        // ADELANTO
                                        saCobroDocReng r_adel = rengs_doc_c.Single(r => r.co_tipo_doc.Trim() == "ADEL");
                                        doc_a = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "ADEL" && d.nro_doc == reng_adel.nro_doc);

                                        // MODIFICANDO SALDOS
                                        doc_f.saldo += r_fact.mont_cob;
                                        context.Entry(doc_f).State = EntityState.Modified;

                                        fact.saldo += r_fact.mont_cob;
                                        // fact.status = fact.saldo < fact.total_neto ? "1" : "0";
                                        context.Entry(fact).State = EntityState.Modified;

                                        doc_a.saldo += r_adel.mont_cob;
                                        // context.Entry(doc_a).State = EntityState.Modified;

                                        // MODIFICANDO COBRO
                                        saCobro cob = context.saCobro.AsNoTracking().Single(c => c.cob_num == cob_num_c);
                                        cob.anulado = true;
                                        context.Entry(cob).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        result = 2; // CRUCE CON OTROS TIPOS DE DOCUMENTOS
                                    }
                                }
                                else if (rengs_doc_c.Count == 3) // ADEL-FACT-AJPM
                                {
                                    // VALIDAR SI SOLO HAY DOCS ADEL FACT Y AJPM
                                    if (rengs_doc_c.Exists(r => r.co_tipo_doc.Trim() == "ADEL") && rengs_doc_c.Exists(r => r.co_tipo_doc.Trim() == "FACT") && 
                                        rengs_doc_c.Exists(r => r.co_tipo_doc.Trim() == "AJPM"))
                                    {
                                        // FACTURA
                                        saCobroDocReng r_fact = rengs_doc_c.Single(r => r.co_tipo_doc.Trim() == "FACT");
                                        saDocumentoVenta doc_f = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "FACT" && d.nro_doc == r_fact.nro_doc);
                                        saFacturaVenta fact = context.saFacturaVenta.AsNoTracking().Single(f => f.doc_num == r_fact.nro_doc);

                                        // ADELANTO
                                        saCobroDocReng r_adel = rengs_doc_c.Single(r => r.co_tipo_doc.Trim() == "ADEL");
                                        doc_a = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "ADEL" && d.nro_doc == r_adel.nro_doc);

                                        // IGTF
                                        saCobroDocReng r_igtf = rengs_doc_c.Single(r => r.co_tipo_doc.Trim() == "AJPM");
                                        saDocumentoVenta doc_i = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "AJPM" && d.nro_doc == r_igtf.nro_doc);

                                        // MODIFICANDO SALDOS
                                        doc_i.saldo = 0;
                                        doc_i.anulado = true;
                                        context.Entry(doc_i).State = EntityState.Modified;

                                        doc_f.saldo += r_fact.mont_cob;
                                        context.Entry(doc_f).State = EntityState.Modified;

                                        fact.saldo += r_fact.mont_cob;
                                        // fact.status = fact.saldo < fact.total_neto ? "1" : "0";
                                        context.Entry(fact).State = EntityState.Modified;

                                        doc_a.saldo += r_adel.mont_cob;
                                        // context.Entry(doc_a).State = EntityState.Modified;

                                        // MODIFICANDO COBRO
                                        saCobro cob = context.saCobro.AsNoTracking().Single(c => c.cob_num == cob_num_c);
                                        cob.anulado = true;
                                        context.Entry(cob).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        result = 2; // CRUCE CON OTROS TIPOS DE DOCUMENTOS
                                    }
                                }
                            } 
                            else
                            {
                                result = 4; // COBRO CRUCE YA ANULADO
                            }
                        }
                        else
                        {
                            result = 1; // ADELANTO ASOCIADO A MAS DE UN CRUCE (COBRO)
                        }

                        // ANULACION DE COBRO ORIGINAL
                        if (result == 0 || result == 4)
                        {
                            // COBRO ORIGINAL
                            saCobro cob = context.saCobro.AsNoTracking().Single(c => c.cob_num == cob_num);
                            
                            if (!cob.anulado)
                            {
                                result = 0;

                                saCobroDocReng cob_doc = context.saCobroDocReng.AsNoTracking().Single(c => c.cob_num == cob_num);
                                // saDocumentoVenta d_adel = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "ADEL" && d.nro_doc == cob_doc.nro_doc);
                                List<saCobroTPReng> cob_tp = context.saCobroTPReng.AsNoTracking().Where(c => c.cob_num == cob_num).ToList();

                                if (doc_a.co_tipo_doc == null)
                                    doc_a = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "ADEL" && d.nro_doc == cob_doc.nro_doc);

                                doc_a.saldo = 0;
                                doc_a.anulado = true;
                                context.Entry(doc_a).State = EntityState.Modified;

                                cob.anulado = true;
                                context.Entry(cob).State = EntityState.Modified;

                                if (cob_tp.Count == 1) // SOLO TRANSF.
                                {
                                    saCobroTPReng reng_tp = cob_tp.Single(t => t.forma_pag == "TP");

                                    // ACTUALIZANDO SALDO
                                    var sp_s = context.pSaldoActualizar(reng_tp.cod_cta, reng_tp.forma_pag, "TF", reng_tp.mont_doc, false, "COBRO", false);
                                    sp_s.Dispose();
                                }
                                else if (cob_tp.Count == 2) // TRANSF. Y EFECTIVO
                                {
                                    saCobroTPReng reng_tp_e = cob_tp.Single(t => t.forma_pag == "EF");
                                    saCobroTPReng reng_tp_t = cob_tp.Single(t => t.forma_pag == "TP");

                                    // ACTUALIZANDO SALDO
                                    var sp_s_e = context.pSaldoActualizar(reng_tp_e.cod_caja, "EF", "EF", Math.Round(reng_tp_e.mont_doc / cob.tasa, 2), false, "COBRO", false);
                                    sp_s_e.Dispose();

                                    var sp_s_t = context.pSaldoActualizar(reng_tp_t.cod_cta, reng_tp_t.forma_pag, "TF", reng_tp_t.mont_doc, false, "COBRO", false);
                                    sp_s_t.Dispose();

                                    // ANULANDO MOVIMIENTO
                                    Box.CancelMoveByCollect(cob_num, user);
                                }
                                else
                                {
                                    result = 3; // ADELANTO CON MAS DE UNA TRANSFERENCIA
                                }
                            }
                            else
                            {
                                result = 5; // COBRO ADELANTO YA ANULADO
                                Transfer.CancelTransf(id, user);
                            }
                        }

                        if (result == 0)
                        {
                            Transfer.CancelTransf(id, user);

                            context.SaveChanges();
                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = -1;
                        tran.Rollback();
                        Incident.CreateIncident("ERROR ANULANDO COBRO " + cob_num, ex);

                        throw ex;
                    }
                }
            }

            return result;
        }

        private decimal GetAmountRets(saFacturaVenta fact, bool calcRetIva, bool calcRetIslr)
        {
            decimal amount = 0;

            if (calcRetIva)
            {
                decimal porc = db.saCliente.AsNoTracking().First(c => c.co_cli == fact.co_cli).porc_esp;
                if (porc == 0)
                    porc = 75;

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