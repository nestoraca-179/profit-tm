using ProfitTM.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
                        serie = "A", // CONSULTAR SI ESTO VARIA
                        sucursal = i.co_sucu_in,
                        tipoDeVenta = "Inmediato",
                        moneda = i.co_mone, // CONSULTAR SI ESTO SIEMPRE SERA EN BSD O USD
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
                    }
                }
            };

            return root;
        }

        public async Task SendInvoiceInfoAsync()
        {
            // URL del recurso al que deseas hacer la petición
            string url = "https://jsonplaceholder.typicode.com/posts";

            // Crear instancia de HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Realizar la petición GET al recurso
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // Verificar si la petición fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta como una cadena
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Mostrar el contenido de la respuesta
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

    public class DocumentoElectronico
    {
        public Encabezado encabezado { get; set; }
        public List<DetallesItem> detallesItems { get; set; }
        public Viajes viajes { get; set; }
        public Transporte transporte { get; set; }
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

    public class Transporte
    {
        public string descripcion { get; set; }
        public string comentario21 { get; set; }
        public string codigo { get; set; }
        public string comentario22 { get; set; }
    }
}