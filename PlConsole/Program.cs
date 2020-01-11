using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;

namespace PlConsole
{
    class Program
    {
        
        //static IDal dal = DalFactory.GetDal();
        static void Main(string[] args)
        {
           IBl bl = BlFactory.GetBL();
           // bl.SendMail();
            //foreach (var branch in dal.GetBranches())
            //Console.WriteLine("Branch {1} of {0} in {2}", branch.BankName, branch.BranchNumber, branch.BranchCity);
        }
    }
}
