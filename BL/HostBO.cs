using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class HostBO
    {
        PersonBO PersonalInfo { get; set; }
        List<HostingUnitBO> UnitsHost { get; set; }
     }
}
