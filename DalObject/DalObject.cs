using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    sealed class DalObject : IDal
    {
        static readonly DalObject instance = new DalObject();
        static DalObject() { }
        DalObject() { }
        public static DalObject Instance { get { return instance; } }

        public void AddGuestRequest(GuestRequest request)
        {

        }

        public void AddHost(Host host)
        {
            throw new NotImplementedException();
        }

        public void AddHostingUnit(HostingUnit unit)
        {
            throw new NotImplementedException();
        }

        public void AddOrder(Order odr)
        {
            throw new NotImplementedException();
        }

        public void AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void DelHost(uint Key)
        {
            throw new NotImplementedException();
        }

        public void DelHostingUnit(uint Key)
        {
            throw new NotImplementedException();
        }

        public Host GetHost(uint Key)
        {
            throw new NotImplementedException();
        }

        public Host GetHost(string name)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(uint Key)
        {
            throw new NotImplementedException();
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
