using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DalApi;
using DO;


namespace BL
{
    class BlImp : IBl
    {
        readonly IDal dal = DalFactory.GetDal();

        public bool CheckLegalDates(GuestRequest request)
        {
            return request.LeaveDate > request.EntryDate;
        }

        public bool CheckHostClearance(Host host)
        {
            return host.CollectingClearance;
        }

        public bool CheckUnitAvilabilty(HostingUnit unit, GuestRequest request)
        {
            DateTime entryDate = request.EntryDate;
            for (; entryDate < request.LeaveDate; entryDate = entryDate.AddDays(1))
            {
                if (unit[entryDate])
                    return false;
            }
            return true;
        }

        public int MarkDaysOfUnit(Order order)
        {
            HostingUnit unit;
            try { unit = dal.GetUnit(order.HostingUnitKey); }
            catch (MissingException ex) { throw ex; }
            GuestRequest request;
            try { request = dal.GetRequest(order.GuestRequestKey); }
            catch (MissingException ex) { throw ex; }

            if (!CheckUnitAvilabilty(unit, request))
                return -1;

            DateTime entryDate = request.EntryDate;
            for (; entryDate < request.LeaveDate; entryDate = entryDate.AddDays(1))
            {
                unit[entryDate] = true;
            }
            dal.UpdateHostingUnit(unit); // update the "real" unit in the DataBase
            return (request.LeaveDate - request.EntryDate).Days - 1;
        }

        public bool CheckIfOrderClosed(Order order)
        {
            return order.Status == OrderStatus.APPROVED;
        }

        public void AddRequest(GuestRequest request) // test
        {
            if (CheckLegalDates(request))
                dal.AddGuestRequest(request);
        }

        public void UpdStatusOrder(uint OrderKey, OrderStatus status) 
        {
            Order order;
            int numDays = 0;
            try { order = dal.GetOrder(OrderKey); }
            catch (MissingException ex) { throw ex; }
            if (CheckIfOrderClosed(order))
                throw new Exception("Order status canot be changed after approved");
            if (status == OrderStatus.APPROVED)
            {
                // we want to make a deal
                if ((numDays = MarkDaysOfUnit(order)) == -1)
                {
                    dal.UpdateStatusOrder(OrderKey, OrderStatus.UNIT_NOT_AVALABELE);
                    throw new Exception("The unit is not abalable");
                }
                CancelOrdersOfRequest(order.GuestRequestKey, OrderKey);
                CancelUnitOrders(order);
                dal.UpdateStatusRequest(order.GuestRequestKey, RequestStatus.ORDERED);
            }
            dal.UpdateStatusOrder(OrderKey, status, numDays);
        }

        /// <summary>
        /// for canceled all the order with the given guestRequest Key
        /// but arent with the given Order Key
        /// </summary>
        /// <param name="guestRequestKey">for </param>
        /// <param name="OrderKey"></param>
        public void CancelOrdersOfRequest(uint guestRequestKey, uint OrderKey)
        {
            var orders = dal.GetOrders(x => x.GuestRequestKey == guestRequestKey && x.Key != OrderKey);
            foreach (Order item in orders)
            {
                dal.UpdateStatusOrder(item.Key, OrderStatus.CANCELED);
            }
        }

        public void CancelUnitOrders(Order odr)
        {
            GuestRequest request = dal.GetRequest(odr.GuestRequestKey);
            var orders = dal.GetOrders(x => x.HostingUnitKey == odr.HostingUnitKey);
            foreach (Order item in orders)
            {
                GuestRequest temp = dal.GetRequest(item.GuestRequestKey);
                if ((temp.EntryDate >= request.EntryDate && temp.EntryDate <= request.LeaveDate && item.Key != odr.Key) ||
                    (temp.LeaveDate >= request.EntryDate && temp.LeaveDate <= request.LeaveDate && item.Key != odr.Key))
                    dal.UpdateStatusOrder(item.Key, OrderStatus.CANCELED);
            }
        }
    }
}

    