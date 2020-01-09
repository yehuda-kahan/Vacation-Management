using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BankBranchBO
    {
        public uint BankNumber { get; set; }
        public string BankName { get; set; }
        public uint BranchNumber { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public StatusBO Status { get; set; }

        public override string ToString()
        {
            return @"Bank number : " + BankNumber
                   + "\nBank name : " + BankName
                   + "\nBranch number : " + BranchNumber
                   + "\nBranch city : " + BranchCity;
        }
    }
}
