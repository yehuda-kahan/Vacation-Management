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
        IEnumerable<HostingUnitBO> UnitsHost { get; set; } 
        IEnumerable<OrderBO> OrdersHost { get; set; } //Not includ the canceled Orders
        BankBranchBO BankDetales { get; set; }

        public string WebSite { get; set; }
        public bool CollectingClearance { get; set; }
    }
}
