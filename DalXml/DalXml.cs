using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;
namespace Dal
{

    class DalXml : IDal
    {
        public static List<Person> persons;
        public static List<Host> hosts;
        public static List<HostingUnit> hostingUnits;
        public static List<Order> orders;
        public static List<GuestRequest> guestRequests;
        public static List<BankBranch> bankBranches;
        public string HostingUnitPath = @"../../../../XMLFiles/HostingUnitXML.xml",
           GuestRequestPath = @"../../../../XMLFiles/GuestRequestXML.xml",
           AdminPath = @"../../../../XMLFiles/AdminXML.xml",
           HostPath = @"../../../../XMLFiles/HostXML.xml",
           OrderdPath = @"../../../../XMLFiles/OrderXML.xml",
           BankBranchPath = @"../../../../XMLFiles/BranchesXML.xml",
           PersonPath = @"../../../../XMLFiles/PersonPath.xml",
           ConfigurationPath = @"../../../../XMLFiles/ConfigurationXML.xml";

        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        DalXml()
        {
            try
            {
                if (!File.Exists(PersonPath))
                    CreatePersonFile();
                else LoadHostingUnitData();
                if (!File.Exists(GuestRequestPath))
                    CreateGuestRequestFile();
                else LoadGuestRequestData();
                if (!File.Exists(AdminPath))
                    CreateAdminFile();
                else LoadAdminFile();
                if (!File.Exists(HostPath))
                    CreateHostFile();
                else LoadHostFile();
                if (!File.Exists(OrderdPath))
                    CreateOrderFile();
                else LoadOrderFile();
                if (!File.Exists(BankBranchPath))
                    CreateBankBranchFile();
                else LoadBankBranchFile();
                if (!File.Exists(ConfigurationPath))
                    CreateConfigurationFile();
                else LoadConfigurationFile();
            }
            catch { }
        }
        public static DalXml Instance { get { return instance; } }

        #endregion

    }
}
