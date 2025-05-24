using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
using iTextSharp.text.pdf;
using iTextSharp.text;
using static app_Multas_Online.Models.csEstructuraViolacion;




namespace app_Multas_Online.Controllers
{
    public class violacionController : Controller
    {
        // GET: violacion
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Violacion(string violation_id)
        {
            DataSet dsi = new DataSet();
            var url = "";
            if (violation_id == null)
                url = $"https://localhost:44388/rest/api/getViolations";
            else
                url = $"https://localhost:44388/rest/api/getViolationById?violation_id=" + violation_id;

            // Enviar la URL al navegador para mostrarla en la consola
            ViewBag.ApiUrl = url;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";  // también corriges el typo aquí
            request.Accept = "application/json";
            string responseBody;

            Debug.WriteLine(url);



            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                            Debug.WriteLine(responseBody);
                            JArray jsonArray = JArray.Parse(responseBody);

                            DataTable dt = new DataTable();
                            dt.Columns.Add("violation_id");
                            dt.Columns.Add("violation_date");
                            dt.Columns.Add("status_infraction");

                            dt.Columns.Add("license_plate");
                            dt.Columns.Add("brand");
                            dt.Columns.Add("model");
                            dt.Columns.Add("color");
                            dt.Columns.Add("vehicle_type");

                            dt.Columns.Add("driver_name");
                            dt.Columns.Add("driver_id_number");
                            dt.Columns.Add("driver_address");
                            dt.Columns.Add("driver_phone");
                            dt.Columns.Add("driver_license_number");

                            dt.Columns.Add("officer_name");
                            dt.Columns.Add("officer_id_number");
                            dt.Columns.Add("officer_rank");

                            dt.Columns.Add("sanction_description");
                            dt.Columns.Add("sanction_type");
                            dt.Columns.Add("sanction_cost");

                            foreach (var item in jsonArray)
                            {
                                var row = dt.NewRow();

                                row["violation_id"] = item["violation_id"]?.ToString();
                                row["violation_date"] = item["violation_date"]?.ToString();
                                row["status_infraction"] = item["status_infraction"]?.ToString();

                                row["license_plate"] = item["vehicle"]?["license_plate"]?.ToString();
                                row["brand"] = item["vehicle"]?["brand"]?.ToString();
                                row["model"] = item["vehicle"]?["model"]?.ToString();
                                row["color"] = item["vehicle"]?["color"]?.ToString();
                                row["vehicle_type"] = item["vehicle"]?["vehicle_type"]?.ToString();

                                row["driver_name"] = item["driver"]?["full_name"]?.ToString();
                                row["driver_id_number"] = item["driver"]?["id_number"]?.ToString();
                                row["driver_address"] = item["driver"]?["address"]?.ToString();
                                row["driver_phone"] = item["driver"]?["phone"]?.ToString();
                                row["driver_license_number"] = item["driver"]?["license_number"]?.ToString();

                                row["officer_name"] = item["officer"]?["full_name"]?.ToString();
                                row["officer_id_number"] = item["officer"]?["id_number"]?.ToString();
                                row["officer_rank"] = item["officer"]?["rank_level"]?.ToString();

                                row["sanction_description"] = item["sanction"]?["description"]?.ToString();
                                row["sanction_type"] = item["sanction"]?["sanction_type"]?.ToString();
                                row["sanction_cost"] = item["sanction"]?["cost"]?.ToString();

                                dt.Rows.Add(row);
                            }

                            dsi.Tables.Add(dt);
                        }
                    }
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
            var violacion = new
            {
                vehicle_id = formCollection["vehicle_id"], // ID Vehículo
                driver_id = formCollection["driver_id"], // Placa
                officer_id = formCollection["officer_id"], // Marca
                sanction = new
                {
                    description = formCollection["description"], // Descripción Sanción
                    sanction_type = formCollection["sanction_type"], // Tipo Sanción
                    cost = formCollection["cost"] // Costo Sanción
                }
            };

