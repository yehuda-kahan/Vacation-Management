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
        // loghic func
        bool CheckLegalDates(GuestRequest request);
        bool CheckHostClearance(Host host);
        bool CheckUnitAvilabilty(HostingUnit unit, GuestRequest request);
        bool CheckIfOrderClosed(Order order);


        int MarkDaysOfUnit(Order order);

        // our additions 
        void UpdStatusOrder(uint OrderKey, OrderStatus status);
        void CancelOrdersOfRequest(uint guestRequestKey, uint orderKey);
        void CancelUnitOrders(Order odr);
    }
}
