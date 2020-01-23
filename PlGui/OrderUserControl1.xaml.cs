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
        public OrderUserControl1(OrderBO order)
        {
            InitializeComponent();
            UserControlGrid.DataContext = order;
        }

        private void Upd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
