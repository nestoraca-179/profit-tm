using ProfitTM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace ProfitTM.Models
{
    public class Root
    {
        // private static readonly string base_url = "https://emisionv2.thefactoryhka.com.ve/api/"; // PRODUCCION
        private static readonly string base_url = "https://demoemisionv2.thefactoryhka.com.ve/api/"; // INTEGRACION

        public DocumentoElectronico documentoElectronico { get; set; }

        public string GetJsonInvoiceInfo(saFacturaVenta i, string serie)
        {
            Root root = new Root();
            saCliente c = i.saCliente;
            bool isFrg = i.co_cli.StartsWith("FR") || i.co_cli.StartsWith("0");

            string result = "";

            try
            {
                root.documentoElectronico = new DocumentoElectronico()
                {
                    encabezado = new Encabezado()
                    {
                        identificacionDocumento = new IdentificacionDocumento()
                        {
                            tipoDocumento = "01",
                            numeroDocumento = i.doc_num.Trim(),
                            serieFacturaAfectada = null,
                            numeroFacturaAfectada = null,
                            fechaFacturaAfectada = null,
                            montoFacturaAfectada = null,
                            comentarioFacturaAfectada = null,
                            fechaEmision = i.fec_emis.ToString("dd/MM/yyyy"),
                            fechaVencimiento = i.fec_venc.ToString("dd/MM/yyyy"),
                            horaEmision = i.fec_emis.ToString("hh:mm:ss") + (i.fec_emis.Hour < 12 ? " am" : " pm"),
                            anulado = i.anulado,
                            tipoDePago = "CONTADO",
                            serie = serie,
                            sucursal = i.co_sucu_in,
                            tipoDeVenta = "Inmediato",
                            moneda = "BSD",
                        },
                        comprador = new Comprador()
                        {
                            tipoIdentificacion = isFrg ? "E" : c.rif.Substring(0, 1),
                            numeroIdentificacion = isFrg ? c.rif : c.rif.Substring(1).Trim(),
                            razonSocial = c.cli_des.Trim(),
                            direccion = c.direc1.Trim(),
                            ubigeo = null,
                            pais = "VE",
                            notificar = "No",
                            telefono = new List<string>() { c.telefonos },
                            correo = GetEmails(c),
                        },
                        totales = new Totales()
                        {
                            nroItems = i.saFacturaVentaReng.Count.ToString(),
                            montoGravadoTotal = i.total_bruto.ToString().Replace(",", "."),
                            montoExentoTotal = i.saFacturaVentaReng.Where(r => r.tipo_imp == "7").Select(r => r.reng_neto).Sum().ToString().Replace(",", "."), // "0.00",
                            subtotal = i.total_bruto.ToString().Replace(",", "."),
                            totalAPagar = Math.Round(
                                i.total_neto + // TOTAL + IVA FACTURA (BSD)
                                (((decimal.Parse(i.comentario) * 3) / 100) * i.tasa) // IGTF (BSD)
                            , 2).ToString().Replace(",", "."),
                            totalIVA = i.monto_imp.ToString().Replace(",", "."),
                            montoTotalConIVA = i.total_neto.ToString().Replace(",", "."),
                            montoEnLetras = new UtilsController().NumberToWords(i.total_neto),
                            listaDescBonificacion = new List<ListaDescBonificacion>()
                            {
                                new ListaDescBonificacion()
                                {
                                    descDescuento = "descuento",
                                    montoDescuento = "0.00"
                                },
                                new ListaDescBonificacion()
                                {
                                    descDescuento = "recargo",
                                    montoDescuento = "0.00"
                                }
                            },
                            impuestosSubtotal = new List<ImpuestosSubtotal>()
                            {
                                new ImpuestosSubtotal()
                                {
                                    codigoTotalImp = "E",
                                    alicuotaImp = "0.00",
                                    baseImponibleImp = i.saFacturaVentaReng.Where(r => r.tipo_imp == "7").Select(r => r.reng_neto).Sum().ToString().Replace(",", "."), // "0.00",
                                    valorTotalImp = i.saFacturaVentaReng.Where(r => r.tipo_imp == "7").Select(r => r.reng_neto).Sum().ToString().Replace(",", "."), // "0.00",
                                },
                                new ImpuestosSubtotal()
                                {
                                    codigoTotalImp = "G",
                                    alicuotaImp = "16.00",
                                    baseImponibleImp = i.total_bruto.ToString().Replace(",", "."), // ESTA BASE IMPONIBLE EN BSD ESTA APARECIENDO EN LA COLUMNA DE USD (PEDIR INVERTIR)
                                    valorTotalImp = i.monto_imp.ToString().Replace(",", "."),
                                },
                                new ImpuestosSubtotal()
                                {
                                    codigoTotalImp = "IGTF",
                                    alicuotaImp = "3.00",
                                    baseImponibleImp = Math.Round(decimal.Parse(i.comentario) * i.tasa, 2).ToString().Replace(",", "."),
                                    valorTotalImp = Math.Round(((decimal.Parse(i.comentario) * i.tasa) * 3) / 100, 2).ToString().Replace(",", "."),
                                }
                            },
                            formasPago = new List<FormasPago>()
                            {
                                //new FormasPago()
                                //{
                                //    descripcion = "Transferencia Bancaria|-|-",
                                //    fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                //    forma = "01",
                                //    monto = Math.Round(i.total_neto, 2).ToString().Replace(",", "."),
                                //    moneda = "BSD"
                                //},
                                new FormasPago()
                                {
                                    descripcion = "Efectivo Divisas|-|-",
                                    fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                    forma = "01",
                                    monto = Math.Round(decimal.Parse(i.comentario), 2).ToString().Replace(",", "."),
                                    moneda = "USD"
                                }
                            }
                        },
                        totalesRetencion = new TotalesRetencion()
                        {
                            totalBaseImponible = i.total_bruto.ToString().Replace(",", "."),
                            numeroCompRetencion = "1",
                            fechaEmisionCR = DateTime.Now.ToString("dd/MM/yyyy"),
                            totalIVA = Math.Round(i.monto_imp * ((c.contribu_e ? c.porc_esp : 75) / 100), 2).ToString().Replace(",", "."),
                            totalISRL = Math.Round((i.total_bruto * 2) / 100, 2).ToString().Replace(",", "."),
                            totalRetenido = (
                                Math.Round(i.monto_imp * ((c.contribu_e ? c.porc_esp : 75) / 100), 2) +
                                Math.Round((i.total_bruto * 2) / 100, 2)
                            ).ToString().Replace(",", ".")
                        },
                        totalesOtraMoneda = new TotalesOtraMoneda()
                        {
                            moneda = "USD",
                            tipoCambio = Math.Round(i.tasa, 2).ToString().Replace(",", "."),
                            montoGravadoTotal = Math.Round(i.total_bruto / i.tasa, 2).ToString().Replace(",", "."),
                            montoExentoTotal = Math.Round(i.saFacturaVentaReng.Where(r => r.tipo_imp == "7").Select(r => r.reng_neto).Sum() / i.tasa, 2).ToString().Replace(",", "."), // "0.00",
                            subtotal = Math.Round(i.total_bruto / i.tasa, 2).ToString().Replace(",", "."),
                            totalAPagar = Math.Round(
                                (i.total_neto / i.tasa) + // TOTAL + IVA FACTURA (USD)
                                ((decimal.Parse(i.comentario) * 3) / 100) // IGTF (USD)
                            , 2).ToString().Replace(",", "."),
                            totalIVA = Math.Round(i.monto_imp / i.tasa, 2).ToString().Replace(",", "."),
                            montoTotalConIVA = Math.Round(i.total_neto / i.tasa, 2).ToString().Replace(",", "."),
                            montoEnLetras = new UtilsController().NumberToWords(Math.Round(i.total_neto / i.tasa, 2)),
                            listaDescBonificacion = new List<ListaDescBonificacion>()
                            {
                                new ListaDescBonificacion()
                                {
                                    descDescuento = "descuento",
                                    montoDescuento = "0.00"
                                },
                                new ListaDescBonificacion()
                                {
                                    descDescuento = "recargo",
                                    montoDescuento = "0.00"
                                }
                            },
                            impuestosSubtotal = new List<ImpuestosSubtotal>()
                            {
                                new ImpuestosSubtotal()
                                {
                                    codigoTotalImp = "E",
                                    alicuotaImp = "0.00",
                                    baseImponibleImp = Math.Round(i.saFacturaVentaReng.Where(r => r.tipo_imp == "7").Select(r => r.reng_neto).Sum() / i.tasa, 2).ToString(), // "0.00",
                                    valorTotalImp = Math.Round(i.saFacturaVentaReng.Where(r => r.tipo_imp == "7").Select(r => r.reng_neto).Sum() / i.tasa, 2).ToString(), // "0.00",
                                },
                                new ImpuestosSubtotal()
                                {
                                    codigoTotalImp = "G",
                                    alicuotaImp = "16.00",
                                    baseImponibleImp = Math.Round(i.total_bruto / i.tasa, 2).ToString().Replace(",", "."), // ESTA BASE IMPONIBLE EN USD ESTA APARECIENDO EN LA COLUMNA DE BSD (PEDIR INVERTIR)
                                    valorTotalImp = Math.Round(i.monto_imp / i.tasa, 2).ToString().Replace(",", "."),
                                },
                                new ImpuestosSubtotal()
                                {
                                    codigoTotalImp = "IGTF",
                                    alicuotaImp = "3.00",
                                    baseImponibleImp = decimal.Parse(i.comentario).ToString().Replace(",", "."),
                                    valorTotalImp = Math.Round((decimal.Parse(i.comentario) * 3) / 100, 2).ToString().Replace(",", "."),
                                }
                            },
                        }
                    },
                    detallesItems = i.saFacturaVentaReng.Select(r => new DetallesItem()
                    {
                        numeroLinea = r.reng_num.ToString(),
                        codigoPLU = r.co_art.Trim(),
                        indicadorBienoServicio = "2",
                        descripcion = new Product().GetArtByID(r.co_art).art_des.Trim(),
                        cantidad = Convert.ToInt32(r.total_art).ToString(),
                        unidadMedida = r.co_uni.Trim(),
                        precioUnitario = Math.Round(r.prec_vta, 2).ToString().Replace(",", "."),
                        precioItem = Math.Round(r.reng_neto, 2).ToString().Replace(",", "."),
                        codigoImpuesto = r.tipo_imp == "1" ? "G" : "E",
                        tasaIVA = r.tipo_imp == "1" ? "16.00" : "0.00",
                        valorIVA = r.tipo_imp == "1" ? Math.Round(r.monto_imp, 2).ToString().Replace(",", ".") : "0.00",
                        valorTotalItem = Math.Round(r.reng_neto, 2).ToString().Replace(",", "."),
                        infoAdicionalItem = new List<InfoAdicionalItem>()
                        {
                            new InfoAdicionalItem()
                            {
                                campo = "USD",
                                valor = Math.Round(r.reng_neto / i.tasa, 2).ToString().Replace(",", ".")
                            }
                        }
                    }).ToList(),
                    viajes = new Viajes()
                    {
                        razonSocialServTransporte = i.campo1,
                        numeroBoleto = i.campo8,
                        puntoSalida = i.campo2,
                        puntoDestino = i.descrip,
                    },
                    infoAdicional = new List<InfoAdicional>()
                    {
                        new InfoAdicional()
                        {
                            campo = "PDF",
                            valor = "{'coletilla1':'De conformidad con la Providencia Administrativa SNAT/2022/000013 publicada en la G.O.N 42.339 del 17-03-2022, este pago está sujeto al cobro adicional del 3% del Impuesto a las Grandes Transacciones Financieras (IGTF), siempre que sea pagado en moneda distinta a la del curso legal.'}"
                        },
                        new InfoAdicional()
                        {
                            campo = "PDF",
                            valor = "{'coletilla2':'En los casos en que la base imponible de la venta o prestación de servicio estuviere expresada en moneda extranjera, se establecerá la equivalencia en moneda nacional, al tipo de cambio corriente en el mercado del día en que ocurra el hecho imponible, salvo que éste ocurra en un día no hábil para el sector financiero, en cuyo caso se aplicará el vigente en el día hábil inmediatamente siguiente al de la operación. (ART. 25 Ley de IVA G.O N° 6.152 de fecha 18/11/2014)'}"
                        }
                    },
                    transporte = new TransporteF()
                    {
                        descripcion = i.campo7,
                        codigo = i.campo3
                    }
                };

                result = JsonConvert.SerializeObject(root);
            }
            catch (Exception ex)
            {
                Incident.CreateIncident(string.Format("ERROR CREANDO JSON FACT {0}", i.doc_num), ex);
            }
            
            return result;
        }

        public async Task<ModelAuthResponse> SendAuth(ModelAuthRequest auth)
        {
            ModelAuthResponse final = new ModelAuthResponse();
            string url = base_url + "Autenticacion";
            string data = JsonConvert.SerializeObject(auth);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelAuthResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != 200)
                        {
                            throw new AuthenticationException($"{final.mensaje} ** {final.codigo}");
                        }
                    }
                    else
                    {
                        throw new AuthenticationException($"{response.StatusCode} ** {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }
        
        public async Task<ModelInvoiceInfoResponse> SendInvoiceInfoAsync(LogsFactOnline log, string token)
        {
            ModelInvoiceInfoResponse final = new ModelInvoiceInfoResponse();
            string url = base_url + "Emision";
            string data = log.BodyJson;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelInvoiceInfoResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != "200" && final.codigo != "203" && final.codigo != "400")
                            throw new InformationException($"{final.mensaje} ** {final.codigo}");
                        else if ((final.codigo == "203" || final.codigo == "400") && final.validaciones != null)
                            throw new InformationException($"{final.mensaje} ** {final.codigo} ** {final.validaciones[0]}");

                        Invoice.UpdateControl(log, final.resultado.numeroControl);
                    }
                    else
                    {
                        int code = (int)response.StatusCode;
                        if (code != 203 && code != 400)
                            throw new InformationException($"{response.StatusCode} ** {code}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }

        public async Task<ModelAssignResponse> SendAssign(ModelAssignRequest assign, string token)
        {
            ModelAssignResponse final = new ModelAssignResponse();
            string url = base_url + "AsignarNumeraciones";
            string data = JsonConvert.SerializeObject(assign);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelAssignResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != "200")
                        {
                            throw new AssignmentException($"{final.mensaje} ** {final.codigo}");
                        }
                    }
                    else
                    {
                        throw new AssignmentException($"{response.StatusCode} ** {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }

        public async Task<ModelSendResponse> SendEmail(ModelSendRequest send, string token)
        {
            ModelSendResponse final = new ModelSendResponse();
            string url = base_url + "Correo/Enviar";
            string data = JsonConvert.SerializeObject(send);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelSendResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != "200")
                        {
                            throw new SendingException($"{final.mensaje} ** {final.codigo}");
                        }
                    }
                    else
                    {
                        throw new SendingException($"{response.StatusCode} ** {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }

        public async Task<ModelDownloadResponse> DownloadInvoice(ModelDownloadRequest download, string token)
        {
            ModelDownloadResponse final = new ModelDownloadResponse();
            string url = base_url + "DescargaArchivo";
            string data = JsonConvert.SerializeObject(download);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelDownloadResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != "200")
                        {
                            throw new DownloadException($"{final.mensaje} ** {final.codigo}");
                        }
                    }
                    else
                    {
                        throw new DownloadException($"{response.StatusCode} ** {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }

        public async Task<ModelCancelResponse> CancelInvoice(ModelCancelRequest cancel, string token)
        {
            ModelCancelResponse final = new ModelCancelResponse();
            string url = base_url + "Anular";
            string data = JsonConvert.SerializeObject(cancel);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelCancelResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != "200" && final.codigo != "203")
                        {
                            throw new CancelException($"{final.mensaje} ** {final.codigo}");
                        }

                        if (final.codigo == "203") 
                        {
                            if (final.validaciones != null && !final.validaciones.Contains("Documento ha sido anulada previamente"))
                                throw new CancelException($"{final.validaciones[0]} ** {final.codigo}");
                        }
                    }
                    else
                    {
                        throw new CancelException($"{response.StatusCode} ** {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }

        private List<string> GetEmails(saCliente c)
        {
            List<string> emails = new List<string>();

            if (string.IsNullOrEmpty(c.email) && string.IsNullOrEmpty(c.email_alterno) && string.IsNullOrEmpty(c.campo1) && string.IsNullOrEmpty(c.campo2) && string.IsNullOrEmpty(c.campo3))
                return null;

            if (!string.IsNullOrEmpty(c.email))
                emails.Add(c.email);

            if (!string.IsNullOrEmpty(c.email_alterno))
                emails.Add(c.email_alterno);

            if (!string.IsNullOrEmpty(c.campo1))
                emails.Add(c.campo1);

            if (!string.IsNullOrEmpty(c.campo2))
                emails.Add(c.campo2);

            if (!string.IsNullOrEmpty(c.campo3))
                emails.Add(c.campo3);

            return emails;
        }
    }

    // MODELO AUTENTICACION
    public class ModelAuthRequest
    {
        public string usuario { get; set; }
        public string clave { get; set; }
    }

    public class ModelAuthResponse
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public string token { get; set; }
        public DateTime expiracion { get; set; }
    }

    // MODELO RESPUESTA ENVIO DOCUMENTO
    public class ModelInvoiceInfoResponse
    {
        public Resultado resultado { get; set; }
        public List<string> validaciones { get; set; }
        public string codigo { get; set; }
        public string mensaje { get; set; }
    }

    public class Resultado
    {
        public string imprentaDigital { get; set; }
        public string autorizado { get; set; }
        public string serie { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string numeroControl { get; set; }
        public string fechaAsignacion { get; set; }
        public string horaAsignacion { get; set; }
        public string fechaAsignacionNumeroControl { get; set; }
        public string horaAsignacionNumeroControl { get; set; }
        public string rangoAsignado { get; set; }
        public string transaccionId { get; set; }
    }

    // MODELO ASIGNACION DE NUMERACION
    public class ModelAssignRequest
    {
        public List<DetalleAsignacion> detalleAsignacion { get; set; }
    }

    public class DetalleAsignacion
    {
        public string serie { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumentoInicio { get; set; }
        public string numeroDocumentoFin { get; set; }
    }

    public class ModelAssignResponse
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public List<string> validaciones { get; set; }
        public string fechaAsignacion { get; set; }
        public List<RangosAsignado> rangosAsignados { get; set; }
    }

    public class RangosAsignado
    {
        public string asignado { get; set; }
        public string global { get; set; }
    }

    // MODELO ENVIO DE CORREO
    public class ModelSendRequest
    {
        public string serie { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public List<string> correos { get; set; }
    }

    public class ModelSendResponse
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public List<string> validaciones { get; set; }
    }

    // MODELO DESCARGA DE ARCHIVO
    public class ModelDownloadRequest
    {
        public string serie { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string tipoArchivo { get; set; }
    }

    public class ModelDownloadResponse
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public string archivo { get; set; }
    }

    // MODELO ANULACION DE FACTURA
    public class ModelCancelRequest
    {
        public string serie { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string motivoAnulacion { get; set; }
        public string fechaAnulacion { get; set; }
        public string horaAnulacion { get; set; }
    }

    public class ModelCancelResponse
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public List<string> validaciones { get; set; }
    }

    // MODELOS JSON FACTURA DE VENTA
    public class DocumentoElectronico
    {
        public Encabezado encabezado { get; set; }
        public List<DetallesItem> detallesItems { get; set; }
        public Viajes viajes { get; set; }
        public List<InfoAdicional> infoAdicional { get; set; }
        public TransporteF transporte { get; set; }
    }

    public class Encabezado
    {
        public IdentificacionDocumento identificacionDocumento { get; set; }
        public Comprador comprador { get; set; }
        public Totales totales { get; set; }
        public TotalesRetencion totalesRetencion { get; set; }
        public TotalesOtraMoneda totalesOtraMoneda { get; set; }
    }

    public class IdentificacionDocumento
    {
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public object serieFacturaAfectada { get; set; }
        public object numeroFacturaAfectada { get; set; }
        public object fechaFacturaAfectada { get; set; }
        public object montoFacturaAfectada { get; set; }
        public object comentarioFacturaAfectada { get; set; }
        public string fechaEmision { get; set; }
        public string fechaVencimiento { get; set; }
        public string horaEmision { get; set; }
        public bool anulado { get; set; }
        public string tipoDePago { get; set; }
        public string serie { get; set; }
        public string sucursal { get; set; }
        public string tipoDeVenta { get; set; }
        public string moneda { get; set; }
    }

    public class Comprador
    {
        public string tipoIdentificacion { get; set; }
        public string numeroIdentificacion { get; set; }
        public string razonSocial { get; set; }
        public string direccion { get; set; }
        public object ubigeo { get; set; }
        public string pais { get; set; }
        public string notificar { get; set; }
        public List<string> telefono { get; set; }
        public List<string> correo { get; set; }
    }

    public class Totales
    {
        public string nroItems { get; set; }
        public string montoGravadoTotal { get; set; }
        public string montoExentoTotal { get; set; }
        public string subtotal { get; set; }
        public string totalAPagar { get; set; }
        public string totalIVA { get; set; }
        public string montoTotalConIVA { get; set; }
        public string montoEnLetras { get; set; }
        public List<ListaDescBonificacion> listaDescBonificacion { get; set; }
        public List<ImpuestosSubtotal> impuestosSubtotal { get; set; }
        public List<FormasPago> formasPago { get; set; }
    }

    public class ListaDescBonificacion
    {
        public string descDescuento { get; set; }
        public string montoDescuento { get; set; }
    }

    public class ImpuestosSubtotal
    {
        public string codigoTotalImp { get; set; }
        public string alicuotaImp { get; set; }
        public string baseImponibleImp { get; set; }
        public string valorTotalImp { get; set; }
    }

    public class FormasPago
    {
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public string forma { get; set; }
        public string monto { get; set; }
        public string moneda { get; set; }
    }

    public class TotalesRetencion
    {
        public string totalBaseImponible { get; set; }
        public string numeroCompRetencion { get; set; }
        public string fechaEmisionCR { get; set; }
        public string totalIVA { get; set; }
        public string totalRetenido { get; set; }
        public string totalISRL { get; set; }
    }

    public class TotalesOtraMoneda
    {
        public string moneda { get; set; }
        public string tipoCambio { get; set; }
        public string montoGravadoTotal { get; set; }
        public string montoExentoTotal { get; set; }
        public string subtotal { get; set; }
        public string totalAPagar { get; set; }
        public string totalIVA { get; set; }
        public string montoTotalConIVA { get; set; }
        public string montoEnLetras { get; set; }
        public List<ListaDescBonificacion> listaDescBonificacion { get; set; }
        public List<ImpuestosSubtotal> impuestosSubtotal { get; set; }
    }

    public class DetallesItem
    {
        public string numeroLinea { get; set; }
        public string codigoPLU { get; set; }
        public string indicadorBienoServicio { get; set; }
        public string descripcion { get; set; }
        public string cantidad { get; set; }
        public string unidadMedida { get; set; }
        public string precioUnitario { get; set; }
        public string precioItem { get; set; }
        public string codigoImpuesto { get; set; }
        public string tasaIVA { get; set; }
        public string valorIVA { get; set; }
        public string valorTotalItem { get; set; }
        public List<InfoAdicionalItem> infoAdicionalItem { get; set; }
    }

    public class InfoAdicionalItem
    {
        public string campo { get; set; }
        public string valor { get; set; }
    }

    public class Viajes
    {
        public string razonSocialServTransporte { get; set; }
        public string numeroBoleto { get; set; }
        public string puntoSalida { get; set; }
        public string puntoDestino { get; set; }
    }

    public class InfoAdicional
    {
        public string campo { get; set; }
        public string valor { get; set; }
    }
    
    public class TransporteF
    {
        public string descripcion { get; set; }
        public string codigo { get; set; }
    }

    // EXCEPCIONES
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string msg) : base(msg) { }
    }

    public class InformationException : Exception
    {
        public InformationException(string msg) : base(msg) { }
    }

    public class AssignmentException : Exception
    {
        public AssignmentException(string msg) : base(msg) { }
    }

    public class SendingException : Exception
    {
        public SendingException(string msg) : base(msg) { }
    }

    public class DownloadException : Exception
    {
        public DownloadException(string msg) : base(msg) { }
    }

    public class CancelException : Exception
    {
        public CancelException(string msg) : base(msg) { }
    }
}