using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class HostingUnitBO
    {
        public uint Key { get; set; }
        public string Owner { get; set; }   // ID of the Owner Host 
        public string HostingUnitName { get; set; }
        public bool[,] Diary { set; get; }
        //public bool this[DateTime index]
        //{
        //    get
        //    {
        //        return Diary[index.Month - 1, index.Day - 1];
        //    }
        //    set
        //    {
        //        Diary[index.Month - 1, index.Day - 1] = true;
        //    }
        //}

        public StatusBO Status { get; set; }

        public override string ToString()
        {
            return "Hosting unit Key : " + Key
                + "\nHosting unit name : " + HostingUnitName
                + "\nOwner ID: " + Owner
                + "\n";
        }
    }
}
