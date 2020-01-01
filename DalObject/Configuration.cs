using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{

    internal class Configuration
    {
        internal static uint GuestRequestserialKey = 10000000;
        internal static uint HostingUnitSerialKey = 20000000;
        internal static uint OrderSerialKey = 30000000;
        internal static double OrderFee = 0.1;
        internal static uint NumDaysToExpire = 10;
    }
}
