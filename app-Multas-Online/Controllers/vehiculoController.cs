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
using static app_Multas_Online.Models.csEstructuraVehiculo;


namespace app_Multas_Online.Controllers
{
    public class vehiculoController : Controller
    {
        // GET: vehiculo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Vehiculo(string vehicle_id)
        {
            DataSet dsi = new DataSet();
            var url = "";
            if (vehicle_id == null)
                url = $"https://localhost:44388/rest/api/getVehicles";
            else
                url = $"https://localhost:44388/rest/api/getVehicleById?vehicle_id=" + vehicle_id;

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
        public ActionResult newVehiculo()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection formCollection)
        {
            string json, resultJson;
            byte[] reqString, restByte;

            requestVehicle insertar = new requestVehicle();
            insertar.license_plate = formCollection["license_plate"];
            insertar.brand = formCollection["brand"];
            insertar.model = formCollection["model"];
            insertar.color = formCollection["color"];
            insertar.vehicle_type = formCollection["vehicle_type"];
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/insertVehicle";
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
                return RedirectToAction("Vehiculo", "Vehiculo");
            return RedirectToAction("newVehiculo", "Vehiculo");


        }

        public ActionResult ActualizarVehiculo(string vehicle_id)
        {
            DataSet dsi = new DataSet();

            var url = $"https://localhost:44388/rest/api/getVehicleById?vehicle_id=" + vehicle_id;

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
            requestVehicle insertar = new requestVehicle();
            insertar.vehicle_id = formCollection["sanction_id"];
            insertar.license_plate = formCollection["license_plate"];
            insertar.brand = formCollection["brand"];
            insertar.model = formCollection["model"];
            insertar.color = formCollection["color"];
            insertar.vehicle_type = formCollection["vehicle_type"];
            json = JsonConvert.SerializeObject(insertar);

            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/updateVehicle";
            var request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.UTF8.GetBytes(json);
            Debug.Write(json);
            restByte = webClient.UploadData(request.Address.ToString(), "post", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);
            responseVehicle result = new responseVehicle();
            result = JsonConvert.DeserializeObject<responseVehicle>(resultJson);
            webClient.Dispose();
            if (result.response == 1)
                return RedirectToAction("Vehiculo", "Vehiculo");
            return RedirectToAction("ActualizarVehiculo", "Vehiculo", new { vehicle_id = formCollection["vehicle_id"] });
        }

        public ActionResult eliminar(string vehicle_id)
        {
            string json, resultJson;
            byte[] reqString, restByte;


            WebClient webClient = new WebClient();
            string url = $"https://localhost:44388/rest/api/deleteVehicle";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestDeleteVehicle eliminar = new requestDeleteVehicle();
            eliminar.vehicle_id = vehicle_id;

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webClient.Headers["content-type"] = "application/json";

            reqString = Encoding.UTF8.GetBytes(json);
            restByte = webClient.UploadData(request.Address.ToString(), "delete", reqString);
            resultJson = Encoding.UTF8.GetString(restByte);

            responseVehicle result = new responseVehicle();

            result = JsonConvert.DeserializeObject<responseVehicle>(resultJson);
            webClient.Dispose();

            return RedirectToAction("Vehiculo", "Vehiculo");
        }
    }
}