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

        public uint AddGuestRequest(GuestRequest request)
        {
            request.Key = Configuration.GuestRequestserialKey++;
            if (DataSource.guestRequests.Any(x => request.Key == x.Key)) // if there is a problem white the serialNumber
                throw new DuplicateException("Guest Request Key", Convert.ToString(request.Key));
            DataSource.guestRequests.Add(request.Clone());
            return request.Key;
        }

        public void AddHost(Host host)
        {
            if (DataSource.hosts.Any(x => host.Id == x.Id))
                throw new DuplicateException("Host ID number", host.Id);
            DataSource.hosts.Add(host.Clone());
        }

        public uint AddHostingUnit(HostingUnit unit)
        {
            unit.Key = Configuration.HostingUnitSerialKey++;
            if (DataSource.hostingUnits.Any(x => unit.Key == x.Key))
                throw new DuplicateException("Unit Key", Convert.ToString(unit.Key));
            DataSource.hostingUnits.Add(unit.Clone());
            return unit.Key;
        }

        public uint AddOrder(Order odr)
        {
            odr.Key = Configuration.OrderSerialKey++;
            if (DataSource.orders.Any(x => odr.Key == x.Key))
                throw new DuplicateException("Order Key", Convert.ToString(odr.Key));
            DataSource.orders.Add(odr.Clone());
            return odr.Key;
        }

        public void AddPerson(Person person)
        {
            if (DataSource.persons.Any(x => person.Id == x.Id))
                throw new DuplicateException("Person ID", person.Id);
            DataSource.persons.Add(person.Clone());
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

        public Host GetHost(string Id)
        {
            Host host = DataSource.hosts.FirstOrDefault(x => x.Id == Id);
            return host == null ? null : host.Clone();
        }

     
        public Order GetOrder(uint Key)
        {
            Order order = DataSource.orders.FirstOrDefault(x => x.Key == Key);
            return order == null ? null : order.Clone();
        }


        public Order GetOrdersGuestRequestKey(uint GuestRequestKey)
        {
            throw new NotImplementedException();
        }

        public Order GetOrdersHostingUnitKey(uint HostingUnitKey)
        {
            throw new NotImplementedException();
        }

        public Person GetPerson(uint Key)
        {
            throw new NotImplementedException();
        }

        public Person GetPerson(string name)
        {
            throw new NotImplementedException();
        }

        public GuestRequest GetRequest(uint Key)
        {
            throw new NotImplementedException();
        }

        public GuestRequest GetRequest(string ClientId)
        {
            throw new NotImplementedException();
        }

        public HostingUnit GetUnit(uint Key)
        {
            throw new NotImplementedException();
        }

        public HostingUnit GetUnit(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateGuestRequest(GuestRequest request)
        {
            throw new NotImplementedException();
        }

        public void UpdateHost(Host host)
        {
            throw new NotImplementedException();
        }

        public void UpdateHostingUnit(HostingUnit unit)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusOrder(uint Key, OrderStatus status)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusPerson(uint Key, Status status)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusRequest(uint Key, RequestStatus status)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Order> IDal.GetOrders(Func<HostingUnit, bool> predicate)
        {
            throw new NotImplementedException();
        }

        IEnumerable<GuestRequest> IDal.GetGuestRequests()
        {
            throw new NotImplementedException();
        }

        IEnumerable<BankBranch> IDal.GetBranches()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
