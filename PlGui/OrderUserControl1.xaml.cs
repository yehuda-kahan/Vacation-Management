using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using BO;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for OrderUserControl1.xaml
    /// </summary>
    public partial class OrderUserControl1 : UserControl
    {
        OrderBO order;
        public event Action UpdListOrder;
        static IBl bl = BlFactory.GetBL();

        public OrderUserControl1(OrderBO givenOrder)
        {
            InitializeComponent();
            order = givenOrder;
            UserControlGrid.DataContext = order;
            comStatus.SelectedIndex = (int)order.Status;
            if (order.CloseDate.Year == 0001)
                CloseDate.Visibility = Visibility.Hidden;
            if (comStatus.SelectedIndex == 2)
                comStatus.IsEnabled = false;
            if (order.Status == OrderStatusBO.APPROVED)
                CreateBut.IsEnabled = false;
        }

        private void Upd_Click(object sender, RoutedEventArgs e)
        {
            if (comStatus.SelectedIndex == 2)
            {
                CloseDate.Visibility = Visibility.Visible;
                CloseDate.SelectedDate = DateTime.Now;
                comStatus.IsEnabled = false;
            }
            bl.UpdStatusOrder(order.Key, order.Status);
            UpdListOrder();
        }

        private void comStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            order.Status = (OrderStatusBO)comStatus.SelectedIndex;

        }
    }
}
