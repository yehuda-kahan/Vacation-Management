using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using DS;


namespace Dal
{
    sealed class DalObject : IDal
    {
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { }
        DalObject() { }
        public static DalObject Instance { get { return instance; } }
        #endregion

        #region Person Function functions

        public bool ChaeckPersonMail(string mail)
        {
            return DataSource.persons.Any(x => mail == x.Email);
        }
        public Person GetPersonById(string Id)
        {
            Person person = DataSource.persons.FirstOrDefault(x => Id == x.Id);
            return person == null ? throw new MissingMemberException("Person ID", Id) : person.Clone();
        }

        public Person GetPersonByMail(string mail)
        {
            Person person = DataSource.persons.FirstOrDefault(x => mail == x.Email);
            return person == null ? throw new MissingMemberException("Person ID", mail) : person.Clone();
        }

        public void AddPerson(Person person)
        {
            if (DataSource.persons.Any(x => person.Id == x.Id))
                throw new DuplicateKeyException(person.Id, " " + person.Id + " Person ID");
            DataSource.persons.Add(person.Clone());
        }

        public IEnumerable<Person> GetPersonsByName(string fullName)
        {
            var persons = from item in DataSource.persons
                          let FullName = item.FirstName + " " + item.LastName
                          where FullName == fullName
                          select item.Clone();
            return persons.Count() > 0 ? persons : throw new MissingMemberException("person", fullName);
        }

        public void UpdatePerson(Person person)
        {
            int count = DataSource.persons.RemoveAll(x => x.Id == person.Id);
            if (count == 0)
                throw new MissingMemberException("Person ID", person.Id);
            DataSource.persons.Add(person.Clone());
        }

        public void UpdateStatusPerson(string id, Status status)
        {
            bool found = false;
            foreach (Person item in DataSource.persons)
            {
                if (item.Id == id)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Person ID", id);
        }
        #endregion

        #region Guest Request functions
        public GuestRequest GetRequest(uint Key)
        {
            GuestRequest request = DataSource.guestRequests.FirstOrDefault(x => Key == x.Key);
            return request == null ?
                throw new MissingMemberException("Guest Request Key", Convert.ToString(Key)) : request.Clone();
        }

        public IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId)
        {
            var requsts = from item in DataSource.guestRequests
                          where item.ClientId == ClientId
                          select item.Clone();
            if (requsts.Any())
                throw new MissingMemberException("Booking requsts of Client ID", ClientId);
            return requsts;
        }

        public uint AddGuestRequest(GuestRequest request)
        {
            if (request.Key == 0)
                request.Key = Configuration.GuestRequestserialKey++;
            if (DataSource.guestRequests.Any(x => request.Key == x.Key)) // if there is a problem white the serialNumber
                throw new DuplicateKeyException(Convert.ToString(request.Key), "" + Convert.ToString(request.Key) + " Guest Request Key");
            DataSource.guestRequests.Add(request.Clone());
            return request.Key;
        }

        public void UpdateGuestRequest(GuestRequest request)
        {
            int count = DataSource.guestRequests.RemoveAll(x => x.Key == request.Key);
            if (count == 0)
                throw new MissingMemberException("Request Key", Convert.ToString(request.Key));
            DataSource.guestRequests.Add(request.Clone());
        }

        public void UpdateStatusRequest(uint Key, RequestStatus status)
        {
            bool found = false;
            foreach (GuestRequest item in DataSource.guestRequests)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Guest Request Key", Convert.ToString(Key));
        }
        #endregion

        #region Host functions
        public Host GetHost(string Id)
        {
            Host host = DataSource.hosts.FirstOrDefault(x => x.Id == Id);
            return host == null ? throw new MissingMemberException("Host ID", Id) : host.Clone();
        }

        public void AddHost(Host host)
        {
            if (DataSource.hosts.Any(x => host.Id == x.Id))
                throw new DuplicateKeyException(host.Id, "" + host.Id + " Host ID number");
            DataSource.hosts.Add(host.Clone());
        }

        public void DelHost(string Id)
        {
            bool found = false;
            foreach (Host item in DataSource.hosts)
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
        }

        public void UpdateHost(Host host)
        {
            int count = DataSource.hosts.RemoveAll(x => x.Id == host.Id);
            if (count == 0)
                throw new MissingMemberException("Host ID", host.Id);
            DataSource.hosts.Add(host.Clone());
        }

        #endregion

