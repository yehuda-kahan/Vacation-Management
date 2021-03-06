﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class GuestRequestBO : IComparable<GuestRequestBO>
    {
        public uint Key { get; set; }
        public string ClientId { get; set; }
        public RequestStatusBO Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public AreaLocationBO Area { get; set; }
        // public SubArea Area { get; set; } // Optiaonal
        public UnitTypeBO Type { get; set; }
        public uint Adults { get; set; }
        public uint Children { get; set; }
        public ThreeOptionsBO Pool { get; set; }
        public ThreeOptionsBO Jacuzzi { get; set; }
        public ThreeOptionsBO Garden { get; set; }
        public ThreeOptionsBO ChildrensAttractions { get; set; }

       

        public int CompareTo(GuestRequestBO other)
        {
            if (this.Key == other.Key)
                return 0;
            else if (this.Key > other.Key)
                return 1;
            else if (this.Key < other.Key)
                return -1;
            return -1;
        }

        public override string ToString()
        {
            return "Request Key : " + Key
                + "\nClient ID : " + ClientId
                + "\nCreate date : " + CreateDate.ToString(format: "dd/MM/yyyy")
                + "\nEntry date : " + EntryDate.ToString(format: "dd/MM/yyyy")
                + "\nLeave date : " + LeaveDate.ToString(format: "dd/MM/yyyy")
                + "\n";
        }
    }
}

