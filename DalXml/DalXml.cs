using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;
namespace Dal
{

    class DalXml : IDal
    {
        PersonXmlHandler personHandler = new PersonXmlHandler();
        GuestRequestXmlHandler guestRequestHandler = new GuestRequestXmlHandler();
        HostXmlHandler hostHandler = new HostXmlHandler();
        UnitsXmlHandler unitsHandler = new UnitsXmlHandler();
        OrderXmlHandler OrderHandler = new OrderXmlHandler();

        public static List<Person> persons;
        public static List<Host> hosts;
        public static List<HostingUnit> hostingUnits;
        public static List<Order> orders;
        public static List<GuestRequest> guestRequests;
        public static List<BankBranch> bankBranches;

        public Dictionary<int, string> BankNumberDictionary { get; private set; }
        public Dictionary<int, string> BankAddressDictionary { get; private set; }
        Dictionary<string, object> Config;

        private XElement ConfigRoot;
        private XElement BanksRoot;

        public const string
           BankBranchPath = @"../../../../XMLFiles/Banks.xml",
           ConfigurationPath = @"../../../../XMLFiles/ConfigurationXML.xml";

        #region singelton

        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        DalXml()
        {
            try
            {
                if (!File.Exists(personHandler.PersonPath))
                    personHandler.CreatePersonFile();
                if (!File.Exists(guestRequestHandler.GuestRequestPath))
                    guestRequestHandler.CreateGuestRequestFile();
                if (!File.Exists(hostHandler.HostPath))
                    hostHandler.CreateHostFile();
                if (!File.Exists(unitsHandler.UnitsPath))
                    unitsHandler.CreateUnitFile();
                if (!File.Exists(OrderHandler.OrderPath))
                    OrderHandler.CreateOrderFile();
                if (!File.Exists(BankBranchPath))
                    CreateXMLBankFiles();
            }
            catch { }
        }
        public static DalXml Instance { get { return instance; } }

        #endregion

        #region Person Function functions

        public bool ChaeckPersonMail(string mail)
        {
            personHandler.load();
            return persons.Any(x => mail == x.Email);
        }
        public Person GetPersonById(string Id)
        {
            personHandler.load();
            Person person = persons.FirstOrDefault(x => Id == x.Id);
            return person == null ? throw new MissingMemberException("Person ID", Id) : person.Clone();
        }

        public Person GetPersonByMail(string mail)
        {
            personHandler.load();
            Person person = persons.FirstOrDefault(x => mail == x.Email);
            return person == null ? throw new MissingMemberException("Person ID", mail) : person.Clone();
        }

        public void AddPerson(Person person)
        {
            personHandler.load();
            if (persons.Any(x => person.Id == x.Id))
                throw new DuplicateKeyException(person.Id, " " + person.Id + " Person ID");
            persons.Add(person.Clone());
            personHandler.Save();
        }

        public IEnumerable<Person> GetPersonsByName(string fullName)
        {
            personHandler.load();
            var personsList = from item in persons
                              let FullName = item.FirstName + " " + item.LastName
                              where FullName == fullName
                              select item.Clone();
            return personsList.Count() > 0 ? persons : throw new MissingMemberException("person", fullName);
        }

        public void UpdatePerson(Person person)
        {
            personHandler.load();
            int count = persons.RemoveAll(x => x.Id == person.Id);
            if (count == 0)
                throw new MissingMemberException("Person ID", person.Id);
            persons.Add(person.Clone());
            personHandler.Save();
        }

        public void UpdateStatusPerson(string id, Status status)
        {
            personHandler.load();
            bool found = false;
            foreach (Person item in persons)
            {
                if (item.Id == id)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Person ID", id);
            else
                personHandler.Save();
        }
        #endregion

        #region Guest Request functions
        public GuestRequest GetRequest(uint Key)
        {
            guestRequestHandler.load();
            GuestRequest request = guestRequests.FirstOrDefault(x => Key == x.Key);
            return request == null ?
                throw new MissingMemberException("Guest Request Key", Convert.ToString(Key)) : request.Clone();
        }

        public IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId)
        {
            guestRequestHandler.load();
            var requsts = from item in guestRequests
                          where item.ClientId == ClientId
                          select item.Clone();
            if (requsts.Any())
                throw new MissingMemberException("Booking requsts of Client ID", ClientId);
            return requsts;
        }

        public uint AddGuestRequest(GuestRequest request)
        {
            guestRequestHandler.load();
            if (request.Key == 0)
            {
                Config = getConfig();
                request.Key = (Convert.ToUInt32(Config["GuestRequestserialKey"]) + 1);
                SetConfig("GuestRequestserialKey", request.Key);
            }
            if (guestRequests.Any(x => request.Key == x.Key)) // if there is a problem white the serialNumber
                throw new DuplicateKeyException(Convert.ToString(request.Key), "" + Convert.ToString(request.Key) + " Guest Request Key");
            guestRequests.Add(request.Clone());
            guestRequestHandler.Save();
            return request.Key;
        }

