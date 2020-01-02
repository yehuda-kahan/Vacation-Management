using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        Person GetPerson(string Id);
        IEnumerable<Person> GetPersonByName(string name);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void UpdateStatusPerson(string id, Status status);

        GuestRequest GetRequest(uint Key);
        IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId);
        uint AddGuestRequest(GuestRequest request);
        void UpdateGuestRequest(GuestRequest request);
        void UpdateStatusRequest(uint Key, RequestStatus status);

        Host GetHost(string Id);
        void AddHost(Host host);
        void UpdateHost(Host host);
        void DelHost(string Key);
        void UpdateStatusHost(string id, Status status);

        HostingUnit GetUnit(uint Key);
        uint AddHostingUnit(HostingUnit unit);
        void UpdateHostingUnit(HostingUnit unit);
        void DelHostingUnit(uint Key);

        Order GetOrder(uint Key);
        IEnumerable<Order> GetOrdersHostingUnitKey(uint HostingUnitKey);
        Order GetOrderGuestRequestKey(uint GuestRequestKey);
        uint AddOrder(Order odr);
        void UpdateStatusOrder(uint Key, OrderStatus status,int numDays=0);

        IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit, bool> predicate);
        IEnumerable<Order> GetOrders(Func<Order, bool> predicate);
        IEnumerable<GuestRequest> GetGuestRequests();

        IEnumerable<BankBranch> GetBranches();
    }
}
