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
        public DocumentoElectronico documentoElectronico { get; set; }

        public Root GetInvoiceInfo(saFacturaVenta i)
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
                        numeroDocumento = i.doc_num,
                        serieFacturaAfectada = null,
                        numeroFacturaAfectada = null,
                        fechaFacturaAfectada = null,
                        montoFacturaAfectada = null,
                        fechaEmision = i.fec_emis.ToString("dd/MM/yyyy"),
                        fechaVencimiento = i.fec_venc.ToString("dd/MM/yyyy"),
                        horaEmision = i.fec_emis.ToString("hh:mm:ss tt"),
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
                        numeroIdentificacion = i.co_cli.Substring(1),
                        razonSocial = c.cli_des,
                        direccion = c.direc1,
                        ubigeo = null,
                        pais = c.co_pais,
                        notificar = "Si",
                        telefono = new List<string>() { c.telefonos },
                        correo = new List<string>() { c.email, c.email_alterno },
                    },
                    totales = new Totales()
                    {
                        nroItems = i.saFacturaVentaReng.Count.ToString(),
                        montoGravadoTotal = i.total_bruto.ToString(),
                        montoExentoTotal = "0",
                        subtotal = i.total_bruto.ToString(),
                        totalAPagar = i.total_neto.ToString(),
                        totalIVA = i.monto_imp.ToString(),
                        montoTotalConIVA = i.total_neto.ToString(),
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
                                baseImponibleImp = i.total_bruto.ToString(),
                                valorTotalImp = i.monto_imp.ToString(),
                            },
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "IGTF",
                                alicuotaImp = "3.00",
                                baseImponibleImp = (decimal.Parse(i.comentario) * i.tasa).ToString(),
                                valorTotalImp = ((decimal.Parse(i.comentario) * i.tasa) * (3 / 100)).ToString(),
                            }
                        },
                        formasPago = new List<FormasPago>()
                        {
                            new FormasPago()
                            {
                                descripcion = "Transferencia Bancaria|Venezuela|04008933",
                                fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                forma = "01",
                                monto = i.total_neto.ToString(),
                                moneda = "BSD"
                            },
                            new FormasPago()
                            {
                                descripcion = "Efectivo Divisas|-|-",
                                fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                forma = "01",
                                monto = (i.total_neto * i.tasa).ToString(),
                                moneda = "USD"
                            }
                        }
                    },
                    totalesRetencion = new TotalesRetencion()
                    {
                        totalBaseImponible = i.total_bruto.ToString(),
                        numeroCompRetencion = "1",
                        fechaEmisionCR = DateTime.Now.ToString("dd/MM/yyyy"),
                        totalIVA = (i.monto_imp * ((c.contribu_e ? c.porc_esp : 75) / 100)).ToString(),
                        totalISRL = (i.total_bruto * (2 / 100)).ToString(),
                        totalRetenido = "0.00"
                    },
                    totalesOtraMoneda = new TotalesOtraMoneda()
                    {
                        moneda = "USD",
                        tipoCambio = i.tasa.ToString(),
                        montoGravadoTotal = Math.Round(i.total_bruto / i.tasa).ToString(),
                        montoExentoTotal = "0.00",
                        subtotal = Math.Round(i.total_bruto / i.tasa).ToString(),
                        totalAPagar = Math.Round(i.total_neto / i.tasa).ToString(),
                        totalIVA = Math.Round(i.monto_imp / i.tasa).ToString(),
                        montoTotalConIVA = Math.Round(i.total_neto / i.tasa).ToString(),
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
                                baseImponibleImp = Math.Round(i.total_bruto / i.tasa).ToString(),
                                valorTotalImp = Math.Round(i.monto_imp / i.tasa).ToString(),
                            },
                            new ImpuestosSubtotal()
                            {
                                codigoTotalImp = "IGTF",
                                alicuotaImp = "3.00",
                                baseImponibleImp = decimal.Parse(i.comentario).ToString(),
                                valorTotalImp = (decimal.Parse(i.comentario) * (3 / 100)).ToString(),
                            }
                        },
                    }
                },
                detallesItems = i.saFacturaVentaReng.Select(r => new DetallesItem() { 
                
                    numeroLinea = r.reng_num.ToString(),
                    codigoPLU = r.co_art,
                    indicadorBienoServicio = "2", // CONSULTAR
                    descripcion = new Product().GetArtByID(r.co_art).art_des,
                    cantidad = r.total_art.ToString(),
                    unidadMedida = r.co_uni,
                    precioUnitario = r.prec_vta.ToString(),
                    precioItem = r.reng_neto.ToString(),
                    codigoImpuesto = r.tipo_imp == "1" ? "G" : "E",
                    tasaIVA = r.tipo_imp == "1" ? "16.00" : "0.00",
                    valorIVA = r.tipo_imp == "1" ? r.monto_imp.ToString() : "0.00",
                    valorTotalItem = (r.reng_neto + (r.tipo_imp == "1" ? r.monto_imp : 0)).ToString(),
                    infoAdicionalItem = new List<InfoAdicionalItem>()
                    {
                        new InfoAdicionalItem()
                        {
                            campo = "USD",
                            valor = Math.Round((r.reng_neto + (r.tipo_imp == "1" ? r.monto_imp : 0)) / i.tasa, 2).ToString()
                        }
                    }

                }).ToList(),
                viajes = new Viajes()
                {
                    razonSocialServTransporte = i.campo1,
                    numeroBoleto = i.campo8,
                    fechaSalida = i.campo2,
                    horaSalida = "",
                    puntoDestino = i.dir_ent
                },
                transporte = new TransporteF()
                {
                    descripcion = i.campo7,
                    codigo = i.campo3
                }
            };

            return root;
        }

        public async Task<ModelResponse> SendAuth(ModelAuth auth)
        {
            ModelResponse final = new ModelResponse();
            string url = "https://demoemision.thefactoryhka.com.ve/api/Autenticacion";
            string data = JsonConvert.SerializeObject(auth);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    // request.Content.Headers.Add("Content-Type", "application/json");
                    // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "tu_token_de_autorizacion");

                    HttpResponseMessage response = await client.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    final = JsonConvert.DeserializeObject<ModelResponse>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (final.codigo != 200)
                        {
                            throw new Exception($"ERROR EN RESPUESTA DE API DE AUTENTICACION {final.codigo}");
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        throw new Exception($"ERROR EN RESPUESTA DE SERVIDOR DE AUTENTICACION {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return final;
        }
        
        public async Task SendInvoiceInfoAsync()
        {
            string url = "https://jsonplaceholder.typicode.com/posts";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        Console.WriteLine("Respuesta:");
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"La petición falló con el código de estado: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error al realizar la petición: {ex.Message}");
                }
            }
        }
    }

    // MODELO JSON AUTHENTICATION
    public class ModelAuth
    {
        public string usuario { get; set; }
        public string clave { get; set; }
    }

    public class ModelResponse
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public string token { get; set; }
        public DateTime expiracion { get; set; }
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
        public string comentario1 { get; set; }
        public string numeroDocumento { get; set; }
        public string comentario2 { get; set; }
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
        public string comentario3 { get; set; }
        public string totalAPagar { get; set; }
        public string totalIVA { get; set; }
        public string montoTotalConIVA { get; set; }
        public string comentario4 { get; set; }
        public string montoEnLetras { get; set; }
        public List<ListaDescBonificacion> listaDescBonificacion { get; set; }
        public List<ImpuestosSubtotal> impuestosSubtotal { get; set; }
        public List<FormasPago> formasPago { get; set; }
    }

    public class ListaDescBonificacion
    {
        public string descDescuento { get; set; }
        public string comentario5 { get; set; }
        public string montoDescuento { get; set; }
        public string comentario6 { get; set; }
    }

    public class ImpuestosSubtotal
    {
        public string codigoTotalImp { get; set; }
        public string comentario7 { get; set; }
        public string alicuotaImp { get; set; }
        public string baseImponibleImp { get; set; }
        public string valorTotalImp { get; set; }
        public string comentario8 { get; set; }
        public string comentario9 { get; set; }
        public string comentario10 { get; set; }
        public string comentario11 { get; set; }
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
        public string comentario12 { get; set; }
        public string totalRetenido { get; set; }
        public string comentario13 { get; set; }
        public string totalISRL { get; set; }
        public string comentario14 { get; set; }
    }

    public class TotalesOtraMoneda
    {
        public string comentario15 { get; set; }
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
        public string comentario16 { get; set; }
        public string numeroBoleto { get; set; }
        public string comentario17 { get; set; }
        public string fechaSalida { get; set; }
        public string comentario18 { get; set; }
        public string horaSalida { get; set; }
        public string comentario19 { get; set; }
        public string puntoDestino { get; set; }
        public string comentario20 { get; set; }
    }

    public class TransporteF
    {
        public string descripcion { get; set; }
        public string comentario21 { get; set; }
        public string codigo { get; set; }
        public string comentario22 { get; set; }
    }
}