        public void UpdateGuestRequest(GuestRequest request)
        {
            guestRequestHandler.load();
            int count = guestRequests.RemoveAll(x => x.Key == request.Key);
            if (count == 0)
                throw new MissingMemberException("Request Key", Convert.ToString(request.Key));
            guestRequests.Add(request.Clone());
            guestRequestHandler.Save();
        }

        public void UpdateStatusRequest(uint Key, RequestStatus status)
        {
            guestRequestHandler.load();
            bool found = false;
            foreach (GuestRequest item in guestRequests)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Guest Request Key", Convert.ToString(Key));
            else
                guestRequestHandler.Save();
        }
        #endregion

        #region Host functions
        public Host GetHost(string Id)
        {
            hostHandler.load();
            Host host = hosts.FirstOrDefault(x => x.Id == Id);
            return host == null ? throw new MissingMemberException("Host ID", Id) : host.Clone();
        }

        public void AddHost(Host host)
        {
            hostHandler.load();
            if (hosts.Any(x => host.Id == x.Id))
                throw new DuplicateKeyException(host.Id, "" + host.Id + " Host ID number");
            hosts.Add(host.Clone());
            hostHandler.Save();
        }

        public void DelHost(string Id)
        {
            hostHandler.load();
            bool found = false;
            foreach (Host item in hosts)
            {
                if (item.Id == Id)
                {
                    found = true;
                    item.Status = Status.INACTIVE;
                    break;
                }
            }
            if (!found)
                throw new MissingMemberException("Host ID", Id);
            else
                hostHandler.Save();
        }

        public void UpdateHost(Host host)
        {
            hostHandler.load();
            int count = hosts.RemoveAll(x => x.Id == host.Id);
            if (count == 0)
                throw new MissingMemberException("Host ID", host.Id);
            hosts.Add(host.Clone());
            hostHandler.Save();
        }

        #endregion

        #region Units functions
        public HostingUnit GetUnit(uint Key)
        {
            unitsHandler.load();
            HostingUnit host = hostingUnits.FirstOrDefault(x => x.Key == Key);
            return host == null ? throw new MissingMemberException("unit Key", Convert.ToString(Key)) : host.Clone();
        }

        public uint AddHostingUnit(HostingUnit unit)
        {
            unitsHandler.load();
            if (unit.Key == 0)
            {
                Config = getConfig();
                unit.Key = (Convert.ToUInt32(Config["HostingUnitSerialKey"]) + 1);
                SetConfig("HostingUnitSerialKey", unit.Key);
            }
            if (hostingUnits.Any(x => unit.Key == x.Key))
                throw new DuplicateKeyException(Convert.ToString(unit.Key), "" + Convert.ToString(unit.Key) + " Unit Key");
            hostingUnits.Add(unit.Clone());
            unitsHandler.Save();
            return unit.Key;
        }

        public void UpdateHostingUnit(HostingUnit unit)
        {
            unitsHandler.load();
            int count = hostingUnits.RemoveAll(x => x.Key == unit.Key);
            if (count == 0)
                throw new MissingMemberException("Unit Key", Convert.ToString(unit.Key));
            hostingUnits.Add(unit.Clone());
            unitsHandler.Save();
        }

