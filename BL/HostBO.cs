using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class HostBO
    {
        public PersonBO PersonalInfo { get; set; }
        public BankBranchBO BankDetales { get; set; }
        public uint BankAccountNumber { get; set; }
        public string WebSite { get; set; }
        public bool CollectingClearance { get; set; }
        public IEnumerable<HostingUnitBO> UnitsHost { get; set; }
        public IEnumerable<OrderBO> OrdersHost { get; set; } //Not includ the canceled and aprroved Orders
        public IEnumerable<OrderBO> AppovedOrdersHost { get; set; } 
        public StatusBO Status { get; set; }
    }
}
