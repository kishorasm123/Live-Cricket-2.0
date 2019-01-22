using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Cricket_2._0.Model
{
    public static class Globals
    {
        public enum enumCarrier { Batting, Bowling };
        public enum enumMatchType { ODI, Test, T20I,IPL };
        public static string strLastNotificationBallNo = string.Empty;
    }
   
}
