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
        #region Person
        Person GetPerson(string Id);
        IEnumerable<Person> GetPersonByName(string name);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void UpdateStatusPerson(string id, Status status);
        #endregion

        #region Guest Request
        GuestRequest GetRequest(uint Key);
        IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId);
        uint AddGuestRequest(GuestRequest request);
        void UpdateGuestRequest(GuestRequest request);
        void UpdateStatusRequest(uint Key, RequestStatus status);
        #endregion

        #region Host
        Host GetHost(string Id);
        void AddHost(Host host);
        void UpdateHost(Host host);
        void DelHost(string Key);
        void UpdateStatusHost(string id, Status status);
        #endregion

        #region Units
        HostingUnit GetUnit(uint Key);
        uint AddHostingUnit(HostingUnit unit);
        void UpdateHostingUnit(HostingUnit unit);
        void DelHostingUnit(uint Key);

        #endregion

        #region Order
        Order GetOrder(uint Key);
        IEnumerable<Order> GetOrdersHostingUnitKey(uint HostingUnitKey);
        Order GetOrderGuestRequestKey(uint GuestRequestKey);
        uint AddOrder(Order odr);

        /// <summary>
        /// update the status for a order.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="status"></param>
        /// <param name="numDays">this is the request vication number days ,
        /// Used in case the order is approved to calculate the comision</param>
        void UpdateStatusOrder(uint Key, OrderStatus status, int numDays = 0);
        #endregion

        #region Get Lists

        /// <summary>
        /// Get colection of units according a specific predicat example : GetHostingUnits(x => x.Key == 123)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<HostingUnit> GetHostingUnits(Func<HostingUnit, bool> predicate);

        /// <summary>
        /// Get colection of Orders according a specific predicat 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<Order> GetOrders(Func<Order, bool> predicate);

        IEnumerable<GuestRequest> GetGuestRequests();

        /// <summary>
        /// Get colection of Requests according a specific predicat
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<GuestRequest> GetGuestRequests(Func<GuestRequest, bool> predicate);

        IEnumerable<BankBranch> GetBranches();
        #endregion
    }
}