        #region Units functions
        public HostingUnit GetUnit(uint Key)
        {
            HostingUnit host = DataSource.hostingUnits.FirstOrDefault(x => x.Key == Key);
            return host == null ? throw new MissingMemberException("unit Key", Convert.ToString(Key)) : host.Clone();
        }

        public uint AddHostingUnit(HostingUnit unit)
        {
            if (unit.Key == 0)
                unit.Key = Configuration.HostingUnitSerialKey++;
            if (DataSource.hostingUnits.Any(x => unit.Key == x.Key))
                throw new DuplicateKeyException(Convert.ToString(unit.Key), "" + Convert.ToString(unit.Key) + " Unit Key");
            DataSource.hostingUnits.Add(unit.Clone());
            return unit.Key;
        }

        public void UpdateHostingUnit(HostingUnit unit)
        {
            int count = DataSource.hostingUnits.RemoveAll(x => x.Key == unit.Key);
            if (count == 0)
                throw new MissingMemberException("Unit Key", Convert.ToString(unit.Key));
            DataSource.hostingUnits.Add(unit.Clone());
        }

        public void DelHostingUnit(uint Key)
        {
            bool found = false;
            foreach (HostingUnit item in DataSource.hostingUnits)
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
        }
        #endregion

        #region Order functions
        public Order GetOrder(uint Key)
        {
            Order order = DataSource.orders.FirstOrDefault(x => x.Key == Key);
            return order == null ? throw new MissingMemberException("Order Key", Convert.ToString(Key)) : order.Clone();
        }

        public Order GetOrderGuestRequestKey(uint GuestRequestKey)
        {
            Order order = DataSource.orders.FirstOrDefault(x => x.GuestRequestKey == GuestRequestKey);
            return order == null ? throw new MissingMemberException("No order with guest request Key",
                Convert.ToString(GuestRequestKey)) : order.Clone();
        }

        public IEnumerable<Order> GetOrdersHostingUnitKey(uint HostingUnitKey)
        {
            var orders = from item in DataSource.orders
                         where item.HostingUnitKey == HostingUnitKey
                         select item.Clone();
            return orders.Count() > 0 ? orders : throw new MissingMemberException("orders for this unit Key",
                Convert.ToString(HostingUnitKey));
        }

        public uint AddOrder(Order odr)
        {
            if (odr.Key == 0)
                odr.Key = Configuration.OrderSerialKey++;
            if (DataSource.orders.Any(x => odr.Key == x.Key))
                throw new DuplicateKeyException(Convert.ToString(odr.Key), "" + Convert.ToString(odr.Key) + " Order Key");
            DataSource.orders.Add(odr.Clone());
            return odr.Key;
        }

        public void UpdOrder(Order odr)
        {
            int count = DataSource.orders.RemoveAll(x => x.Key == odr.Key);
            if (count == 0)
                throw new MissingMemberException("Order key", Convert.ToString(odr.Key));
            DataSource.orders.Add(odr.Clone());
        }

        public void UpdateStatusOrder(uint Key, OrderStatus status)
        {
            bool found = false;
            foreach (Order item in DataSource.orders)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Order Key", Convert.ToString(Key));
        }
        #endregion

        #region ListFunctions
        public IEnumerable<Host> GetHosts(Func<Host, bool> predicate)
        {
            return from item in DataSource.hosts
                   where predicate(item)
                   select item;
        }

        public IEnumerable<Order> GetOrders(Func<Order, bool> predicate)
        {
            return from item in DataSource.orders
                   where predicate(item)
                   select item.Clone();
        }

        public IEnumerable<GuestRequest> GetGuestRequests(Func<GuestRequest, bool> predicate)
        {
            return from item in DataSource.guestRequests
                   where predicate(item)
                   select item.Clone();
        }

        public IEnumerable<GuestRequest> GetGuestRequests()
        {
            return from item in DataSource.guestRequests
                   select item.Clone();
        }

        public IEnumerable<BankBranch> GetBranches()
        {
            return from item in DataSource.bankBranches
                   select item.Clone();
        }

        public IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit, bool> predicate)
        {
            return from item in DataSource.hostingUnits
                   where predicate(item)
                   select item.Clone();
        }
        #endregion

        #region
        public BankBranch GetBranch(uint bankNum, uint branchNum)
        {
            return DataSource.bankBranches.FirstOrDefault
                (x => x.BankNumber == bankNum && x.BranchNumber == branchNum).Clone();//TODO throw
        }

        #endregion
    }
}
