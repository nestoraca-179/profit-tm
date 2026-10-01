using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data.Entity.Core;

namespace ProfitTM.Models
{
    public class Order : ProfitAdmManager
    {
        public saPedidoVenta GetOrderByID(string id)
        {
            saPedidoVenta order;

            try
            {
                using (var transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    order = db.saPedidoVenta.AsNoTracking().Include("saCliente")
                        .Include("saCondicionPago").Include("saVendedor").Single(i => i.doc_num == id);

                    order.saPedidoVentaReng = db.saPedidoVentaReng.AsNoTracking().Where(reng => reng.doc_num == order.doc_num)
                        .OrderBy(reng => reng.reng_num).ToList();

                    order.saCliente.saPedidoVenta = null;
                    order.saVendedor.saPedidoVenta = null;
                    order.saCondicionPago.saPedidoVenta = null;
                    foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
                    {
                        reng.saPedidoVenta = null;
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                order = null;
                Incident.CreateIncident($"ERROR CONSULTANDO PEDIDO Y RENGLONES. DOC_NUM: {id}", ex);
            }

            return order;
        }

        public List<saPedidoVenta> GetAllOrders(int number, bool free)
        {
            List<saPedidoVenta> orders = new List<saPedidoVenta>();

            try
            {
                if (free)
                {
                    orders = db.saPedidoVenta.AsNoTracking().Include("saCliente").Where(o => o.status.Trim() == "0")
                        .OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                }
                else
                {
                    orders = db.saPedidoVenta.AsNoTracking().Include("saPedidoVentaReng").Include("saCliente").Include("saVendedor")
                        .Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                }

                foreach (saPedidoVenta order in orders)
                {
                    if (!free)
                    {
                        order.saVendedor.saPedidoVenta = null;
                        order.saCondicionPago.saPedidoVenta = null;
                    }

                    order.saCliente.saPedidoVenta = null;
                    foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
                    {
                        reng.saPedidoVenta = null;
                    }
                }
            }
            catch (Exception ex)
            {
                orders = null;
                Incident.CreateIncident("ERROR BUSCANDO PEDIDOS", ex);
            }

            return orders;
        }

        public static saPedidoVenta Add(saPedidoVenta order, string n_entity)
        {
            try
            {
                string request = JsonConvert.SerializeObject(new { saPedidoVenta = order, n_entity },
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                LogsFact.CreateLogInFile($"ORDER_ADD_REQUEST {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {request}");
            }
            catch (Exception ex)
            {
                LogsFact.CreateLogInFile($"ORDER_ADD_LOG_ERROR {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.Message}");
            }

            (bool valid, string paramName, string message) validation = isValid(order, n_entity);
            if (!validation.valid)
                throw new ArgumentException(validation.message, validation.paramName);

            using (ProfitAdmEntities context = new ProfitAdmEntities(n_entity))
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string docNum = order.doc_num.Trim();

                        if (context.saPedidoVenta.AsNoTracking().Any(item => item.doc_num.Trim() == docNum))
                            throw new InvalidOperationException("El numero de pedido ya existe: " + docNum);

                        order.status = "0";
                        context.pInsertarPedidoVenta(docNum, order.descrip, order.co_cli, order.co_tran, order.co_mone,
                            order.co_cta_ingr_egr, order.co_ven, order.co_cond, order.fec_emis, order.fec_venc, order.fec_reg,
                            order.anulado, order.status, order.tasa, order.n_control, null, order.porc_desc_glob,
                            order.monto_desc_glob, order.porc_reca, order.monto_reca, order.saldo, order.total_bruto,
                            order.monto_imp, order.monto_imp2, order.monto_imp3, order.otros1, order.otros2, order.otros3,
                            order.total_neto, order.dis_cen, order.comentario, order.dir_ent, order.contrib, order.impresa,
                            order.salestax, order.impfis, order.impfisfac, order.ven_ter, order.campo1, order.campo2,
                            order.campo3, order.campo4, order.campo5, order.campo6, order.campo7, order.campo8,
                            order.co_us_in, order.co_sucu_in, order.revisado, order.trasnfe, "SERVER PROFIT WEB").FirstOrDefault();

                        foreach (saPedidoVentaReng reng in order.saPedidoVentaReng.OrderBy(item => item.reng_num))
                        {
                            reng.pendiente = reng.total_art;
                            context.pInsertarRenglonesPedidoVenta(reng.reng_num, docNum, reng.co_art, reng.des_art,
                                reng.co_uni, reng.sco_uni, reng.co_alma, reng.co_precio, reng.tipo_imp, reng.tipo_imp2,
                                reng.tipo_imp3, reng.total_art, reng.stotal_art, reng.prec_vta, reng.porc_desc,
                                reng.monto_desc, reng.reng_neto, reng.pendiente, reng.pendiente2, reng.monto_desc_glob,
                                reng.monto_reca_glob, reng.otros1_glob, reng.otros2_glob, reng.otros3_glob,
                                reng.monto_imp_afec_glob, reng.monto_imp2_afec_glob, reng.monto_imp3_afec_glob,
                                reng.tipo_doc, reng.rowguid_doc, reng.num_doc, reng.porc_imp, reng.porc_imp2, reng.porc_imp3,
                                reng.monto_imp, reng.monto_imp2, reng.monto_imp3, reng.otros, reng.total_dev, reng.monto_dev,
                                reng.comentario, reng.dis_cen, order.co_sucu_in, order.co_us_in, reng.revisado, reng.trasnfe,
                                "SERVER PROFIT WEB").FirstOrDefault();
                        }

                        context.SaveChanges();
                        transaction.Commit();

                        saPedidoVenta created = context.saPedidoVenta.AsNoTracking()
                            .Include("saPedidoVentaReng").Include("saCliente").Include("saCondicionPago").Include("saVendedor")
                            .Single(item => item.doc_num.Trim() == docNum);

                        created.saCliente.saPedidoVenta = null;
                        created.saVendedor.saPedidoVenta = null;
                        created.saCondicionPago.saPedidoVenta = null;
                        foreach (saPedidoVentaReng reng in created.saPedidoVentaReng)
                        {
                            reng.saPedidoVenta = null;
                        }

                        return created;
                    }
                    catch (DbEntityValidationException ex)
                    {
                        try { transaction.Rollback(); } catch { }
                        Incident.CreateIncident("ERROR DE VALIDACION AGREGANDO PEDIDO " + order.doc_num, ex);
                        throw;
                    }
                    catch (DbUpdateException ex)
                    {
                        try { transaction.Rollback(); } catch { }
                        Incident.CreateIncident("ERROR DE ACTUALIZACION AGREGANDO PEDIDO " + order.doc_num, ex);
                        throw;
                    }
                    catch (SqlException ex)
                    {
                        try { transaction.Rollback(); } catch { }
                        Incident.CreateIncident("ERROR SQL AGREGANDO PEDIDO " + order.doc_num, ex);
                        throw;
                    }
                    catch (EntityException ex)
                    {
                        try { transaction.Rollback(); } catch { }
                        Incident.CreateIncident("ERROR DE CONEXION AGREGANDO PEDIDO " + order.doc_num, ex);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        try { transaction.Rollback(); } catch { }
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO PEDIDO " + order.doc_num, ex);
                        throw;
                    }
                }
            }
        }

