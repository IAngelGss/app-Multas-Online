using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_Multas_Online.Models
{
	public class csEsctructuraConductor
	{
        public class requestDriver
        {
            public string driver_id { get; set; }
            public string full_name { get; set; }
            public string id_number { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
            public string license_number { get; set; }
            public DateTime registration_date { get; set; }
        }
        public class requestDeleteDriver
        {
            public string driver_id { get; set; }
        }
        public class responseDriver
        {
            public int response { get; set; }
            public string message { get; set; }
            public string driver_id { get; set; }
        }
    }
}
