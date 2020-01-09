using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DalApi;
using DO;
using BO;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace BL
{
    class BlImp : IBl
    {
        readonly IDal dal = DalFactory.GetDal();

        #region Check functions
        public bool CheckValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        public bool CheckLegalDates(GuestRequestBO request)
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
                if (unit.GetDates(entryDate))
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
            if (orders.Any())
                return true;
            return false;
        }

        #endregion

        #region Client functions
        public void AddClient(ClientBO client)
        {
            try { AddPerson(client.PersonalInfo); }
            catch (DuplicateException ex) { throw ex; }

            foreach (GuestRequestBO item in client.ClientRequests)
            {
                try { AddRequest(item); }
                catch (DuplicateException duplicateEx) { throw duplicateEx; }
                catch (FormatException formatEx) { throw formatEx; }
            }
        }

        public ClientBO GetClient(string id)
        {
            ClientBO client = new ClientBO();
            try { client.PersonalInfo = GetPerson(id); }
            catch (MissingException ex) { throw ex; }
            client.ClientRequests = GetGuestRequests(x => x.ClientId == id);
            return client;
        }
        #endregion

        #region Bank Branch

        public BankBranch ConverntBankBranchBOToDO(BankBranchBO branch)
        {
            BankBranch target = new BankBranch();
            target.BankNumber = branch.BankNumber;
            target.BankName = branch.BankName;
            target.BranchAddress = branch.BranchAddress;
            target.BranchCity = branch.BranchCity;
            target.BranchNumber = branch.BranchNumber;
            target.Status = (Status)branch.Status;
            return target;
        }

        public BankBranchBO ConverntBankBranchDOToBO(BankBranch branch)
        {
            BankBranchBO target = new BankBranchBO();
            target.BankNumber = branch.BankNumber;
            target.BankName = branch.BankName;
            target.BranchAddress = branch.BranchAddress;
            target.BranchCity = branch.BranchCity;
            target.BranchNumber = branch.BranchNumber;
            target.Status = (StatusBO)branch.Status;
            return target;
        }

        //public BankBranchBO GetBranch()

        #endregion

        #region Person functions

        PersonBO PersonConvertDOToBO(Person person)
        {
            PersonBO temp = new PersonBO();
            temp.Id = person.Id;
            temp.IdType = (IdTypesBO)person.IdType;
            temp.FirstName = person.FirstName;
            temp.LastName = person.LastName;
            temp.Email = person.Email;
            temp.Password = person.Password;
            temp.PhoneNomber = person.PhoneNomber;
            temp.Status = (StatusBO)person.Status;
            return temp;
        }

        Person PersonConvertBOToDO(PersonBO person)
        {
            Person temp = new Person();
            temp.Id = person.Id;
            temp.IdType = (IdTypes)person.IdType;
            temp.FirstName = person.FirstName;
            temp.LastName = person.LastName;
            temp.Email = person.Email;
            temp.Password = person.Password;
            temp.PhoneNomber = person.PhoneNomber;
            temp.Status = (Status)person.Status;
            return temp;
        }

        public PersonBO GetPerson(string Id)
        {
            Person temp = null;
            try { temp = dal.GetPerson(Id); }
            catch (MissingException ex) { throw ex; }
            return PersonConvertDOToBO(temp);
        }

        public void AddPerson(PersonBO person)
        {
            // TODO check id , 
            try { dal.AddPerson(PersonConvertBOToDO(person)); }
            catch (DuplicateException ex) { throw ex; }
        }

        public void UpdPerson(PersonBO person)
        {
            if (!CheckValidEmail(person.Email))
                throw new FormatException("The given mail " + person.Email + " is invalid");
            try { dal.UpdatePerson(PersonConvertBOToDO(person)); }
            catch (MissingException ex) { throw ex; }
        }

        public void UpdStatusPerson(string id, StatusBO status)
        {
            try { dal.UpdateStatusPerson(id, (Status)status); }
            catch (MissingException ex) { throw ex; }
        }

        #endregion

        #region Guest Request functions

        public GuestRequestBO GuestRequesConvertDOToBO(GuestRequest request)
        {
            GuestRequestBO temp = new GuestRequestBO();
            temp.Key = request.Key;
            temp.ClientId = request.ClientId;
            temp.Type = (UnitTypeBO)request.Type;
            temp.Status = (RequestStatusBO)request.Status;
            temp.CreateDate = request.CreateDate;
            temp.EntryDate = request.EntryDate;
            temp.LeaveDate = request.LeaveDate;
            temp.Adults = request.Adults;
            temp.Children = request.Children;
            temp.Area = (AreaLocationBO)request.Area;
            temp.ChildrensAttractions = (ThreeOptionsBO)request.ChildrensAttractions;
            temp.Garden = (ThreeOptionsBO)request.Garden;
            temp.Jacuzzi = (ThreeOptionsBO)request.Jacuzzi;
            temp.Pool = (ThreeOptionsBO)request.Pool;
            return temp;
        }

        public GuestRequest GuestRequesConvertBOToDO(GuestRequestBO request)
        {
            GuestRequest temp = new GuestRequest();
            temp.Key = request.Key;
            temp.ClientId = request.ClientId;
            temp.Type = (UnitType)request.Type;
            temp.Status = (RequestStatus)request.Status;
            temp.CreateDate = request.CreateDate;
            temp.EntryDate = request.EntryDate;
            temp.LeaveDate = request.LeaveDate;
            temp.Adults = request.Adults;
            temp.Children = request.Children;
            temp.Area = (AreaLocation)request.Area;
            temp.ChildrensAttractions = (ThreeOptions)request.ChildrensAttractions;
            temp.Garden = (ThreeOptions)request.Garden;
            temp.Jacuzzi = (ThreeOptions)request.Jacuzzi;
            temp.Pool = (ThreeOptions)request.Pool;
            return temp;
        }

        public void AddRequest(GuestRequestBO request)
        {
            if (CheckLegalDates(request))
            {
                try { dal.AddGuestRequest(GuestRequesConvertBOToDO(request)); }
                catch (DuplicateException ex) { throw ex; }
            }
            else
                throw new FormatException("Leave date must be bigger from entry date");
        }

        public GuestRequestBO GetRequest(uint key)
        {
            GuestRequest temp = null;
            try { temp = dal.GetRequest(key); }
            catch (MissingException ex) { throw ex; }
            return GuestRequesConvertDOToBO(temp);
        }

        public void UpdRequest(GuestRequestBO request)
        {
            if (CheckLegalDates(request))
            {
                try { dal.UpdateGuestRequest(GuestRequesConvertBOToDO(request)); }
                catch (MissingException ex) { throw ex; }
            }
            else
                throw new FormatException("Leave date must be bigger from entry date");
        }

        public void UpdStatusRequest(uint key, RequestStatusBO status)
        {
            try { dal.UpdateStatusRequest(key, (RequestStatus)status); }
            catch (MissingException ex) { throw ex; }
        }

        /// <summary>
        /// Help function
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<GuestRequestBO> GetGuestRequests(Func<GuestRequest, bool> predicate)
        {
            IEnumerable<GuestRequest> sourceRequest = dal.GetGuestRequests(predicate);
            return from item in sourceRequest
                   select GuestRequesConvertDOToBO(item);
        }

        #endregion

        #region Order functions

        public OrderBO ConvertOrderDOToBO(Order order)
        {
            OrderBO target = new OrderBO();
            target.Key = order.Key;
            target.Fee = order.Fee;
            target.HostingUnit = ConvertHostingUnitDOToBO(dal.GetUnit(order.HostingUnitKey));
            target.GuestRequest = GuestRequesConvertDOToBO(dal.GetRequest(order.GuestRequestKey));
            target.ClientFirstName = dal.GetPerson(target.GuestRequest.ClientId).FirstName;
            target.ClientLastName = dal.GetPerson(target.GuestRequest.ClientId).LastName;
            target.CloseDate = order.CloseDate;
            target.HostId = order.HostId;
            target.OrderDate = order.OrderDate;
            target.SentDate = order.SentDate;
            target.Status = (OrderStatusBO)order.Status;
            return target;
        }

        public Order ConvertOrderBOToDO(OrderBO order)
        {
            Order target = new Order();
            target.Key = order.Key;
            target.Fee = order.Fee;
            target.GuestRequestKey = order.GuestRequest.Key;
            target.HostingUnitKey = order.HostingUnit.Key;
            target.CloseDate = order.CloseDate;
            target.HostId = order.HostId;
            target.OrderDate = order.OrderDate;
            target.SentDate = order.SentDate;
            target.Status = (OrderStatus)order.Status;
            return target;
        }

        public OrderBO GetOrder(uint key)
        {
            OrderBO order = null;
            try { order = ConvertOrderDOToBO(dal.GetOrder(key)); }
            catch (MissingException ex) { throw ex; }
            return order;
        }

        public void AddOrder(OrderBO order)
        {
            try { dal.AddOrder(ConvertOrderBOToDO(order)); }
            catch (DuplicateException ex) { throw ex; }
        }

        public void CancelOrdersOfRequest(uint guestRequestKey, uint OrderKey)
        {
            var orders = dal.GetOrders(x => x.GuestRequestKey == guestRequestKey && x.Key != OrderKey);
            foreach (Order item in orders)
            {
                dal.UpdateStatusOrder(item.Key, OrderStatus.CANCELED);
            }
        }

        /// <summary>
        /// Help function 
        /// Cancels all orders which offerd for this unit, 
        /// which overlap with customer request dates for now captured dates
        /// </summary>
        /// <param name="odr">The Order which contained the captured unit and the details of the approved request</param>
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

        public void UpdStatusOrder(uint OrderKey, OrderStatusBO status)
        {
            Order order = null;
            int numDays = 0;
            try { order = dal.GetOrder(OrderKey); }
            catch (MissingException ex) { throw ex; }
            if (CheckOrderClosed(order))
                throw new Exception("Order status canot be changed after approved"); //TODO change exception type
            if (status == OrderStatusBO.APPROVED)
            {
                // we want to make a deal
                if ((numDays = MarkDaysOfUnit(ConvertOrderDOToBO(order))) == -1)
                {
                    dal.UpdateStatusOrder(OrderKey, OrderStatus.UNIT_NOT_AVALABELE);
                    throw new Exception("The unit is not abalable");
                }
                CancelOrdersOfRequest(order.GuestRequestKey, OrderKey);
                CancelUnitOrders(order);
                dal.UpdateStatusRequest(order.GuestRequestKey, RequestStatus.ORDERED);
                order.Fee = 10 * numDays;
                order.CloseDate = DateTime.Now;
                order.Status = (OrderStatus)status;
                dal.UpdOrder(order);
            }
            else
                dal.UpdateStatusOrder(OrderKey, (OrderStatus)status);
        }

        public IEnumerable<OrderBO> GetOdrsCreatedBigerFromNumDays(int numDays)
        {
            IEnumerable<Order> temp = dal.GetOrders(x => (DateTime.Now - x.OrderDate).Days >= numDays);
            return from item in temp
                   select ConvertOrderDOToBO(item);
        }

        public int NumOfApprovedOrdersForUnit(HostingUnitBO unit)
        {
            return dal.GetOrders(x => x.HostingUnitKey == unit.Key && x.Status == OrderStatus.APPROVED).Count();
        }

        public int NumOfOrdersForRequst(GuestRequestBO request)
        {
            return dal.GetOrders(x => x.GuestRequestKey == request.Key).Count();
        }
        #endregion

        #region Host functions

        #endregion

        #region Units functions

        HostingUnitBO ConvertHostingUnitDOToBO(HostingUnit unit)
        {
            HostingUnitBO target = new HostingUnitBO();
            target.Diary = unit.Diary;
            target.HostingUnitName = unit.HostingUnitName;
            target.Key = unit.Key;
            target.Owner = unit.Owner;
            target.Status = (StatusBO)unit.Status;
            return target;
        }

        HostingUnit ConvertHostingUnitBOToDO(HostingUnitBO unit)
        {
            HostingUnit target = new HostingUnit();
            target.Diary = unit.Diary;
            target.HostingUnitName = unit.HostingUnitName;
            target.Key = unit.Key;
            target.Owner = unit.Owner;
            target.Status = (Status)unit.Status;
            return target;
        }

        public HostingUnitBO GetUnit(uint key)
        {
            HostingUnitBO temp = null;
            try { temp = ConvertHostingUnitDOToBO(dal.GetUnit(key)); }
            catch (MissingException ex) { throw ex; }
            return temp;
        }

        public void AddUnit(HostingUnitBO unit)
        {
            try { dal.AddHostingUnit(ConvertHostingUnitBOToDO(unit)); }
            catch (DuplicateException ex) { throw ex; }
        }

        public void UpdUnit(HostingUnitBO unit)
        {
            try { dal.UpdateHostingUnit(ConvertHostingUnitBOToDO(unit)); }
            catch (MissingException ex) { throw ex; }
        }

        public IEnumerable<HostingUnitBO> GetAvalableUnits(DateTime entryDate, uint days)
        {
            var units = dal.GetHostingUnits(x => x != null);
            return from item in units
                   where CheckUnitAvilabilty(item, entryDate, entryDate.AddDays(days))
                   select ConvertHostingUnitDOToBO(item);
        }

        public int MarkDaysOfUnit(OrderBO order)
        {

            if (!CheckUnitAvilabilty(ConvertHostingUnitBOToDO(order.HostingUnit), order.GuestRequest.EntryDate, order.GuestRequest.LeaveDate))
                return -1;

            DateTime entryDate = order.GuestRequest.EntryDate;
            for (; entryDate < order.GuestRequest.LeaveDate; entryDate = entryDate.AddDays(1))
            {
                order.HostingUnit.SetDates(entryDate);
            }
            dal.UpdateHostingUnit(ConvertHostingUnitBOToDO(order.HostingUnit)); // update the "real" unit in the DataBase
            return (order.GuestRequest.LeaveDate - order.GuestRequest.EntryDate).Days;
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

