using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static app_Multas_Online.Models.csEstructuraAgente;
using static app_Multas_Online.Models.csEstructuraSancion;

namespace app_Multas_Online.Controllers
{
    public class trafficOfficerController : Controller
    {
        // GET: TrafficOfficer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Agente(string officer_id)
        {
            DataSet dsi = new DataSet();
            var url = "";
            if (officer_id == null)
                url = $"https://localhost:44388/rest/api/getTrafficOfficers";
            else
                url = $"https://localhost:44388/rest/api/getTrafficOfficerById?officer_id=" + officer_id;

            ViewBag.ApiUrl = url;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
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

        // GET: TrafficOfficer/New
        public ActionResult New()
        {
            return View();
        }

        // POST: TrafficOfficer/Guardar
        [HttpPost]
        public ActionResult Guardar(FormCollection formCollection)
        {
            requestTrafficOfficer insertar = new requestTrafficOfficer();
            
            string json, resultJson;
            byte[] reqString, restByte;

            insertar.full_name = formCollection["full_name"];
            insertar.id_number = formCollection["id_number"];
            insertar.rank_level = formCollection["rank_level"];
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/insertTrafficOfficer";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "post", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseTrafficOfficer result = new responseTrafficOfficer();
            result = JsonConvert.DeserializeObject<responseTrafficOfficer>(resultJson);
            webClient.Dispose();

            if (result.response == 1)
                return RedirectToAction("Agente", "Agente");
            return RedirectToAction("newOfficer", "Agente");


         
        }

        public ActionResult ActualizarAgente(string officer_id)
        {
            DataSet dsi = new DataSet();

            var url = $"https://localhost:44388/rest/api/getTrafficOfficerById?officer_id=" + officer_id;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
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

        // POST: TrafficOfficer/Actualizar
        [HttpPost]
        public ActionResult Actualizar(FormCollection formCollection)
        {
            requestTrafficOfficer insertar = new requestTrafficOfficer();

            string json, resultJson;
            byte[] reqString, restByte;
            insertar.full_name = formCollection["full_name"];
            insertar.id_number = formCollection["id_number"];
            insertar.rank_level = formCollection["rank_level"];
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/updateTrafficOfficer";
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
                return RedirectToAction("Agente", "Agente");
            return RedirectToAction("ActualizarAgente", "Agente", new { officer_id = formCollection["officer_id"] });
        }



        // GET: TrafficOfficer/Eliminar/{officer_id}
        public ActionResult Eliminar(string officer_id)
        {
            string json, resultJson;
            byte[] reqString, restByte;

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/deleteTrafficOfficer";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestDeleteTrafficOfficer eliminar = new requestDeleteTrafficOfficer();
            eliminar.officer_id = officer_id;

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";

            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "delete", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseSanction result = new responseSanction();

            result = JsonConvert.DeserializeObject<responseSanction>(resultJson);
            webClient.Dispose();

            return RedirectToAction("Agente", "Agente");

            
        }
    }
}
