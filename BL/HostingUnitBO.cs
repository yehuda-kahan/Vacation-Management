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
        public string HostingUnitName { get; set; }
        public string IdHost { get; set; }   // ID of the Owner Host 
        public string LastNameHost { get; set; }
        public string FirstNameHost { get; set; }
        public string PhoneNumberHost { get; set; }
        public string MailHost { get; set; }
        public uint BankAccountNumberHost { get; set; }
        public bool CollectingClearance { get; set; }
        public bool[,] Diary { get; set; }
        public Status Status { get; set; }

        public bool this[DateTime index]
        {
            get
            {
                return Diary[index.Month - 1, index.Day - 1];
            }
            set
            {
                Diary[index.Month - 1, index.Day - 1] = true;
            }
        }
       
        public override string ToString()
        {
            return "Hosting unit Key : " + Key
                + "\nHosting unit name : " + HostingUnitName
                + "\nOwner ID: " + IdHost
                + "\n";
        }


    }
}
