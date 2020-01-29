using BO;
using BlApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PlGui
{
    /// <summary>
    /// Interaction logic for Create_Order_UserControl.xaml
    /// </summary>
    public partial class Create_Order_UserControl : UserControl
    {
        ObservableCollection<HostingUnitBO> myHostingUnits;
        public Create_Order_UserControl(ObservableCollection<HostingUnitBO> hostingUnits)
        {
            InitializeComponent();
            myHostingUnits = hostingUnits;
            GridHostingUnits.DataContext = myHostingUnits;
        }

        private void CrtOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
