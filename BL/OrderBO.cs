using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OrderBO
    {
        public uint Key { get; set; }
        public HostingUnitBO HostingUnit { get; set; }
        public GuestRequestBO GuestRequest { get; set; }
        public string HostId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public double Fee { get; set; }
        public OrderStatusBO Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CloseDate { get; set; }

        public override string ToString()
        {
            return "Order Key : " + Key
                + "\nHosting unit : " + HostingUnit
                + "\nGuest request : " + GuestRequest
                + "\nOrder date : " + OrderDate.ToString(format: "dd/MM/yyyy")
                + "\n";
        }
    }
}
