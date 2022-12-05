using System;
using System.Collections.Generic;

namespace web.Models
{
    public class ZapisVBazo
    {
        public string? meteoID { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double alt { get; set; }
        public string? time { get; set; }
        public double temp { get; set; }
        public double hum { get; set; }

    }
}
            
