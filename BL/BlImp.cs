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
using System.Data.Linq;
using System.ComponentModel;
using System.Threading;

namespace BL
{
    class BlImp : IBl
    {
        readonly IDal dal = DalFactory.GetDal();

        #region Check functions
        public bool IsValidMail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        private bool IsNumeric(string NumStr)
        {
            long temp;
            return long.TryParse(NumStr, out temp);
        }

        public bool IsValidTZ(string TZ)
        {
            int sum = 0;
            int tmp = 0;
            int mult = 0;

            TZ = TZ.Trim();
            if (!IsNumeric(TZ))
            { return false; }
            if (TZ == null || TZ.Length == 0 || TZ.Length != 9)
            {
                return false;
            }

            mult = (TZ.Length % 2 == 0) ? 2 : 1;

            for (int i = 0; i < TZ.Length - 1; i++)
            {
                try
                {
                    tmp = Convert.ToInt32(TZ.Substring(i, 1)) * mult;

                    if (tmp > 9)
                    {
                        tmp = 1 + tmp - 10;
                    }

                    sum += tmp;
                    mult = (mult == 2) ? 1 : 2;
                }
                catch
                {
                    return false;
                }
            }

            sum += Convert.ToInt32(TZ.Substring(TZ.Length - 1, 1));

            if (sum > 0 && sum % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsLegalDates(GuestRequestBO request)
        {
            return request.LeaveDate > request.EntryDate;
        }

        private bool CheckHostClearance(Host host)
        {
            return host.CollectingClearance;
        }

        private bool CheckUnitAvilabilty(HostingUnit unit, DateTime entryDate, DateTime leaveDate)
        {
            for (; entryDate < leaveDate; entryDate = entryDate.AddDays(1))
            {
                if (unit.GetDates(entryDate))
                    return false;
            }
            return true;
        }

        private bool CheckOrderClosed(Order order)
        {
            return order.Status == OrderStatus.APPROVED;
        }

        private bool CheckOpenOrdersForUnit(uint unitKey)
        {
            var orders = dal.GetOrders(x => x.HostingUnitKey == unitKey &&
            (x.Status == OrderStatus.PROCESSING || x.Status == OrderStatus.MAIL_SENT));
            if (orders.Count() > 0)
                return true;
            return false;
        } //TODO check necessarity

        private bool CheckOpenOrdersForHost(string id)
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
            catch (DuplicateKeyException ex) { throw ex; }

            foreach (GuestRequestBO item in client.ClientRequests)
            {
                try { AddRequest(item); }
                catch (DuplicateKeyException duplicateEx) { throw duplicateEx; }
                catch (FormatException formatEx) { throw formatEx; }
            }
        }

        public ClientBO GetClient(string id)
        {
            ClientBO client = new ClientBO();
            try { client.PersonalInfo = GetPersonById(id); }
            catch (MissingMemberException ex) { throw ex; }
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

        public void downloadBankXml()
        {
            dal.CreateXMLBankFiles();
        }

        public Dictionary<int, string> getBanknameList()
        {
            return dal.BuildDictionaryBankName();
        }

        public Dictionary<int, string> GetBranchesListForBank(int BankNum)
        {
            return dal.buildDictioneryBanches(BankNum);
        }

        public BankBranchBO GetBranch(uint bankNum, uint branchNum)
        {
            return ConverntBankBranchDOToBO(dal.GetBranch(bankNum, branchNum));
        }

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

        public PersonBO GetPersonById(string Id)
        {
            Person temp = null;
            try { temp = dal.GetPersonById(Id); }
            catch (MissingMemberException ex) { throw ex; }
            return PersonConvertDOToBO(temp);
        }

        public PersonBO GetPersonByMail(string mail)
        {
            PersonBO person = null;
            try { person = PersonConvertDOToBO(dal.GetPersonByMail(mail)); }
            catch (MissingMemberException ex) { throw ex; }
            return person;
        }

        public void AddPerson(PersonBO person)
        {
            if (!IsValidTZ(person.Id))
                new InvalidOperationException("The given Id is invalid");
            if (!dal.ChaeckPersonMail(person.Email))
            {
                try { dal.AddPerson(PersonConvertBOToDO(person)); }
                catch (DuplicateKeyException ex) { throw ex; }
            }
            else
                throw new DuplicateKeyException(person.Email, "The mail alredy is in the system by a othrer user\n");
        }

        public void UpdPerson(PersonBO person)
        {
            if (!IsValidMail(person.Email))
                throw new FormatException("The given mail " + person.Email + " is invalid");
            try { dal.UpdatePerson(PersonConvertBOToDO(person)); }
            catch (MissingMemberException ex) { throw ex; }
        }

        public void UpdStatusPerson(string id, StatusBO status)
        {
            try { dal.UpdateStatusPerson(id, (Status)status); }
            catch (MissingMemberException ex) { throw ex; }
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

        public uint AddRequest(GuestRequestBO request)
        {
            uint temp;
            if (IsLegalDates(request))
            {
                try { temp = dal.AddGuestRequest(GuestRequesConvertBOToDO(request)); return temp; }
                catch (DuplicateKeyException ex) { throw ex; }
            }
            else
                throw new FormatException("Leave date must be bigger from entry date");
        }

        public GuestRequestBO GetRequest(uint key)
        {
            GuestRequest temp = null;
            try { temp = dal.GetRequest(key); }
            catch (MissingMemberException ex) { throw ex; }
            return GuestRequesConvertDOToBO(temp);
        }

        public void UpdRequest(GuestRequestBO request)
        {
            if (IsLegalDates(request))
            {
                try { dal.UpdateGuestRequest(GuestRequesConvertBOToDO(request)); }
                catch (MissingMemberException ex) { throw ex; }
            }
            else
                throw new FormatException("Leave date must be bigger from entry date");
        }

        public void UpdStatusRequest(uint key, RequestStatusBO status)
        {
            try { dal.UpdateStatusRequest(key, (RequestStatus)status); }
            catch (MissingMemberException ex) { throw ex; }
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

        public IEnumerable<GuestRequestBO> GetGuestRequests()
        {
            IEnumerable<GuestRequest> sourceRequest = dal.GetGuestRequests();
            return from item in sourceRequest
                   select GuestRequesConvertDOToBO(item);
        }

        public IEnumerable<GuestRequestBO> GetRequestsCreatedBigerFromNumDays(int numDays)
        {
            IEnumerable<GuestRequest> temp = dal.GetGuestRequests(x => (DateTime.Now - x.CreateDate).Days >= numDays);
            return from item in temp
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
            target.ClientFirstName = dal.GetPersonById(target.GuestRequest.ClientId).FirstName;
            target.ClientLastName = dal.GetPersonById(target.GuestRequest.ClientId).LastName;
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
            catch (MissingMemberException ex) { throw ex; }
            return order;
        }

        public uint AddOrder(OrderBO order)
        {
            uint resultKey = 0;
            try
            {
                dal.GetPersonById(order.GuestRequest.ClientId);
                try
                {
                    dal.GetUnit(order.HostingUnit.Key);
                    try { resultKey = dal.AddOrder(ConvertOrderBOToDO(order)); }
                    catch (DuplicateKeyException ex) { throw ex; }
                }
                catch (MissingMemberException ex) { throw new MissingMemberException("Canot add this order because ", ex.ToString()); }
            }
            catch (MissingMemberException ex) { throw new MissingMemberException("Canot add this order because ", ex.ToString()); }
            return resultKey;
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
            catch (MissingMemberException ex) { throw ex; }
            if (CheckOrderClosed(order))
                throw new InvalidOperationException("Order status canot be changed after approved");
            if (status == OrderStatusBO.APPROVED)
            {
                // we want to make a deal
                if ((numDays = MarkDaysOfUnit(ConvertOrderDOToBO(order))) == -1)
                {
                    dal.UpdateStatusOrder(OrderKey, OrderStatus.UNIT_NOT_AVALABELE);
                    throw new InvalidOperationException("The unit is not abalable");
                }
                CancelOrdersOfRequest(order.GuestRequestKey, OrderKey);
                CancelUnitOrders(order);
                dal.UpdateStatusRequest(order.GuestRequestKey, RequestStatus.ORDERED);
                order.Fee = numDays * dal.GetFeePercent();
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

        public IEnumerable<OrderBO> GetOdrsOfHost(string id)
        {
            IEnumerable<Order> temp = dal.GetOrders(x => x.HostId == id && x.Status != OrderStatus.CANCELED
            && x.Status != OrderStatus.UNIT_NOT_AVALABELE && x.Status != OrderStatus.APPROVED);
            return from item in temp
                   select ConvertOrderDOToBO(item);
        }

        public IEnumerable<OrderBO> GetAprrovedOdrsOfHost(string id)
        {
            IEnumerable<Order> temp = dal.GetOrders(x => x.HostId == id && x.Status == OrderStatus.APPROVED);
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
        HostBO ConvertHostDOToBO(Host host)
        {
            HostBO target = new HostBO();
            target.CollectingClearance = host.CollectingClearance;
            target.WebSite = host.WebSite;
            target.BankAccountNumber = host.BankAccountNumber;
            target.BankDetales = GetBranch(host.BankNumber, host.BranchNumber);
            target.PersonalInfo = GetPersonById(host.Id);
            target.UnitsHost = GetHostUnits(host.Id);
            target.OrdersHost = GetOdrsOfHost(host.Id);
            target.AppovedOrdersHost = GetAprrovedOdrsOfHost(host.Id);
            target.Status = (StatusBO)host.Status;
            return target;
        }

        Host ConvertHostBOToDO(HostBO host)
        {
            Host target = new Host();
            target.CollectingClearance = host.CollectingClearance;
            target.WebSite = host.WebSite;
            target.BankAccountNumber = host.BankAccountNumber;
            target.BankNumber = host.BankDetales.BankNumber;
            target.BranchNumber = host.BankDetales.BranchNumber;
            target.Id = host.PersonalInfo.Id;
            target.Status = (Status)host.Status;
            return target;
        }

        public HostBO GetHost(string id)
        {
            try { return ConvertHostDOToBO(dal.GetHost(id)); }
            catch(MissingMemberException ex) { throw ex; }
        }

        public void AddHost(HostBO host)
        {
            try { dal.AddHost(ConvertHostBOToDO(host)); }
            catch (DuplicateKeyException ex) { throw ex; }
        }

        public void UpdHost(HostBO host)
        {
            if (host.CollectingClearance == false)
            {
                var temp = from item in host.OrdersHost
                           where item.Status != OrderStatusBO.APPROVED
                           select item;
                if (temp.Any())
                    throw new TypeAccessException
                        ("אינך יכול לבטל את ההרשאה לחיוב בזמן שיש הזמנות פתוחות במערכת");
            }
            try { dal.UpdateHost(ConvertHostBOToDO(host)); }
            catch (MissingMemberException ex) { throw ex; }
        }

        public void DelHost(HostBO host)
        {
            if (host.CollectingClearance == false)
            {
                var temp = from item in host.OrdersHost
                           where item.Status != OrderStatusBO.APPROVED
                           select item;
                if (temp.Any())
                    throw new TypeAccessException
                        ("NOT allowed to delete Host while there is opened orders in the system");
            }
            try { dal.DelHost(host.PersonalInfo.Id); }
            catch (MissingMemberException ex) { throw ex; }
        }

        #endregion

        #region Units functions

        HostingUnitBO ConvertHostingUnitDOToBO(HostingUnit unit)
        {
            HostingUnitBO target = new HostingUnitBO();
            target.Area = (AreaLocationBO)unit.Area;
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
            target.Area = (AreaLocation)unit.Area;
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
            catch (MissingMemberException ex) { throw ex; }
            return temp;
        }

        public uint AddUnit(HostingUnitBO unit)
        {
            try { return dal.AddHostingUnit(ConvertHostingUnitBOToDO(unit)); }
            catch (DuplicateKeyException ex) { throw ex; }
        }

        public void UpdUnit(HostingUnitBO unit)
        {
            try { dal.UpdateHostingUnit(ConvertHostingUnitBOToDO(unit)); }
            catch (MissingMemberException ex) { throw ex; }
        }

        public IEnumerable<HostingUnitBO> GetAvalableUnits(DateTime entryDate, uint days)
        {
            var units = dal.GetHostingUnits(x => x != null);
            return from item in units
                   where CheckUnitAvilabilty(item, entryDate, entryDate.AddDays(days))
                   select ConvertHostingUnitDOToBO(item);
        }

        public IEnumerable<HostingUnitBO> GetHostUnits(string id)
        {
            var units = dal.GetHostingUnits(x => x.Owner == id);
            return from item in units
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

        public int GetConfigByName(string conf)
        {
            return dal.GetConfigByName(conf);
        }

        public void SetConfigByName(string ConfName, object Value)
        {
            try
            {
                dal.SetConfig(ConfName, Value);
            }
            catch (AccessViolationException ex) { throw ex; }
            catch (KeyNotFoundException ex) { throw ex; }
        }

        public double GetUpComingFee()
        {
            double sumDays = 0;
            var orders = dal.GetOrders(x => x.Status != OrderStatus.APPROVED && x.Status
            != OrderStatus.CANCELED && x.Status != OrderStatus.UNIT_NOT_AVALABELE && x.Status
            != OrderStatus.NO_CLIENT_RESPONSE);

            foreach (Order item in orders)
            {
                OrderBO temp = ConvertOrderDOToBO(item);
                sumDays += DaysBetweenDates(temp.GuestRequest.EntryDate, temp.GuestRequest.LeaveDate);
            }
            return sumDays * dal.GetFeePercent();
        }

        public string GetAdminUser()
        {
            return dal.GetAdministratorUser();
        }

        public string GetAdminPass()
        {
            return dal.GetAdministratorPass();
        }


        #endregion

        #region system fuctions

        public int DaysBetweenDates(DateTime first, DateTime last)
        {
            if (last.Year == 1) // The defult value
                last = DateTime.Now;
            return (last - first).Days;
        }


        public void SendMail(Email email)
        {
            bool flag = true;
            try { email.SendMail(); }
            catch (ArgumentNullException ex) { flag = false; }
            catch (InvalidOperationException ex) { flag = false; }
            catch (SmtpException ex) { flag = false; }
            if (!flag)
            {
                Thread.Sleep(5000);
                SendMail(email);
            }
        }

        public void DelExpierInvatationAndRequests()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (DateTime.Now.Hour == 0)//12 oclook am. 
                    {
                        IEnumerable<OrderBO> expieredOrd = GetOdrsCreatedBigerFromNumDays(dal.GetNumDaysToExpire());
                        IEnumerable<GuestRequestBO> expierRequests = GetRequestsCreatedBigerFromNumDays(dal.GetNumDaysToExpire());

                        foreach (OrderBO item in expieredOrd)
                        {
                            UpdStatusOrder(item.Key, OrderStatusBO.NO_CLIENT_RESPONSE);
                        }
                        foreach (GuestRequestBO item in expierRequests)
                        {
                            UpdStatusRequest(item.Key, RequestStatusBO.EXPIRED);
                        }
                    }
                    Thread.Sleep(3600000);
                }
            }
            ).Start();
        }
        #endregion

        #region Lists function
        public IEnumerable<IGrouping<AreaLocationBO, GuestRequestBO>> GetRequestByArea()
        {
            return from item in dal.GetGuestRequests()
                   select GuestRequesConvertDOToBO(item)
                         into tempResult
                   group tempResult by tempResult.Area;
        }

        public IEnumerable<IGrouping<AreaLocationBO, HostingUnitBO>> GetHostingUnitsByArea()
        {
            return from item in dal.GetHostingUnits(x => x.Status == Status.ACTIVE)
                   select ConvertHostingUnitDOToBO(item)
                         into tempResult
                   group tempResult by tempResult.Area;
        }

        public IEnumerable<IGrouping<uint, GuestRequestBO>> GetRequestByNumOfGuest()
        {
            return from item in dal.GetGuestRequests()
                   select GuestRequesConvertDOToBO(item)
                         into tempResult
                   group tempResult by tempResult.Adults + tempResult.Children;
        }

        public IEnumerable<IGrouping<int, HostBO>> GetHostsByNumOfUnits()
        {
            return from item in dal.GetHosts(x => x.Status == Status.ACTIVE)
                   select ConvertHostDOToBO(item)
                   into tempHosts
                   group tempHosts by tempHosts.UnitsHost.Count();
        }

        public IEnumerable<OrderBO> GetAppOrders()
        {
            IEnumerable<Order> orders = dal.GetOrders(x => x.Status == OrderStatus.APPROVED);
            return from item in orders
                   select ConvertOrderDOToBO(item);
        }

        public IEnumerable<HostBO> GetAllHosts()
        {
            return from item in dal.GetHosts(x => x != null)
                   select ConvertHostDOToBO(item);
        }

        public List<PersonBO> GetAllClients()
        {
            bool flag;
            List<PersonBO> clients = new List<PersonBO>();
            IEnumerable<HostBO> hosts = GetAllHosts();
            IEnumerable<Person> persons = dal.GetAllPersons();
            
            foreach (Person person in persons)
            {
                flag = true;
                foreach (HostBO host in hosts)
                {
                    if (person.Id == host.PersonalInfo.Id)
                        flag = false;
                }
                if (flag) // if he not a host
                {
                    clients.Add(PersonConvertDOToBO(person));
                }
            }
            return clients;
        }
        #endregion

    }
}

