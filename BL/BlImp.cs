using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DalApi;
using DO;
using BO;

namespace BL
{
    class BlImp : IBl
    {
        readonly IDal dal = DalFactory.GetDal();

        #region Check functions
        public bool CheckLegalDates(GuestRequest request)
        {
            return request.LeaveDate > request.EntryDate;
        }

        public bool CheckHostClearance(Host host)
        {
            return host.CollectingClearance;
        }

        public bool CheckUnitAvilabilty(HostingUnit unit, DateTime entryDate, DateTime leaveDate)
        {
            for (; entryDate < leaveDate; entryDate = entryDate.AddDays(1))
            {
                if (unit[entryDate])
                    return false;
            }
            return true;
        }

        public bool CheckOrderClosed(Order order)
        {
            return order.Status == OrderStatus.APPROVED;
        }

        public bool CheckOpenOrdersForUnit(uint unitKey)
        {
            var orders = dal.GetOrders(x => x.HostingUnitKey == unitKey &&
            (x.Status == OrderStatus.PROCESSING || x.Status == OrderStatus.MAIL_SENT));
            if (orders.Count() > 0)
                return true;
            return false;
        }

        public bool CheckOpenOrdersForHost(string id)
        {
            var orders = dal.GetOrders(x => x.HostId == id &&
            (x.Status == OrderStatus.PROCESSING || x.Status == OrderStatus.MAIL_SENT));
            if (orders.Count() > 0)
                return true;
            return false;
        }

        #endregion

        #region Person functions
        public Person GetPerson(string Id)
        {
            Person temp = null;
            try { temp = dal.GetPerson(Id); }
            catch (MissingException ex) { throw ex; }
            return temp;
        }

        public void AddPerson(Person person)
        {
            Person 
        }

        #endregion

        #region Guest Request functions
        public void AddRequest(GuestRequest request) // test
        {
            if (CheckLegalDates(request))
                dal.AddGuestRequest(request);
        }
        public IEnumerable<GuestRequest> GetGuestRequests(Func<GuestRequest, bool> predicate)
        {
            return dal.GetGuestRequests(predicate);
        }

        #endregion

        #region Order functions

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

        public void UpdStatusOrder(uint OrderKey, OrderStatus status)
        {
            Order order;
            int numDays = 0;
            try { order = dal.GetOrder(OrderKey); }
            catch (MissingException ex) { throw ex; }
            if (CheckOrderClosed(order))
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

        public IEnumerable<Order> GetOdrsCreatedBigerFromNumDays(int numDays)
        {
            return dal.GetOrders(x => (DateTime.Now - x.OrderDate).Days >= numDays);
        }

        public int NumOfApprovedOrdersForUnit(HostingUnit unit)
        {
            return dal.GetOrders(x => x.HostingUnitKey == unit.Key && x.Status == OrderStatus.APPROVED).Count();
        }

        public int NumOfOrdersForRequst(GuestRequest request)
        {
            return dal.GetOrders(x => x.GuestRequestKey == request.Key).Count();
        }
        #endregion

        #region Host functions

        #endregion

        #region Units functions

        public IEnumerable<HostingUnit> GetAvalableUnits(DateTime entryDate, uint days)
        {
            var units = dal.GetHostingUnits(x => x != null);
            return from item in units
                   where CheckUnitAvilabilty(item, entryDate, entryDate.AddDays(days))
                   select item;
        }
        public int MarkDaysOfUnit(Order order)
        {
            HostingUnit unit;
            try { unit = dal.GetUnit(order.HostingUnitKey); }
            catch (MissingException ex) { throw ex; }
            GuestRequest request;
            try { request = dal.GetRequest(order.GuestRequestKey); }
            catch (MissingException ex) { throw ex; }

            if (!CheckUnitAvilabilty(unit, request.EntryDate, request.LeaveDate))
                return -1;

            DateTime entryDate = request.EntryDate;
            for (; entryDate < request.LeaveDate; entryDate = entryDate.AddDays(1))
            {
                unit[entryDate] = true;
            }
            dal.UpdateHostingUnit(unit); // update the "real" unit in the DataBase
            return (request.LeaveDate - request.EntryDate).Days;
        }
        #endregion

        #region manage functions

        public double GetHostFee(string id)
        {
            double sum = 0;
            var orders = dal.GetOrders(x => x.HostId == id);
            foreach (Order item in orders)
                sum += item.Fee;
            return sum;
        }

        public double GetAllFee()
        {
            double sum = 0;
            var orders = dal.GetOrders(x => x != null);
            foreach (Order item in orders)
                sum += item.Fee;
            return sum;
        }

        #endregion

        #region system fuctions

        public int DaysBetweenDates(DateTime first, DateTime last)
        {
            if (last.Year == 1) // The defult value
                last = DateTime.Now;
            return (first - last).Days;
        }
        public void SendMail()
        {
            Console.WriteLine("Mail send\n");
        }

        #endregion

    }
}

