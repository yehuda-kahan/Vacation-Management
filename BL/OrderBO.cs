using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BO
{
    public class OrderBO //: DependencyObject
    {
        //public uint Key
        //{
        //    get { return (uint)GetValue(KeyProperty); }
        //    set { SetValue(KeyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty KeyProperty =
        //    DependencyProperty.Register("Key", typeof(uint), typeof(OrderBO), new UIPropertyMetadata(0));

        public uint Key { get; set; }



        //public OrderStatusBO Status
        //{
        //    get { return (OrderStatusBO)GetValue(StatusProperty); }
        //    set { SetValue(StatusProperty, value); }
        //}

        //Using a DependencyProperty as the backing store for Status.This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty StatusProperty =
        //    DependencyProperty.Register("Status", typeof(OrderStatusBO), typeof(OrderBO), new PropertyMetadata(0));



        public HostingUnitBO HostingUnit { get; set; }
        public GuestRequestBO GuestRequest { get; set; }
        public string HostId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public double Fee { get; set; }
        public OrderStatusBO Status { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CloseDate { get; set; }



        public override string ToString()
        {
            return "Order Key : " + Key
                + "\nHosting unit : " + HostingUnit
                + "\nGuest request : " + GuestRequest
                + "\nOrder date : " + OrderDate.ToString(format: "dd/MM/yyyy")
                + "\n";
        }
    }
}
