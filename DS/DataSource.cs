using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DS
{
    public static class DataSource
    {
        public static List<Person> persons;
        public static List<Host> hosts;
        public static List<HostingUnit> hostingUnits;
        public static List<Order> orders;
        public static List<GuestRequest> guestRequests;
        public static List<BankBranch> bankBranches;

        static DataSource()
        {
            persons = new List<Person>()
            {
                new Person
                {
                    Id ="11223344",
                    IdType = IdTypes.PASSPORT,
                    FirstName ="avi",
                    LastName ="ros",
                    PhoneNomber="+972502783644",
                    Email="avrumi2018@gmail.com",
                    Password="Aa212",
                    Status = Status.ACTIVE
                },
                new Person
                {
                     Id ="12345678",
                    IdType = IdTypes.DRIVING_LICENSE,
                    FirstName ="yehuda",
                    LastName ="kahan",
                    PhoneNomber="+972587271600",
                    Email="yehudajka@gmail.com",
                    Password="1234",
                    Status = Status.ACTIVE
                }
            };

            hosts = new List<Host>()
            {
                new Host
                {
                    Id="11223344",
                    BankNumber=10,
                    BranchNumber = 905,
                    BankAccountNumber= 57112658,
                    CollectingClearance= true,
                    Status = Status.ACTIVE,
                    WebSite = "www.google.com"
                },
            };

            hostingUnits = new List<HostingUnit>()
            {
                new HostingUnit
                {
                    Key=1111,
                    Owner="11223344",
                    HostingUnitName = "The villa",
                    Diary =new bool[12,31],
                    Status = Status.ACTIVE,
                    Area = AreaLocation.CENTER
                },
                new HostingUnit
                {
                    Key=1112,
                    Owner="11223344",
                    HostingUnitName = "The apartment",
                    Diary =new bool[12,31],
                    Status = Status.ACTIVE,
                    Area = AreaLocation.JERUSALEM
                }
            };

            orders = new List<Order>()
            {
                new Order
                {
                    Key =10000001,
                    HostingUnitKey = 1111,
                    GuestRequestKey =20000001,
                    OrderDate =new DateTime(2019,12,31),
                    CloseDate =new DateTime(2019,12,31),
                    SentDate =new DateTime(2019,12,31),
                    Status= OrderStatus.MAIL_SENT
                },
                 new Order
                {
                    Key =10000002,
                    HostingUnitKey = 1112,
                    GuestRequestKey =2000000,
                    OrderDate =new DateTime(2019,12,12),
                    CloseDate =new DateTime(2019,12,12),
                    SentDate =new DateTime(2019,12,12),
                    Status= OrderStatus.PROCESSING
                }
            };

            guestRequests = new List<GuestRequest>()
            {
                new GuestRequest
                {
                    Key=1,
                    ClientId = "12345678",
                    CreateDate =new DateTime(2019,04,21),
                    EntryDate =new DateTime(2020,01,01),
                    LeaveDate =new DateTime(2020,01,04),
                    Area = AreaLocation.ALL,
                    Type = UnitType.ZIMMER,
                    Adults =2,
                    Children=2,
                    Pool = ThreeOptions.YES,
                    Jacuzzi = ThreeOptions.YES,
                    ChildrensAttractions = ThreeOptions.MAYBE,
                    Garden = ThreeOptions.MAYBE,
                    Status = RequestStatus.CANCELLED
                },
                new GuestRequest
                {
                    Key=20000002,
                    ClientId = "12345678",
                    CreateDate =new DateTime(2019,12,12),
                    EntryDate =new DateTime(2020,01,10),
                    LeaveDate =new DateTime(2020,01,15),
                    Area = AreaLocation.ALL,
                    Type = UnitType.ZIMMER,
                    Adults =2,
                    Children=2,
                    Pool = ThreeOptions.YES,
                    Jacuzzi = ThreeOptions.YES,
                    ChildrensAttractions = ThreeOptions.MAYBE,
                    Garden = ThreeOptions.MAYBE,
                    Status = RequestStatus.OPEN
                },
                new GuestRequest
                {
                    Key=20000003,
                    ClientId = "12345678",
                    CreateDate =new DateTime(2019,12,31),
                    EntryDate =new DateTime(2020,02,01),
                    LeaveDate =new DateTime(2020,02,04),
                    Area = AreaLocation.JERUSALEM,
                    Type = UnitType.HOTEL,
                    Adults =2,
                    Children=0,
                    Pool = ThreeOptions.YES,
                    Jacuzzi = ThreeOptions.MAYBE,
                    ChildrensAttractions = ThreeOptions.NO,
                    Garden = ThreeOptions.MAYBE,
                    Status = RequestStatus.OPEN
                },
                 new GuestRequest
                {
                    Key=20000004,
                    ClientId = "12345678",
                    CreateDate =new DateTime(2019,12,31),
                    EntryDate =new DateTime(2020,02,10),
                    LeaveDate =new DateTime(2020,02,14),
                    Area = AreaLocation.JERUSALEM,
                    Type = UnitType.HOTEL,
                    Adults =2,
                    Children=0,
                    Pool = ThreeOptions.YES,
                    Jacuzzi = ThreeOptions.MAYBE,
                    ChildrensAttractions = ThreeOptions.NO,
                    Garden = ThreeOptions.MAYBE,
                    Status = RequestStatus.OPEN
                }
            };

            bankBranches = new List<BankBranch>()
            {
                new BankBranch
                {
                    BankNumber =10,
                    BankName = "leumi",
                    BranchNumber= 940,
                    BranchAddress= "Rutshild 58",
                    BranchCity = "petah tikva",
                    Status = Status.ACTIVE
                },
                new BankBranch
                {
                    BankNumber =10,
                    BankName = "leumi",
                    BranchNumber= 905,
                    BranchAddress= "Faran 15",
                    BranchCity = "Jerusalem",
                    Status = Status.ACTIVE
                }
            };

        }


    }
}
