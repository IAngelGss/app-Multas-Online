using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_Multas_Online.Models
{
	public class csEstructuraVehiculo
	{
        public class requestVehicle
        {
            public string vehicle_id { get; set; }
            public string license_plate { get; set; }
            public string brand { get; set; }
            public string model { get; set; }
            public string color { get; set; }
            public string vehicle_type { get; set; }
        }
        public class requestDeleteVehicle
        {
            public string vehicle_id { get; set; }
        }
        public class responseVehicle
        {
            public int response { get; set; }
            public string message { get; set; }

            public string vehicle_id { get; set; }
        }
    }
}