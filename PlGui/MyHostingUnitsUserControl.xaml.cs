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
        static IBl bl = BlFactory.GetBL();
        ObservableCollection<HostingUnitBO> hostingUnits;
        string hostId;
        public MyHostingUnitsUserControl(IEnumerable<HostingUnitBO> hostings, string Id)
        {
            InitializeComponent();
            hostingUnits = new ObservableCollection<HostingUnitBO>(hostings);
            unitsList.DataContext = hostingUnits;
            hostId = Id;
        }

        private void UnitDetals_Click(object sender, RoutedEventArgs e)
        {
            UnitUserCuntrol unitControl = new UnitUserCuntrol((HostingUnitBO)unitsList.SelectedItem);
            unitControl.UpdUnit += UnitControl_UpdUnit;
            MaterialDesignThemes.Wpf.DialogHost.Show(unitControl, "HostingUnitsDialog");
        }

        private void UnitControl_UpdUnit()
        {
            hostingUnits = new ObservableCollection<HostingUnitBO>(bl.GetHostUnits(hostId));
            unitsList.DataContext = hostingUnits;
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void AddUnit_Click(object sender, RoutedEventArgs e)
        {
            AddUnitUserControl unitUserControl = new AddUnitUserControl(hostId);
            unitUserControl.AddUnitEv += UnitUserControl_AddUnitEv;
            MaterialDesignThemes.Wpf.DialogHost.Show(unitUserControl, "HostingUnitsDialog");
        }

        private void UnitUserControl_AddUnitEv()
        {
            hostingUnits = new ObservableCollection<HostingUnitBO>(bl.GetHostUnits(hostId));
            unitsList.DataContext = hostingUnits;
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
