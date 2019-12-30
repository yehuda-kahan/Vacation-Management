using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DS
{
    public  static class DataSource
    {
        public static List<Person> persons;
        public static List<Host> hosts;
        public static List<HostingUnit> hostingUnits;
        public static List<Order> orders;
        public static List<GuestRequest> guestRequests;
        public static List<BankBranch> bankBranches;

        static DataSource()
        {
            persons = new List<Person>()
            {
                new Person
                {
                    Id ="11223344",
                    d
                }
            }
        }

    }
}
