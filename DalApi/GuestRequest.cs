using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class GuestRequest
    {
        public uint Key { get; set; }
        public string ClientId { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public AreaLocation Area { get; set; }
        // public SubArea Area { get; set; } // Optiaonal
        public UnitType Type { get; set; }
        public uint Adults { get; set; }
        public uint Children { get; set; }
        public ThreeOptions Pool { get; set; }
        public ThreeOptions Jacuzzi { get; set; }
        public ThreeOptions Garden { get; set; }
        public ThreeOptions ChildrensAttractions { get; set; }

        public override string ToString()
        {
            return "Request Key : " + Key
                + "\nClient ID : " + ClientId
                + "\nCreate date : " + CreateDate.ToString(format: "dd/MM/yyyy")
                + "\nEntry date : "+EntryDate.ToString(format:"dd/MM/yyyy")
                + "\nLeave date : "+ LeaveDate.ToString(format:"dd/MM/yyyy")
                + "\n";
        }
    }
}
