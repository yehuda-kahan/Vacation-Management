using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    internal interface IBl
    {

        #region Person

        /// <summary>
        /// Exceptions : MissingException, FormatException
        /// </summary>
        /// <param name="person"></param>
        void UpdPerson(PersonBO person);

        /// <summary>
        /// Exceptions : DuplicateException
        /// </summary>
        /// <param name="person"></param>
        void AddPerson(PersonBO person);

        /// <summary>
        /// Exceptions : MissingException
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        PersonBO GetPerson(string Id);

        /// <summary>
        /// Exceptions : MissingException.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void UpdStatusPerson(string id, StatusBO status);
        #endregion

        #region Guest Requst function

        /// <summary>
        /// Exceptions : DuplicateException , FormatException
        /// </summary>
        /// <param name="request"></param>
        void AddRequest(GuestRequestBO request);

        /// <summary>
        /// Exceptions : MissingException
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        GuestRequestBO GetRequest(uint key);

        /// <summary>
        ///  Exceptions : MissingException , FormatException
        /// </summary>
        /// <param name="request"></param>
        void UpdRequest(GuestRequestBO request);

        /// <summary>
        ///  Exceptions : MissingException
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        void UpdStatusRequest(uint key, RequestStatusBO status);

        IEnumerable<GuestRequestBO> GetGuestRequests(Func<GuestRequestBO, bool> predicate);
        #endregion

        #region Client

        /// <summary>
        /// Exceptions : DuplicateException
        /// </summary>
        /// <param name="client"></param>
        void AddClient(ClientBO client);

        #endregion

        #region Order 

        void UpdStatusOrder(uint OrderKey, OrderStatus status);

        /// <summary>
        /// for canceled all the order with the given guestRequest Key
        /// but arent with the given Order Key
        /// </summary>
        /// <param name="guestRequestKey">for </param>
        /// <param name="OrderKey"></param>
        void CancelOrdersOfRequest(uint guestRequestKey, uint orderKey);

        /// <summary>
        /// Cancels all orders which offerd for this unit, 
        /// which overlap with customer request dates for now captured dates
        /// </summary>
        /// <param name="odr">The Order which contained the captured unit and the details of the approved request</param>
        void CancelUnitOrders(Order odr);

        /// <summary>
        /// Return the orders that the creation date is bigger from the given 
        /// number according to today
        /// </summary>
        /// <param name="numDays"></param>
        /// <returns></returns>
        IEnumerable<Order> GetOdrsCreatedBigerFromNumDays(int numDays);

        /// <summary>
        /// Return the number of orders that conect to the given request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int NumOfOrdersForRequst(GuestRequest request);

        /// <summary>
        ///  Return the number of approved orders that conect to the given unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        int NumOfApprovedOrdersForUnit(HostingUnit unit);

        #endregion

        #region Units function
        int MarkDaysOfUnit(Order order);
        IEnumerable<HostingUnit> GetAvalableUnits(DateTime entryDate, uint days);
        #endregion

        #region Manage
        double GetHostFee(string id);
        double GetAllFee();
        #endregion

        #region Check functions
        bool CheckValidEmail(string email);
        bool CheckLegalDates(GuestRequestBO request);
        bool CheckHostClearance(Host host);
        bool CheckUnitAvilabilty(HostingUnit unit, DateTime entryDate, DateTime leaveDate);
        bool CheckOrderClosed(Order order);
        bool CheckOpenOrdersForUnit(uint unitKey);
        bool CheckOpenOrdersForHost(string id);
        #endregion

        #region system functions
        void SendMail();

        /// <summary>
        /// Return the number of the days between a two given dates 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        int DaysBetweenDates(DateTime first, DateTime last = default);
        #endregion
    }
}
