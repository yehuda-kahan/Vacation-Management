using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Host
    {
        public string Id { get; set; }
        public uint BankNumber { get; set; }
        public uint BranchNumber { get; set; }
        public uint BankAccountNumber { get; set; }
        public bool CollectingClearance { get; set; }
        public Status Status { get; set; }

        public override string ToString()
        {
            return "Host ID : " + Id
                + "\nBank account number : " + BankAccountNumber
                + "\n";
        }
    }
}