            string json = JsonConvert.SerializeObject(violacion); // Convertir a JSON
            Debug.WriteLine(json);
            // Realizar la solicitud HTTP al servicio de la API
            using (var webClient = new WebClient())
            {
                string url = "https://localhost:44388/rest/api/insertViolation"; // API de Violación
                var request = (HttpWebRequest)WebRequest.Create(url);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                webClient.Headers["Content-Type"] = "application/json";
                byte[] reqString = Encoding.UTF8.GetBytes(json); // Convertir a bytes
                byte[] restByte = webClient.UploadData(request.Address.ToString(), "POST", reqString); // Realizar la solicitud POST
                string resultJson = Encoding.UTF8.GetString(restByte); // Obtener la respuesta

                // Deserializar la respuesta
                var result = JsonConvert.DeserializeObject<responseViolation>(resultJson);

                // Verificar el resultado
                if (result.response == 1)
                {
                    return RedirectToAction("Violacion", "Violacion"); // Si se guarda correctamente, redirigir a la lista de violaciones
                }
                else
                {
                    return RedirectToAction("newViolacion", "Violacion"); // Si hay un error, regresar al formulario
                }
            }
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

        private DataSet ObtenerDatosViolacion(string violation_id)
        {
            DataSet dsi = new DataSet();
            var url = "";
            if (string.IsNullOrEmpty(violation_id))
                url = $"https://localhost:44388/rest/api/getViolations";
            else
                url = $"https://localhost:44388/rest/api/getViolationById?violation_id=" + violation_id;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (WebResponse response = request.GetResponse())
                using (Stream strReader = response.GetResponseStream())
                using (StreamReader objReader = new StreamReader(strReader))
                {
                    var responseBody = objReader.ReadToEnd();
                    Debug.WriteLine(responseBody);
                    JArray jsonArray = JArray.Parse(responseBody);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("violation_id");
                    dt.Columns.Add("violation_date");
                    dt.Columns.Add("status_infraction");

                    dt.Columns.Add("license_plate");
                    dt.Columns.Add("brand");
                    dt.Columns.Add("model");
                    dt.Columns.Add("color");
                    dt.Columns.Add("vehicle_type");

                    dt.Columns.Add("driver_name");
                    dt.Columns.Add("driver_id_number");
                    dt.Columns.Add("driver_address");
                    dt.Columns.Add("driver_phone");
                    dt.Columns.Add("driver_license_number");

                    dt.Columns.Add("officer_name");
                    dt.Columns.Add("officer_id_number");
                    dt.Columns.Add("officer_rank");

                    dt.Columns.Add("sanction_description");
                    dt.Columns.Add("sanction_type");
                    dt.Columns.Add("sanction_cost");

                    foreach (var item in jsonArray)
                    {
                        var row = dt.NewRow();

                        row["violation_id"] = item["violation_id"]?.ToString();
                        row["violation_date"] = item["violation_date"]?.ToString();
                        row["status_infraction"] = item["status_infraction"]?.ToString();

                        row["license_plate"] = item["vehicle"]?["license_plate"]?.ToString();
                        row["brand"] = item["vehicle"]?["brand"]?.ToString();
                        row["model"] = item["vehicle"]?["model"]?.ToString();
                        row["color"] = item["vehicle"]?["color"]?.ToString();
                        row["vehicle_type"] = item["vehicle"]?["vehicle_type"]?.ToString();

                        row["driver_name"] = item["driver"]?["full_name"]?.ToString();
                        row["driver_id_number"] = item["driver"]?["id_number"]?.ToString();
                        row["driver_address"] = item["driver"]?["address"]?.ToString();
                        row["driver_phone"] = item["driver"]?["phone"]?.ToString();
                        row["driver_license_number"] = item["driver"]?["license_number"]?.ToString();

                        row["officer_name"] = item["officer"]?["full_name"]?.ToString();
                        row["officer_id_number"] = item["officer"]?["id_number"]?.ToString();
                        row["officer_rank"] = item["officer"]?["rank_level"]?.ToString();

                        row["sanction_description"] = item["sanction"]?["description"]?.ToString();
                        row["sanction_type"] = item["sanction"]?["sanction_type"]?.ToString();
                        row["sanction_cost"] = item["sanction"]?["cost"]?.ToString();

                        dt.Rows.Add(row);
                    }

                    dsi.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error o manejarlo
            }

            return dsi;
        }

