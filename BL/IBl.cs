﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using BO;

namespace BlApi
{
    internal interface IBl
    {
        #region Cilent
        void AddClient();
        #endregion

        #region Person
        Person GetPerson(string Id);

        void AddPerson(PersonBO person);

        #endregion

        #region Guest Requst function
        IEnumerable<GuestRequest> GetGuestRequests(Func<GuestRequest, bool> predicate);
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
        bool CheckLegalDates(GuestRequest request);
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
