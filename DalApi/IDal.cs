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
        Person GetPerson(uint Key);
        Person GetPerson(string name);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void UpdateStatusPerson(uint Key,Status status);

        GuestRequest GetRequest(uint Key);
        GuestRequest GetRequest(string ClientId);
        uint AddGuestRequest(GuestRequest request);
        void UpdateGuestRequest(GuestRequest request);
        void UpdateStatusRequest(uint Key, RequestStatus status);

        Host GetHost(string Id);
        void AddHost(Host host);
        void UpdateHost(Host host);
        void DelHost(string Key);

        HostingUnit GetUnit(uint Key);
        HostingUnit GetUnit(string name);
        uint AddHostingUnit(HostingUnit unit);
        void UpdateHostingUnit(HostingUnit unit);
        void DelHostingUnit(uint Key);

        Order GetOrder(uint Key);
        Order GetOrdersHostingUnitKey(uint HostingUnitKey);
        Order GetOrdersGuestRequestKey(uint GuestRequestKey);
        uint AddOrder(Order odr);
        void UpdateStatusOrder(uint Key, OrderStatus status);

        IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit,bool> predicate);
        IEnumerable<Order> GetOrders(Func<HostingUnit, bool> predicate);
        IEnumerable<GuestRequest> GetGuestRequests();

        IEnumerable<BankBranch> GetBranches();
    }
}
