﻿using ProfitTM.Controllers;
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
        public DocumentoElectronico documentoElectronico { get; set; }

        public string GetJsonInvoiceInfo(saFacturaVenta i)
        {
            Root root = new Root();
            saCliente c = new Client().GetClientByID(i.co_cli);

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
                        fechaEmision = i.fec_emis.ToString("dd/MM/yyyy"),
                        fechaVencimiento = i.fec_venc.ToString("dd/MM/yyyy"),
                        horaEmision = i.fec_emis.ToString("hh:mm:ss tt").Replace("p. m.", "pm").Replace("a. m.", "am"),
                        anulado = i.anulado,
                        tipoDePago = "CONTADO",
                        serie = "A",
                        sucursal = i.co_sucu_in,
                        tipoDeVenta = "Inmediato",
                        moneda = "BSD",
                    },
                    comprador = new Comprador()
                    {
                        tipoIdentificacion = i.co_cli.Substring(0, 1),
                        numeroIdentificacion = i.co_cli.Substring(1).Trim(),
                        razonSocial = c.cli_des.Trim(),
                        direccion = c.direc1.Trim(),
                        ubigeo = null,
                        pais = "VE",
                        notificar = "Si",
                        telefono = new List<string>() { c.telefonos },
                        correo = new List<string>() { "nestoraca.179@gmail.com" },
                    },
                    totales = new Totales()
                    {
                        nroItems = i.saFacturaVentaReng.Count.ToString(),
                        montoGravadoTotal = i.total_bruto.ToString().Replace(",", "."),
                        montoExentoTotal = "0.00",
                        subtotal = i.total_bruto.ToString().Replace(",", "."),
                        totalAPagar = i.total_neto.ToString().Replace(",", "."),
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
                                baseImponibleImp = "0.00",
                                valorTotalImp = "0.00",
                            },
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "G",
                                alicuotaImp = "16.00",
                                baseImponibleImp = i.total_bruto.ToString().Replace(",", "."),
                                valorTotalImp = i.monto_imp.ToString().Replace(",", "."),
                            },
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "IGTF",
                                alicuotaImp = "3.00",
                                baseImponibleImp = Math.Round(decimal.Parse(i.comentario) * i.tasa, 2).ToString().Replace(",", "."),
                                valorTotalImp = Math.Round((decimal.Parse(i.comentario) * i.tasa) * (3 / 100), 2).ToString().Replace(",", "."),
                            }
                        },
                        formasPago = new List<FormasPago>()
                        {
                            new FormasPago()
                            {
                                descripcion = "Transferencia Bancaria|Venezuela|04008933",
                                fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                forma = "01",
                                monto = Math.Round(i.total_neto, 2).ToString().Replace(",", "."),
                                moneda = "BSD"
                            },
                            new FormasPago()
                            {
                                descripcion = "Efectivo Divisas|-|-",
                                fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                forma = "01",
                                monto = Math.Round(i.total_neto / i.tasa, 2).ToString().Replace(",", "."),
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
                        totalISRL = (i.total_bruto * (2 / 100)).ToString().Replace(",", "."),
                        totalRetenido = "0.00"
                    },
                    totalesOtraMoneda = new TotalesOtraMoneda()
                    {
                        moneda = "USD",
                        tipoCambio = Math.Round(i.tasa, 2).ToString().Replace(",", "."),
                        montoGravadoTotal = Math.Round(i.total_bruto / i.tasa).ToString().Replace(",", "."),
                        montoExentoTotal = "0.00",
                        subtotal = Math.Round(i.total_bruto / i.tasa).ToString().Replace(",", "."),
                        totalAPagar = Math.Round(i.total_neto / i.tasa).ToString().Replace(",", "."),
                        totalIVA = Math.Round(i.monto_imp / i.tasa).ToString().Replace(",", "."),
                        montoTotalConIVA = Math.Round(i.total_neto / i.tasa).ToString().Replace(",", "."),
                        montoEnLetras = new UtilsController().NumberToWords(Math.Round(i.total_neto / i.tasa)),
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
                        impuestosSubtotal = new List<ImpuestosSubtotal>() // CONSULTAR SI ESTO ES EN MONEDA BASE
                        {
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "E",
                                alicuotaImp = "0.00",
                                baseImponibleImp = "0.00",
                                valorTotalImp = "0.00",
                            },
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "G",
                                alicuotaImp = "16.00",
                                baseImponibleImp = Math.Round(i.total_bruto / i.tasa).ToString().Replace(",", "."),
                                valorTotalImp = Math.Round(i.monto_imp / i.tasa).ToString().Replace(",", "."),
                            },
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "IGTF",
                                alicuotaImp = "3.00",
                                baseImponibleImp = decimal.Parse(i.comentario).ToString().Replace(",", "."),
                                valorTotalImp = (decimal.Parse(i.comentario) * (3 / 100)).ToString().Replace(",", "."),
                            }
                        },
                    }
                },
                detallesItems = i.saFacturaVentaReng.Select(r => new DetallesItem()
                {
                    numeroLinea = r.reng_num.ToString(),
                    codigoPLU = r.co_art.Trim(),
                    indicadorBienoServicio = "2", // CONSULTAR
                    descripcion = new Product().GetArtByID(r.co_art).art_des.Trim(),
                    cantidad = Convert.ToInt32(r.total_art).ToString(),
                    unidadMedida = r.co_uni.Trim(),
                    precioUnitario = Math.Round(r.prec_vta, 2).ToString().Replace(",", "."),
                    precioItem = Math.Round(r.reng_neto, 2).ToString().Replace(",", "."),
                    codigoImpuesto = r.tipo_imp == "1" ? "G" : "E",
                    tasaIVA = r.tipo_imp == "1" ? "16.00" : "0.00",
                    valorIVA = r.tipo_imp == "1" ? Math.Round(r.monto_imp, 2).ToString().Replace(",", ".") : "0.00",
                    valorTotalItem = (r.reng_neto + (r.tipo_imp == "1" ? Math.Round(r.monto_imp, 2) : 0)).ToString().Replace(",", "."),
                    infoAdicionalItem = new List<InfoAdicionalItem>()
                    {
                        new InfoAdicionalItem()
                        {
                            campo = "USD",
                            valor = Math.Round((r.reng_neto + (r.tipo_imp == "1" ? r.monto_imp : 0)) / i.tasa, 2).ToString().Replace(",", ".")
                        }
                    }

                }).ToList(),
                viajes = new Viajes()
                {
                    razonSocialServTransporte = i.campo1,
                    numeroBoleto = i.campo8,
                    puntoSalida = i.campo2,
                    puntoDestino = i.dir_ent
                },
                transporte = new TransporteF()
                {
                    descripcion = i.campo7,
                    codigo = i.campo3
                }
            };

            return JsonConvert.SerializeObject(root);
        }

        public async Task<ModelAuthResponse> SendAuth(ModelAuthRequest auth)
        {
            ModelAuthResponse final = new ModelAuthResponse();
            string url = "https://demoemision.thefactoryhka.com.ve/api/Autenticacion";
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
        
        public async Task<ModelInvoiceInfoResponse> SendInvoiceInfoAsync(string json, string token)
        {
            ModelInvoiceInfoResponse final = new ModelInvoiceInfoResponse();
            string url = "https://demoemision.thefactoryhka.com.ve/api/Emision";
            string data = json;

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
                        if (final.codigo != "200" && final.codigo != "203")
                        {
                            throw new InformationException($"{final.mensaje} ** {final.codigo}");
                        }
                    }
                    else
                    {
                        throw new InformationException($"{response.StatusCode} ** {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }

        public async Task<ModelAssignResponse> SendAssign(ModelAssignRequest assign)
        {
            ModelAssignResponse final = new ModelAssignResponse();
            string url = "https://demoemision.thefactoryhka.com.ve/api/AsignarNumeraciones";
            string data = JsonConvert.SerializeObject(assign);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");

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
    }

    // MODELOS JSON AUTENTICACION
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

    // MODELOS JSON FACTURA DE VENTA
    public class DocumentoElectronico
    {
        public Encabezado encabezado { get; set; }
        public List<DetallesItem> detallesItems { get; set; }
        public Viajes viajes { get; set; }
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
}