        public void DelHostingUnit(uint Key)
        {
            unitsHandler.load();
            bool found = false;
            foreach (HostingUnit item in hostingUnits)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = Status.INACTIVE;
                    break;
                }
            }
            if (!found)
                throw new MissingMemberException("hosting Unit Key", Convert.ToString(Key));
            else
                unitsHandler.Save();
        }
        #endregion

        #region Order functions
        public Order GetOrder(uint Key)
        {
            OrderHandler.load();
            Order order = orders.FirstOrDefault(x => x.Key == Key);
            return order == null ? throw new MissingMemberException("Order Key", Convert.ToString(Key)) : order.Clone();
        }

        public Order GetOrderGuestRequestKey(uint GuestRequestKey)
        {
            OrderHandler.load();
            Order order = orders.FirstOrDefault(x => x.GuestRequestKey == GuestRequestKey);
            return order == null ? throw new MissingMemberException("No order with guest request Key",
                Convert.ToString(GuestRequestKey)) : order.Clone();
        }

        public IEnumerable<Order> GetOrdersHostingUnitKey(uint HostingUnitKey)
        {
            OrderHandler.load();
            var ordersList = from item in orders
                             where item.HostingUnitKey == HostingUnitKey
                             select item.Clone();
            return ordersList.Count() > 0 ? orders : throw new MissingMemberException("orders for this unit Key",
                Convert.ToString(HostingUnitKey));
        }

        public uint AddOrder(Order odr)
        {
            OrderHandler.load();
            if (odr.Key == 0)
            {
                Config = getConfig();
                odr.Key = (Convert.ToUInt32(Config["OrderSerialKey"]) + 1);
                SetConfig("OrderSerialKey", odr.Key);
            }
            if (orders.Any(x => odr.Key == x.Key))
                throw new DuplicateKeyException(Convert.ToString(odr.Key), "" + Convert.ToString(odr.Key) + " Order Key");
            orders.Add(odr.Clone());
            OrderHandler.Save();
            return odr.Key;
        }

        public void UpdOrder(Order odr)
        {
            OrderHandler.load();
            int count = orders.RemoveAll(x => x.Key == odr.Key);
            if (count == 0)
                throw new MissingMemberException("Order key", Convert.ToString(odr.Key));
            orders.Add(odr.Clone());
            OrderHandler.Save();
        }

        public void UpdateStatusOrder(uint Key, OrderStatus status)
        {
            OrderHandler.load();
            bool found = false;
            foreach (Order item in orders)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Order Key", Convert.ToString(Key));
            else
                OrderHandler.Save();
        }
        #endregion

        #region ListFunctions
        public IEnumerable<Host> GetHosts(Func<Host, bool> predicate)
        {
            hostHandler.load();
            return from item in hosts
                   where predicate(item)
                   select item;
        }

        public IEnumerable<Order> GetOrders(Func<Order, bool> predicate)
        {
            OrderHandler.load();
            return from item in orders
                   where predicate(item)
                   select item.Clone();
        }

        public IEnumerable<GuestRequest> GetGuestRequests(Func<GuestRequest, bool> predicate)
        {
            guestRequestHandler.load();
            return from item in guestRequests
                   where predicate(item)
                   select item.Clone();
        }

        public IEnumerable<GuestRequest> GetGuestRequests()
        {
            guestRequestHandler.load();
            return from item in guestRequests
                   select item.Clone();
        }

        public IEnumerable<BankBranch> GetBranches()
        {
            return from item in bankBranches
                   select item.Clone();
        }

        public IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit, bool> predicate)
        {
            unitsHandler.load();
            return from item in hostingUnits
                   where predicate(item)
                   select item.Clone();
        }
        #endregion

        #region BankXml
        public void CreateXMLBankFiles()
        {
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"https://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, BankBranchPath);
            }
            catch (Exception)
            {
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, BankBranchPath);
            }
            finally
            {
                wc.Dispose();
            }
        }

        public Dictionary<int, string> BuildDictionaryBankName()
        {
            Load(ref BanksRoot, BankBranchPath);
            return (from Bank in BanksRoot.Elements()
                    select new { Bankcode = int.Parse(Bank.Element("קוד_בנק").Value), Bnakname = Bank.Element("שם_בנק").Value }
                        into temp
                    group temp by temp.Bankcode).ToDictionary(x => x.Key, x => x.ElementAt(0).Bnakname);
        }

        public Dictionary<int, string> buildDictioneryBanches(int BankNum)
        {
            Load(ref BanksRoot, BankBranchPath);
            return (from Bank in BanksRoot.Elements()
                    where (int.Parse(Bank.Element("קוד_בנק").Value) == BankNum)
                    select new
                    {
                        BranchCode = int.Parse(Bank.Element("קוד_סניף").Value),
                        address = Bank.Element("כתובת_ה-ATM").Value,
                        City = Bank.Element("ישוב").Value
                    }
                    into temp
                    group temp by temp.BranchCode).ToDictionary(x => x.Key, x => x.ElementAt(0).address + "@" + x.ElementAt(0).City);
        }
        #endregion


        public event Action<Dictionary<String, Object>> ConfigHandler;

        private void Load(ref XElement t, string a)
        {
            try
            {
                t = XElement.Load(a);
            }
            catch
            {
                throw new DirectoryNotFoundException(" שגיאה! בעיית טעינת קובץ:" + a);
            }

        }

        public Dictionary<string, object> getConfig()
        {
            try
            {
                Load(ref ConfigRoot, ConfigurationPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            bool v;
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            foreach (var item in ConfigRoot.Elements())
            {
                if (Convert.ToBoolean(item.Element("Value").Element("Readable").Value))
                {
                    keyValues.Add(item.Element("Key").Value, int.Parse(item.Element("Value").Element("value").Value));
                }
            }
            return keyValues;
        }

        public void SetConfig(string parm, Object value)
        {
            try
            {
                Load(ref ConfigRoot, ConfigurationPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            foreach (var item in ConfigRoot.Elements())
            {
                if (item.Element("Key").Value == parm)
                {
                    if (Convert.ToBoolean(item.Element("Value").Element("Writable").Value))
                    {
                        item.Element("Value").Element("value").Value = value.ToString();
                        ConfigRoot.Save(ConfigurationPath);
                        return;
                    }
                    throw new AccessViolationException("שגיאה! אין הרשאה לשנות מאפיין קונפיגורציה זה.");
                }
            }
            throw new KeyNotFoundException("שגיאה! לא קיים מאפיין קונפיגורציה בשם זה במערכת.");
        }
    }
}
