using System;
using System.Collections.Generic;
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
        static readonly DalObject instance = new DalObject();
        static DalObject() { }
        DalObject() { }
        public static DalObject Instance { get { return instance; } }

        public Person GetPerson(string Id)
        {
            Person person = DataSource.persons.FirstOrDefault(x => Id == x.Id);
            return person == null ? throw new MissingException("Person ID", Id) : person.Clone();
        }

        public IEnumerable<Person> GetPersonByName(string fullName)
        {
            var persons = from item in DataSource.persons
                          let FullName = item.FirstName + " " + item.LastName
                          where FullName == fullName
                          select item.Clone();
            return persons.Count() > 0 ? persons : throw new MissingException("person", fullName);
        }

        public void AddPerson(Person person)
        {
            if (DataSource.persons.Any(x => person.Id == x.Id))
                throw new DuplicateException("Person ID", person.Id);
            DataSource.persons.Add(person.Clone());
        }

        public void UpdatePerson(Person person)
        {
            int count = DataSource.persons.RemoveAll(x => x.Id == person.Id);

            if (count == 0)
                throw new MissingException("Person ID", person.Id);
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
                throw new MissingException("Person ID", id);
        }


        public GuestRequest GetRequest(uint Key)
        {
            GuestRequest request = DataSource.guestRequests.FirstOrDefault(x => Key == x.Key);
            return request == null ?
                throw new MissingException("Guest Request Key", Convert.ToString(Key)) : request.Clone();
        }

        public IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId)
        {
            var requsts = from item in DataSource.guestRequests
                          where item.ClientId == ClientId
                          select item.Clone();
            if (requsts.Count() == 0)
                throw new MissingException("Booking requsts of Client ID", ClientId);
            return requsts;
        }

        public uint AddGuestRequest(GuestRequest request)
        {
            if (request.Key == 0)
            request.Key = Configuration.GuestRequestserialKey++;
            if (DataSource.guestRequests.Any(x => request.Key == x.Key)) // if there is a problem white the serialNumber
                throw new DuplicateException("Guest Request Key", Convert.ToString(request.Key));
            DataSource.guestRequests.Add(request.Clone());
            return request.Key;
        }

        public void UpdateGuestRequest(GuestRequest request)
        {
            int count = DataSource.guestRequests.RemoveAll(x => x.Key == request.Key);
            if (count == 0)
                throw new MissingException("Request Key", Convert.ToString(request.Key));
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
                throw new MissingException("Guest Request Key", Convert.ToString(Key));
        }


        public Host GetHost(string Id)
        {
            Host host = DataSource.hosts.FirstOrDefault(x => x.Id == Id);
            return host == null ? throw new MissingException("Host ID", Id) : host.Clone();
        }

        public void AddHost(Host host)
        {
            if (DataSource.hosts.Any(x => host.Id == x.Id))
                throw new DuplicateException("Host ID number", host.Id);
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
                throw new MissingException("Host ID", Id);
        }

        public void UpdateHost(Host host)
        {
            int count = DataSource.hosts.RemoveAll(x => x.Id == host.Id);
            if (count == 0)
                throw new MissingException("Host ID", host.Id);
            DataSource.hosts.Add(host.Clone());
        }

        public void UpdateStatusHost(string id, Status status)
        {
            bool found = false;
            foreach (Host item in DataSource.hosts)
            {
                if (item.Id == id)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingException("Host ID", id);
        }


        public HostingUnit GetUnit(uint Key)
        {
            HostingUnit host = DataSource.hostingUnits.FirstOrDefault(x => x.Key == Key);
            return host == null ? throw new MissingException("unit Key", Convert.ToString(Key)) : host.Clone();
        }

        public uint AddHostingUnit(HostingUnit unit)
        {
            if (unit.Key == 0)
                unit.Key = Configuration.HostingUnitSerialKey++;
            if (DataSource.hostingUnits.Any(x => unit.Key == x.Key))
                throw new DuplicateException("Unit Key", Convert.ToString(unit.Key));
            DataSource.hostingUnits.Add(unit.Clone());
            return unit.Key;
        }

        public void UpdateHostingUnit(HostingUnit unit)
        {
            int count = DataSource.hostingUnits.RemoveAll(x => x.Key == unit.Key);
            if (count == 0)
                throw new MissingException("Unit Key", Convert.ToString(unit.Key));
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
                throw new MissingException("hosting Unit Key", Convert.ToString(Key));
        }


        public Order GetOrder(uint Key)
        {
            Order order = DataSource.orders.FirstOrDefault(x => x.Key == Key);
            return order == null ? throw new MissingException("Order Key", Convert.ToString(Key)) : order.Clone();
        }

        public Order GetOrderGuestRequestKey(uint GuestRequestKey)
        {
            Order order = DataSource.orders.FirstOrDefault(x => x.GuestRequestKey == GuestRequestKey);
            return order == null ? throw new MissingException("No order with guest request Key",
                Convert.ToString(GuestRequestKey)) : order.Clone();
        }

        public IEnumerable<Order> GetOrdersHostingUnitKey(uint HostingUnitKey)
        {
            var orders = from item in DataSource.orders
                         where item.HostingUnitKey == HostingUnitKey
                         select item.Clone();
            return orders.Count() > 0 ? orders : throw new MissingException("orders for this unit Key",
                Convert.ToString(HostingUnitKey));
        }

        public uint AddOrder(Order odr)
        {
            if (odr.Key == 0)
            odr.Key = Configuration.OrderSerialKey++;
            if (DataSource.orders.Any(x => odr.Key == x.Key))
                throw new DuplicateException("Order Key", Convert.ToString(odr.Key));
            DataSource.orders.Add(odr.Clone());
            return odr.Key;
        }

        public void UpdateStatusOrder(uint Key, OrderStatus status, int numDays)
        {
            bool found = false;
            foreach (Order item in DataSource.orders)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = status;
                    if (item.Status == OrderStatus.APPROVED ||
                        item.Status == OrderStatus.NO_CLIENT_RESPONSE ||
                        item.Status == OrderStatus.UNIT_NOT_AVALABELE)
                        item.CloseDate = DateTime.Now;
                    if (item.Status == OrderStatus.APPROVED)
                        item.Fee = 10 * numDays;
                }
            }
            if (!found)
                throw new MissingException("Order Key", Convert.ToString(Key));
        }


        public IEnumerable<Order> GetOrders(Func<Order, bool> predicate)
        {
            var orders = from item in DataSource.orders
                         where predicate(item)
                         select item.Clone();
            return orders == null ? throw new MissingException("Orders") : orders;
        }

        public IEnumerable<GuestRequest> GetGuestRequests()
        {
            var requests = from item in DataSource.guestRequests
                           select item.Clone();
            return requests == null ? throw new MissingException("Guest Requests") : requests;
        }

        public IEnumerable<BankBranch> GetBranches()
        {
            var branches = from item in DataSource.bankBranches
                           select item.Clone();
            return branches == null ? throw new MissingException("Bank Branches") : branches;
        }

        public IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit, bool> predicate)
        {
            var units = from item in DataSource.hostingUnits
                        where predicate(item)
                        select item.Clone();
            return units == null ? throw new MissingException("Units") : units;
        }
    }
}
