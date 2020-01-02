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

        public bool MarkDaysOfUnit(Order order)
        {
            HostingUnit unit;
            try { unit = dal.GetUnit(order.HostingUnitKey); }
            catch (MissingException ex) { throw ex; }
            GuestRequest request;
            try { request = dal.GetRequest(order.GuestRequestKey); }
            catch (MissingException ex) { throw ex; }

            if (!CheckUnitAvilabilty(unit, request))
                throw new Exception("The unit are not avaleble at this dates");

            DateTime entryDate = request.EntryDate;
            for (; entryDate < request.LeaveDate; entryDate = entryDate.AddDays(1))
            {
                unit[entryDate] = true;
            }
            dal.UpdateHostingUnit(unit); // update the "real" unit in the DataBase
            return true;
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
            try { order = dal.GetOrder(OrderKey); }
            catch (MissingException ex) { throw ex; }
            if (CheckIfOrderClosed(order))
                throw new Exception("Order status canot be changed after approved");

            if (status == OrderStatus.APPROVED) // we want to make a deal
                if (!MarkDaysOfUnit(order))
                {
                    dal.UpdateStatusOrder(OrderKey, OrderStatus.UNIT_NOT_AVALABELE);
                    throw new Exception("The unit is not abalable");
                }
            dal.UpdateStatusOrder(OrderKey, status);
        }
    }
}
