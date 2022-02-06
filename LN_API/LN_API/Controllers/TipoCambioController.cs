using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml;
using LN_API.cr.fi.bccr.gee;
using Newtonsoft.Json;

namespace LN_API.Controllers
{
    public class EjecutadorWebServicesController : ApiController
    { 
        
        const string CodigoCompra = "317";
        const string CodigoVenta = "318";
        wsindicadoreseconomicos tipocambio;
        public EjecutadorWebServicesController()
        {
            tipocambio = new wsindicadoreseconomicos();
        }
        private string consumirWebServiceGet(string urlRequest)
        {
            var request = (HttpWebRequest)WebRequest.Create(urlRequest);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response != null)
            {
                return obtenerData(response);
            }
            return "Respuesta no ha sido obtenida";
        }
        private string consumirWebService_IntegradoXML(string Tipo) {
            
            string[] valoresParametros = ConfigurationManager.AppSettings["WebServiceUrlParametros_Valores"].Split(',');
            string result;
            DateTime ahora = DateTime.Now;
            DateTime haceMedioAnio = ahora.AddMonths(-6);
            if (Tipo == "Compra")
            {
                result = tipocambio.ObtenerIndicadoresEconomicosXML("317", haceMedioAnio.ToString("d"), ahora.ToString("d"), "Pablo", "S", "ps7566966@gmail.com", "H2OP3O6PAC");
            }
            else
            {
                result = tipocambio.ObtenerIndicadoresEconomicosXML("318", haceMedioAnio.ToString("d"), ahora.ToString("d"), "Pablo", "S", "ps7566966@gmail.com", "H2OP3O6PAC");
            }
            XDocument doc = XDocument.Parse(result);
            return doc.ToString();

        }
        private string consumirWebService_IntegradoJson(string Tipo)
        {
            string result;
            DateTime ahora = DateTime.Now;
            DateTime haceMedioAnio = ahora.AddMonths(-6);
            if (Tipo == "Compra")
            {
                result = tipocambio.ObtenerIndicadoresEconomicosXML("317", haceMedioAnio.ToString("d"), ahora.ToString("d"), "Pablo", "S", "ps7566966@gmail.com", "H2OP3O6PAC");
            }
            else
            {
                result = tipocambio.ObtenerIndicadoresEconomicosXML("318", haceMedioAnio.ToString("d"), ahora.ToString("d"), "Pablo", "S", "ps7566966@gmail.com", "H2OP3O6PAC");
            }
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            
            return JsonConvert.SerializeXmlNode(doc);

        }
        private string obtenerData(HttpWebResponse response)
        {
            Stream receiveStream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
             
            return readStream.ReadToEnd();
        }
        private string ObtenerCodigoTipoCambio(string nombre)
        {
            return nombre == "Compra" ? CodigoCompra : CodigoVenta;
        }
        [HttpGet]
        [Route("api/TipoCambio/ObtenerTiposDeCambio")]
        public string ObtenerTiposDeCambio(string NombreTipoCambio,string Formato)
        { 
            try
            {

                string[] urls = ConfigurationManager.AppSettings["WebServiceUrls"].Split(',');
                string[] metodos = ConfigurationManager.AppSettings["WebServiceUrlMetodos"].Split(',');
                string[] parametros = ConfigurationManager.AppSettings["WebServiceUrlParametros"].Split(',');
                string[] valoresParametros = ConfigurationManager.AppSettings["WebServiceUrlParametros_Valores"].Split(',');

                string urlRequest = "";

                urlRequest = ConstruirURL(NombreTipoCambio, urls, metodos, parametros, valoresParametros, urlRequest);

                if (urlRequest != null)
                {
                    string xmlRESPONSE;
                    if (Formato == "Json")
                    {
                        xmlRESPONSE = consumirWebService_IntegradoJson(NombreTipoCambio);
                    }
                    else 
                    {
                        //XML
                        xmlRESPONSE = consumirWebServiceGet(urlRequest);
                        XDocument doc = XDocument.Parse(xmlRESPONSE);
                    } 
                    return xmlRESPONSE.ToString();
                }

                return "Respuesta no ha sido obtenida.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string ConstruirURL(string Nombre, string[] urls, string[] metodos, string[] parametros, string[] valoresParametros, string urlRequest)
        {
            int indiceDelServidor;
            int indiceDeLosParametros;

            for (indiceDelServidor = 0; indiceDelServidor < urls.Length; indiceDelServidor++)
            {
                if (urls[indiceDelServidor] == null)
                    continue;
                if (metodos[indiceDelServidor] == null)
                    continue;

                urlRequest = urls[indiceDelServidor] + metodos[indiceDelServidor];

                for (indiceDeLosParametros = 0; indiceDeLosParametros < parametros.Length; indiceDeLosParametros++)
                {
                    if (valoresParametros[indiceDelServidor] == null)
                        continue;
                    if (parametros[indiceDelServidor] == null)
                        continue;
                    if (valoresParametros[indiceDeLosParametros] == "{0}")
                    {
                        urlRequest = urlRequest + parametros[indiceDeLosParametros] + "=" + ObtenerCodigoTipoCambio(Nombre) + "&";
                    }
                    else if (valoresParametros[indiceDeLosParametros] == "{1}")
                    {
                        urlRequest = urlRequest + parametros[indiceDeLosParametros] + "=" + Nombre.Trim() + "&";
                    }
                    else
                    {
                        urlRequest = urlRequest + parametros[indiceDeLosParametros] + "=" + valoresParametros[indiceDeLosParametros] + "&";
                    }
                }

                urlRequest = urlRequest.Remove(urlRequest.Length - 1);

                break;

            }

            return urlRequest;
        }
    }
}
