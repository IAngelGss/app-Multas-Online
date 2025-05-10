using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
            url = $"http://localhost:44388/rest/api/getSanctions";
            else
                url = $"http://localhost:44388/rest/api/getSanctionById?sanction_id=" + idSanction;


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
                    dsi = JsonConvert.DeserializeObject<DataSet>(responseBody);
                }
            }
            catch(Exception ex)
            {

            }


            return View(dsi);
        }
    }
}