        public ActionResult ExportarViolacionPDF(string violation_id)
        {

            DataSet dsi = ObtenerDatosViolacion(violation_id);
            if (dsi.Tables.Count == 0 || dsi.Tables[0].Rows.Count == 0)
                return new HttpStatusCodeResult(404, "No se encontraron datos");

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Fuentes
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Título principal
                Paragraph title = new Paragraph("Reporte de Infracción de Tránsito", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(title);

                DataRow row = dsi.Tables[0].Rows[0]; // solo uno si es por ID

                // Función auxiliar para crear tabla de secciones
                PdfPTable CrearTablaSeccion(Dictionary<string, string> data)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    table.SpacingAfter = 10f;
                    float[] widths = new float[] { 30f, 70f };
                    table.SetWidths(widths);

                    foreach (var item in data)
                    {
                        PdfPCell label = new PdfPCell(new Phrase(item.Key, labelFont)) { BackgroundColor = new BaseColor(230, 230, 230), Padding = 5 };
                        PdfPCell value = new PdfPCell(new Phrase(item.Value ?? "", valueFont)) { Padding = 5 };
                        table.AddCell(label);
                        table.AddCell(value);
                    }
                    return table;
                }

                // 1. Datos de la infracción
                document.Add(new Paragraph("Datos de la Infracción", sectionFont));
                document.Add(CrearTablaSeccion(new Dictionary<string, string>
        {
            { "ID Infracción", row["violation_id"]?.ToString() },
            { "Fecha", row["violation_date"]?.ToString() },
            { "Estado", row["status_infraction"]?.ToString() }
        }));

                // 2. Datos del Vehículo
                document.Add(new Paragraph("Datos del Vehículo", sectionFont));
                document.Add(CrearTablaSeccion(new Dictionary<string, string>
        {
            { "Placa", row["license_plate"]?.ToString() },
            { "Marca", row["brand"]?.ToString() },
            { "Modelo", row["model"]?.ToString() },
            { "Color", row["color"]?.ToString() },
            { "Tipo", row["vehicle_type"]?.ToString() }
        }));

                // 3. Datos del Conductor
                document.Add(new Paragraph("Datos del Conductor", sectionFont));
                document.Add(CrearTablaSeccion(new Dictionary<string, string>
        {
            { "Nombre", row["driver_name"]?.ToString() },
            { "Cédula", row["driver_id_number"]?.ToString() },
            { "Dirección", row["driver_address"]?.ToString() },
            { "Teléfono", row["driver_phone"]?.ToString() },
            { "Licencia de Conducir", row["driver_license_number"]?.ToString() }
        }));

                // 4. Datos del Oficial
                document.Add(new Paragraph("Datos del Oficial", sectionFont));
                document.Add(CrearTablaSeccion(new Dictionary<string, string>
        {
            { "Nombre", row["officer_name"]?.ToString() },
            { "Cédula", row["officer_id_number"]?.ToString() },
            { "Rango", row["officer_rank"]?.ToString() }
        }));

                // 5. Sanción
                document.Add(new Paragraph("Datos de la Sanción", sectionFont));
                document.Add(CrearTablaSeccion(new Dictionary<string, string>
        {
            { "Descripción", row["sanction_description"]?.ToString() },
            { "Tipo", row["sanction_type"]?.ToString() },
            { "Costo", "$" + row["sanction_cost"]?.ToString() }
        }));

                document.Close();

                byte[] bytes = ms.ToArray();
                return File(bytes, "application/pdf", "ReporteViolacion.pdf");
            }
        }

        public ActionResult newViolation()
        {
            return View();
        }
    }
}