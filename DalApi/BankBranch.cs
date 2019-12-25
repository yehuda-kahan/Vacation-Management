using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BankBranch
    {
        public uint BankNumber { get; private set; }
        public string BankName { get; private set; }
        public uint BranchNumber { get; private set; }
        public string BranchAddress { get; private set; }
        public string BranchCity { get; private set; }

        public override string ToString()
        {
            return @"Bank number : " + BankNumber
                   + "Bank name : " + BankName
                   + "Branch number : " + BranchNumber
                   + "Branch city : " + BranchCity;
        }
    }
}
