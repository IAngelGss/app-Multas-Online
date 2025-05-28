using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static app_Multas_Online.Models.csEstructuraSancion;

namespace app_Multas_Online.Controllers
{
    public class sancionController : Controller
    {
        // GET: sancion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sancion(string idSanction)
        {
            DataSet dsi = new DataSet();
            var url = "";
            if (idSanction == null)
                url = $"http://localhost/api-multas/rest/api/getSanctions";
            else
                url = $"http://localhost/api-multas/rest/api/getSanctionById?sanction_id=" + idSanction;

            // Enviar la URL al navegador para mostrarla en la consola
            ViewBag.ApiUrl = url;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";  // también corriges el typo aquí
            request.Accept = "application/json";
            string responseBody;



            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();

                        }
                    }
                    var jObj = JObject.Parse(responseBody);
                    var tableJson = jObj["Table"].ToString();

                    dsi = JsonConvert.DeserializeObject<DataSet>("{\"Table\":" + tableJson + "}");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
            }

            return View(dsi);
        }
        public ActionResult newSanction()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection formCollection)
        {
            string json, resultJson;
            byte[] reqString, restByte;

            requestSanction insertar = new requestSanction();
            insertar.description = formCollection["description"];
            insertar.sanction_type = formCollection["sanction_type"];
            insertar.cost = Convert.ToDecimal(formCollection["cost"].ToString());
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"http://localhost/api-multas/rest/api/insertSanction";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "post", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseSanction result = new responseSanction();
            result = JsonConvert.DeserializeObject<responseSanction>(resultJson);
            webClient.Dispose();

            if (result.response == 1)
                return RedirectToAction("Sancion", "Sancion");
            return RedirectToAction("newSanction", "Sancion");  


        }

        public ActionResult ActualizarSancion(string sanction_id)
        {
            DataSet dsi = new DataSet();

            var url = $"http://localhost/api-multas/rest/api/getSanctionById?sanction_id=" + sanction_id;
       
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";  // también corriges el typo aquí
            request.Accept = "application/json";
            string responseBody;



            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                           
                        }
                    }
                    var jObj = JObject.Parse(responseBody);
                    var tableJson = jObj["Table"].ToString();
                    dsi = JsonConvert.DeserializeObject<DataSet>("{\"Table\":" + tableJson + "}");
                    Debug.WriteLine(dsi);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
            }

            return View(dsi);
        }
        public ActionResult Actualizar(FormCollection formCollection)
        {
            string json, resultJson;
            byte[] reqString, restByte;
            requestSanction insertar = new requestSanction();
            insertar.sanction_id = formCollection["sanction_id"];
            insertar.description = formCollection["description"];
            insertar.sanction_type = formCollection["sanction_type"];
            insertar.cost = Convert.ToDecimal(formCollection["cost"].ToString());
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"http://localhost/api-multas/rest/api/updateSanction";
            var request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.UTF8.GetBytes(json);
            Debug.Write(json);
            restByte = webClient.UploadData(request.Address.ToString(), "post", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);
            responseSanction result = new responseSanction();
            result = JsonConvert.DeserializeObject<responseSanction>(resultJson);
            webClient.Dispose();
            if (result.response == 1)
                return RedirectToAction("Sancion", "Sancion");
            return RedirectToAction("ActualizarSancion", "Sancion", new { sanction_id = formCollection["sanction_id"] });
        }

        public ActionResult eliminar(string sanction_id)
        {
            string json, resultJson;
            byte[] reqString, restByte;
          

            WebClient webClient = new WebClient();
            string url = $"http://localhost/api-multas/rest/api/deleteSanction";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestDeleteSanction eliminar = new requestDeleteSanction();
            eliminar.sanction_id = sanction_id;

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";

            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "delete", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseSanction result = new responseSanction();

            result = JsonConvert.DeserializeObject<responseSanction>(resultJson);
            webClient.Dispose();

            return RedirectToAction("Sancion", "Sancion");
        }
    }
}

