using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBl
    {

        #region Person

        /// <summary>
        /// Exceptions : MissingMemberException, FormatException
        /// </summary>
        /// <param name="person"></param>
        void UpdPerson(PersonBO person);

        /// <summary>
        /// Exceptions : DuplicateKeyException , InvalidOperationException
        /// </summary>
        /// <param name="person"></param>
        void AddPerson(PersonBO person);

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        PersonBO GetPersonById(string Id);

        /// <summary>
        ///  Exceptions : MissingMemberException
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        PersonBO GetPersonByMail(string mail);

        /// <summary>
        /// Exceptions : MissingMemberException.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void UpdStatusPerson(string id, StatusBO status);
        #endregion

        #region Guest Requst function

        /// <summary>
        /// Exceptions : DuplicateKeyException , FormatException
        /// </summary>
        /// <param name="request"></param>
        uint AddRequest(GuestRequestBO request);

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        GuestRequestBO GetRequest(uint key);

        /// <summary>
        ///  Exceptions : MissingMemberException , FormatException
        /// </summary>
        /// <param name="request"></param>
        void UpdRequest(GuestRequestBO request);

        /// <summary>
        ///  Exceptions : MissingMemberException
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        void UpdStatusRequest(uint key, RequestStatusBO status);

      
        #endregion

        #region Client

        /// <summary>
        /// Exceptions : DuplicateKeyException , FormatException
        /// </summary>
        /// <param name="client"></param>
        void AddClient(ClientBO client);

        /// <summary>
        ///  Exceptions : MissingMemberException
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClientBO GetClient(string id);


        #endregion

        #region Bank Branch

        void downloadBankXml();

        Dictionary<int, string> getBanknameList();

        Dictionary<int, string> GetBranchesListForBank(int BankNum);

        #endregion

        #region Order 

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        OrderBO GetOrder(uint key);

        /// <summary>
        /// Exceptions : DuplicateKeyException
        /// </summary>
        /// <param name="order"></param>
        uint AddOrder(OrderBO order);

        /// <summary>
        /// Exceptions : InvalidOperationException, MissingMemberException
        /// </summary>
        /// <param name="OrderKey"></param>
        /// <param name="status"></param>
        void UpdStatusOrder(uint OrderKey, OrderStatusBO status);

        /// <summary>
        /// for canceled all the order with the given guestRequest Key
        /// but arent with the given Order Key
        /// </summary>
        /// <param name="guestRequestKey">for </param>
        /// <param name="OrderKey"></param>
        void CancelOrdersOfRequest(uint guestRequestKey, uint orderKey);


        /// <summary>
        /// Return the orders that the creation date is bigger from the given 
        /// number according to today
        /// </summary>
        /// <param name="numDays"></param>
        /// <returns></returns>
        IEnumerable<OrderBO> GetOdrsCreatedBigerFromNumDays(int numDays);

        /// <summary>
        /// Return the number of orders that conect to the given request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int NumOfOrdersForRequst(GuestRequestBO request);

        /// <summary>
        ///  Return the number of approved orders that conect to the given unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        int NumOfApprovedOrdersForUnit(HostingUnitBO unit);

        IEnumerable<OrderBO> GetOdrsOfHost(string id);

        #endregion

        #region Units function

        /// <summary>
        /// Exceptions : MissingMemberException 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        HostingUnitBO GetUnit(uint key);

        /// <summary>
        ///  Exceptions : DuplicateKeyException
        /// </summary>
        /// <param name="unit"></param>
        uint AddUnit(HostingUnitBO unit);

        /// <summary>
        ///  Exceptions : MissingMemberException 
        /// </summary>
        /// <param name="unit"></param>
        void UpdUnit(HostingUnitBO unit);

        int MarkDaysOfUnit(OrderBO order);

        IEnumerable<HostingUnitBO> GetAvalableUnits(DateTime entryDate, uint days);

        IEnumerable<HostingUnitBO> GetHostUnits(string id);
        #endregion

        #region Host

        /// <summary>
        /// Exceptions : MissingMemberException
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HostBO GetHost(string id);

        /// <summary>
        /// Exceptions : DuplicatekeyException
        /// </summary>
        /// <param name="host"></param>
        void AddHost(HostBO host);

        void UpdHost(HostBO host);

        void DelHost(HostBO host);

        IEnumerable<OrderBO> GetAprrovedOdrsOfHost(string id);
        #endregion

        #region Manage
        double GetHostFee(string id);
        double GetAllFee();
        #endregion

        #region system functions
        void SendMail(Email email);

        /// <summary>
        /// Return the number of the days between a two given dates 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        int DaysBetweenDates(DateTime first, DateTime last = default);

        bool IsValidMail(string email);

        bool IsValidTZ(string TZ);
        #endregion

        #region List function

        IEnumerable<IGrouping<AreaLocationBO, GuestRequestBO>> GetRequestByArea();

        IEnumerable<IGrouping<uint, GuestRequestBO>> GetRequestByNumOfGuest();

        IEnumerable<IGrouping<int, HostBO>> GetHostsByNumOfUnits();

        IEnumerable<IGrouping<AreaLocationBO, HostingUnitBO>> GetHostingUnitsByArea();

        IEnumerable<GuestRequestBO> GetGuestRequests();

        IEnumerable<OrderBO> GetAppOrders();

        #endregion
    }
}
