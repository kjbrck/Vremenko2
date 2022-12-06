using System;
using System.Collections.Generic;

namespace web.Models
{
    public class ZapisVBazo
    {
        public enum olbacnost
        {
            clear, mostClear, slightClear, partCloudy, modCloudy, prevCloudy, overcast, FG
        }
        public enum vremPojav
        {
            FG, DZ, FZDZ, RA, FZRA, RASN, SN, SHRA, SHRASN,
            SHSN, SHGR, TS, TSRA, TSRASN, TSS, TSGR
        }
        public string meteoID { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string alt { get; set; }
        public string time { get; set; }
        public string temp { get; set; }
        public string hum { get; set; }
        public olbacnost oblaki{ get; set; }
        public vremPojav vPojav{ get; set; }
    }
}
            
