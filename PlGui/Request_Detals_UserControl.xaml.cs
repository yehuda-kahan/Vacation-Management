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
    /// Interaction logic for Request_Detals_UserControl.xaml
    /// </summary>
    public partial class Request_Detals_UserControl : UserControl
    {
        HostBO myHost;
        PersonBO Person;
        GuestRequestBO myRequest;
        ObservableCollection<HostingUnitBO> hostingUnits;
        public static IBl bl = BlFactory.GetBL();
        public event Action<OrderBO> AddOrderEvent;
        public Request_Detals_UserControl(GuestRequestBO request, HostBO host)
        {
            InitializeComponent();
            myRequest = request;
            myHost = host;
            Person = bl.GetPersonById(myRequest.ClientId);
            FirstName.Text = Person.FirstName;
            LastName.Text = Person.LastName;
            GridRequest.DataContext = myRequest;
            initi_comboxes();
        }

        void initi_comboxes()
        {
            comboJacuzzi.SelectedIndex = (int)myRequest.Jacuzzi;
            comboGarden.SelectedIndex = (int)myRequest.Garden;
            comboPool.SelectedIndex = (int)myRequest.Pool;
            comboUnitType.SelectedIndex = (int)myRequest.Type;
            comboChildrensAttractions.SelectedIndex = (int)myRequest.ChildrensAttractions;
            comboArea.SelectedIndex = (int)myRequest.Area;
        }

        private void CheckAvelabiltyBtn_Click(object sender, RoutedEventArgs e)
        {
            hostingUnits = new ObservableCollection<HostingUnitBO>();
            var avalableUnits = bl.GetAvalableUnits(myRequest.EntryDate, (uint)(myRequest.LeaveDate - myRequest.EntryDate).Days);
            foreach (HostingUnitBO unit in avalableUnits)
            {
                if (unit.Owner == myHost.PersonalInfo.Id && (myRequest.Area == unit.Area|| myRequest.Area== AreaLocationBO.ALL) && unit.Status == StatusBO.פעיל)
                    hostingUnits.Add(unit);
            }
            Create_Order_UserControl create_Order_UserControl = new Create_Order_UserControl(hostingUnits, myRequest);
            create_Order_UserControl.AddOrderEvent += Create_Order_UserControl_AddOrderEvent;
            MaterialDesignThemes.Wpf.DialogHost.Show(create_Order_UserControl, "RequestDetales");
            
        }

        private void Create_Order_UserControl_AddOrderEvent(OrderBO obj)
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            AddOrderEvent(obj);
        }
    }
}
