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
using static app_Multas_Online.Models.csEsctructuraConductor;
using static app_Multas_Online.Models.csEstructuraSancion;

namespace app_Multas_Online.Controllers
{
    public class conductorController : Controller
    {
        // GET: conductor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Conductor(string idConductor)
        {
            DataSet dsi = new DataSet();
            var url = "";
            if (idConductor == null)
                url = $"https://localhost:44388/rest/api/getDrivers";
            else
                url = $"https://localhost:44388/rest/api/getDriverById?driver_id=" + idConductor;
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
                Debug.WriteLine("Error: " + ex.Message);
                return View("Error");
            }
            return View(dsi);
        }
        public ActionResult newDriver()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection formCollection) 
        {
            string json, resultJson;
            byte[] reqString, restByte;

            requestDriver insertar = new requestDriver();
            insertar.full_name = formCollection["full_name"];
            insertar.id_number = formCollection["id_number"];
            insertar.address = formCollection["address"];
            insertar.phone = formCollection["phone"];
            insertar.license_number = formCollection["license_number"];
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/insertDriver";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "post", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseDriver result = new responseDriver();
            result = JsonConvert.DeserializeObject<responseDriver>(resultJson);
            webClient.Dispose();

            if (result.response == 1)
                return RedirectToAction("Conductor", "Conductor");
            return RedirectToAction("newDriver", "Conductor");
        }
        public ActionResult ActualizarConductor(string driver_id)
        {
            DataSet dsi = new DataSet();

            var url = $"https://localhost:44388/rest/api/getDriverById?driver_id=" + driver_id;

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
            requestDriver insertar = new requestDriver();
            insertar.driver_id = formCollection["driver_id"];
            insertar.full_name = formCollection["full_name"];
            insertar.id_number = formCollection["id_number"];
            insertar.address = formCollection["address"];
            insertar.phone = formCollection["phone"];
            insertar.license_number = formCollection["license_number"];
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/updateDriver";
            var request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.UTF8.GetBytes(json);
            Debug.Write(json);
            restByte = webClient.UploadData(request.Address.ToString(), "post", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);
            responseDriver result = new responseDriver();
            result = JsonConvert.DeserializeObject<responseDriver>(resultJson);
            webClient.Dispose();
            if (result.response == 1)
                return RedirectToAction("Conductor", "Conductor");
            return RedirectToAction("ActualizarConductor", "Conductor", new { driver_id = formCollection["driver_id"] });
        }

        public ActionResult Eliminar(string driver_id)
        {
            string json, resultJson;
            byte[] reqString, restByte;


            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/deleteDriver";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestDeleteDriver eliminar = new requestDeleteDriver();
            eliminar.driver_id = driver_id;

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";

            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "delete", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseDriver result = new responseDriver();
                
            result = JsonConvert.DeserializeObject<responseDriver>(resultJson);
            webClient.Dispose();

            return RedirectToAction("Conductor", "Conductor");
        }
    }
}