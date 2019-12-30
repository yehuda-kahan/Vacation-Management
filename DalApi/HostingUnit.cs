using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class HostingUnit
    {
        public uint Key { get; set; }
        public string Owner { get; set; }   // ID of the Owner Host 
        public string HostingUnitName { get; set; }
        public bool[,] Diary { get; set; }
        public Status Status { get; set; }

        public override string ToString()
        {
            return "Hosting unit Key : " + Key
                + "\nOwner ID: " + Owner
                + "\n";
        }
    }
}
