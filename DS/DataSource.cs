﻿using System;
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
                    Id ="12345678",
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
                     Id ="204284376",
                    IdType = IdTypes.DRIVING_LICENSE,
                    FirstName ="yehuda",
                    LastName ="kahan",
                    PhoneNomber="+972587271600",
                    Email="1",
                    Password="1",
                    Status = Status.ACTIVE
                }
            };

            hosts = new List<Host>()
            {
                new Host
                {
                    Id="204284376",
                    BankNumber=10,
                    BranchNumber = 905,
                    BankAccountNumber= 57112658,
                    CollectingClearance= false,
                    Status = Status.ACTIVE,
                    WebSite = "www.google.com"
                },
            };

            hostingUnits = new List<HostingUnit>()
            {
                new HostingUnit
                {
                    Key=1111,
                    Owner="204284376",
                    HostingUnitName = "The villa",
                    Diary =new bool[12,31],
                    Status = Status.ACTIVE,
                    Area = AreaLocation.JERUSALEM
                },
                new HostingUnit
                {
                    Key=1112,
                    Owner="204284376",
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
                    HostId = "204284376",
                    Key =10000001,
                    HostingUnitKey = 1111,
                    GuestRequestKey =20000001,
                    OrderDate =new DateTime(2019,12,31),
                    CloseDate =default,
                    SentDate =new DateTime(2019,12,31),
                    Status= OrderStatus.MAIL_SENT
                },
                 new Order
                {
                    HostId = "204284376",
                    Key =10000002,
                    HostingUnitKey = 1112,
                    GuestRequestKey =20000000,
                    OrderDate =new DateTime(2019,12,12),
                   
                    SentDate =new DateTime(2019,12,12),
                    Status= OrderStatus.PROCESSING
                }
            };

            guestRequests = new List<GuestRequest>()
            {
                new GuestRequest
                {
                    Key=20000000,
                    ClientId = "12345678",
                    CreateDate =new DateTime(2020,04,21),
                    EntryDate =new DateTime(2020,02,03),
                    LeaveDate =new DateTime(2020,02,08),
                    Area = AreaLocation.JERUSALEM,
                    Type = UnitType.ZIMMER,
                    Adults =2,
                    Children=2,
                    Pool = ThreeOptions.YES,
                    Jacuzzi = ThreeOptions.YES,
                    ChildrensAttractions = ThreeOptions.MAYBE,
                    Garden = ThreeOptions.MAYBE,
                    Status = RequestStatus.ORDERED
                },
                new GuestRequest
                {
                    Key=20000001,
                    ClientId = "12345678",
                    CreateDate =new DateTime(2019,12,12),
                    EntryDate =new DateTime(2020,01,10),
                    LeaveDate =new DateTime(2020,01,15),
                    Area = AreaLocation.JERUSALEM,
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
                    BankName = "לאומי",
                    BranchNumber= 940,
                    BranchAddress= "רוטשילד 50",
                    BranchCity = "פתח תקווה",
                    Status = Status.ACTIVE
                },
                new BankBranch
                {
                    BankNumber =10,
                    BankName = "לאומי",
                    BranchNumber= 905,
                    BranchAddress= "פארן 15",
                    BranchCity = "ירושלים",
                    Status = Status.ACTIVE
                }
            };

        }


    }
}
