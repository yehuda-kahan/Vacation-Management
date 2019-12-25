using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Configuration
    {
        public static uint GuestRequestserialKey = 10000000;
        public static uint HostSerialKey = 20000000;
        public static uint HostingUnitSerialKey = 10000000;
        public static uint OrderSerialKey = 10000000;
        public static double OrderFee = 0.1;
        public static uint NumDaysToExpire = 10;
    }
}