        public saPedidoVenta Delete(string id)
        {
            saPedidoVenta order = GetOrderByID(id);
            db.pEliminarPedidoVenta(order.doc_num, order.validador, "SERVER PROFIT WEB", "PROFIT WEB", null, order.rowguid);

            return order;
        }

        private static (bool valid, string paramName, string message) isValid(saPedidoVenta order, string n_entity)
        {
            if (order == null)
                return (false, "order", "El pedido es obligatorio.");

            if (string.IsNullOrWhiteSpace(n_entity))
                return (false, "n_entity", "La cadena de conexion es obligatoria.");

            if (string.IsNullOrWhiteSpace(order.doc_num))
                return (false, "order", "El numero de pedido es obligatorio.");

            if (order.saPedidoVentaReng == null || !order.saPedidoVentaReng.Any())
                return (false, "order", "El pedido debe tener al menos un renglon.");

            if (order.saPedidoVentaReng.Any(reng => reng == null))
                return (false, "order", "El pedido contiene un renglon invalido.");

            if (order.saPedidoVentaReng.GroupBy(reng => reng.reng_num).Any(group => group.Count() > 1))
                return (false, "order", "El pedido contiene numeros de renglon repetidos.");

            if (order.saPedidoVentaReng.Any(reng => reng.reng_num <= 0))
                return (false, "order", "Todos los renglones deben tener un numero mayor que cero.");

            if (order.saPedidoVentaReng.Any(reng => string.IsNullOrWhiteSpace(reng.co_art)))
                return (false, "order", "Todos los renglones deben tener articulo.");

            return (true, string.Empty, string.Empty);
        }
    }
}