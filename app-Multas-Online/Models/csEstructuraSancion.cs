	using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_Multas_Online.Models
{
	public class csEstructuraSancion
	{
        public class requestSanction
        {
            public string sanction_id { get; set; }
            public string description { get; set; }
            public string sanction_type { get; set; }
            public decimal cost { get; set; }
            public DateTime created_at { get; set; }
        }
        public class requestDeleteSanction
        {
            public string sanction_id { get; set; }
        }
        public class responseSanction
        {
            public int response { get; set; }
            public string message { get; set; }
            public string sanction_id { get; set; }
        }
    }
}