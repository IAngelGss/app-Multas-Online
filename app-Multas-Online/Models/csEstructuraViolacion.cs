using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static app_Multas_Online.Models.csEsctructuraConductor;
using static app_Multas_Online.Models.csEstructuraAgente;
using static app_Multas_Online.Models.csEstructuraSancion;
using static app_Multas_Online.Models.csEstructuraVehiculo;

namespace app_Multas_Online.Models
{
    public class csEstructuraViolacion
    {
        public class requestViolation
        {
            public string violation_id { get; set; }
            public DateTime violation_date { get; set; }
            public string status_infraction { get; set; }
            public requestVehicle vehicle { get; set; }
            public requestDriver driver { get; set; }
            public requestTrafficOfficer officer { get; set; }
            public requestSanction sanction { get; set; }
        }

        public class requestDeleteViolation
        {
            public string violation_id { get; set; }
        }

        public class responseViolation
        {
            public int response { get; set; }
            public string message { get; set; }
            public string violation_id { get; set; }
        }
    }
}