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
        Dictionary<string, object> getConfig();
        void SetConfig(string parm, Object value);
        event Action<Dictionary<String, Object>> ConfigHandler;

        #region BankXml
        void CreateXMLBankFiles();

        BankBranch GetBranch(uint bankNum, uint branchNum);
        Dictionary<int, string> BuildDictionaryBankName();

        Dictionary<int, string> buildDictioneryBanches(int BankNum);
        #endregion

        #region Person

        bool ChaeckPersonMail(string mail);

        /// <summary>
        /// Exceptions: MissingMemberException
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>A person or throwing a MissingMemberException </returns>
        Person GetPersonById(string Id);
        IEnumerable<Person> GetPersonsByName(string name);

        Person GetPersonByMail(string Id);

        /// <summary>
        /// Exceptions: DuplicateKeyException
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        void AddPerson(Person person);

        /// <summary>
        /// Exceptions: MissingMemberException
        /// </summary>
        /// <param name="person"></param>
        void UpdatePerson(Person person);

        /// <summary>
        /// Exceptions: MissingMemberException
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void UpdateStatusPerson(string id, Status status);
        #endregion

        #region Guest Request

        /// <summary>
        /// Exceptions :  DuplicateKeyException
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        uint AddGuestRequest(GuestRequest request);

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        GuestRequest GetRequest(uint Key);

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId);

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="request"></param>
        void UpdateGuestRequest(GuestRequest request);

        void UpdateStatusRequest(uint Key, RequestStatus status);
        #endregion

        #region Host
        Host GetHost(string Id);
        /// <summary>
        /// Exceptions : DuplicateKeyException
        /// </summary>
        /// <param name="host"></param>
        void AddHost(Host host);
        void UpdateHost(Host host);
        void DelHost(string Key);
        #endregion

        #region Units
        HostingUnit GetUnit(uint Key);
        /// <summary>
        /// Exceptions : DuplicateKeyException
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        uint AddHostingUnit(HostingUnit unit);
        void UpdateHostingUnit(HostingUnit unit);
        void DelHostingUnit(uint Key);

        #endregion

        #region Order
        Order GetOrder(uint Key);
        IEnumerable<Order> GetOrdersHostingUnitKey(uint HostingUnitKey);
        Order GetOrderGuestRequestKey(uint GuestRequestKey);
        /// <summary>
        /// Exceptions : DuplicateKeyException
        /// </summary>
        /// <param name="odr"></param>
        /// <returns></returns>
        uint AddOrder(Order odr);

        void UpdOrder(Order odr);

        /// <summary>
        /// update the status for a order.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="status"></param>
        /// <param name="numDays">this is the request vication number days ,
        /// Used in case the order is approved to calculate the comision</param>
        void UpdateStatusOrder(uint Key, OrderStatus status);
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

        IEnumerable<Host> GetHosts(Func<Host, bool> predicate);
        #endregion

    }
}
