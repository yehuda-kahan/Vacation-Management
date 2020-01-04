using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BlApi
{
    internal interface IBl
    {
      

        #region Guest Requst function
        IEnumerable<GuestRequest> GetGuestRequests(Func<GuestRequest, bool> predicate);
        #endregion

        #region Order 

        void UpdStatusOrder(uint OrderKey, OrderStatus status);
        void CancelOrdersOfRequest(uint guestRequestKey, uint orderKey);
        void CancelUnitOrders(Order odr);
        IEnumerable<Order> GetOdrsCreatedBigerFromNumDays(int numDays);
        int NumOfOrdersForRequst(GuestRequest request);
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
        void sendMail();
        int DaysBetweenDates(DateTime first, DateTime last = new DateTime());
        #endregion

    }
}
