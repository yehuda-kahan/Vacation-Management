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
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for MyHostingUnitsUserControl.xaml
    /// </summary>
    public partial class MyHostingUnitsUserControl : UserControl
    {
        ObservableCollection<HostingUnitBO> hostingUnits;
        public MyHostingUnitsUserControl(IEnumerable<HostingUnitBO> hostings)
        {
            InitializeComponent();
            hostingUnits = new ObservableCollection<HostingUnitBO>(hostings);
            unitsList.DataContext = hostingUnits;
        }

        private void unitsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UnitDetals_Click(object sender, RoutedEventArgs e)
        {                     
            UnitUserCuntrol UnitControl = new UnitUserCuntrol((HostingUnitBO)unitsList.SelectedItem);
            MaterialDesignThemes.Wpf.DialogHost.Show(UnitControl, "HostingUnitsDialog");
        }
    }
}
