﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OrderBO
    {
        public uint Key { get; set; }
        public uint HostingUnitKey { get; set; }
        public uint GuestRequestKey { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public string LastNameClient  { get; set; }
        public string FirstNameClient  { get; set; }
        public string HostId { get; set; }
        public double Fee { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CloseDate { get; set; }


        public override string ToString()
        {
            return "Order Key : " + Key
                + "\nHosting unit key : " + HostingUnitKey
                + "\nGuest request key : " + GuestRequestKey
                + "\nOrder date : " + OrderDate.ToString(format: "dd/MM/yyyy")
                + "\n";
        }
    }
}
