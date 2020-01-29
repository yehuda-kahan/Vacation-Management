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
using System.Data.Linq;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for Create_Order_UserControl.xaml
    /// </summary>
    public partial class Create_Order_UserControl : UserControl
    {
        public static IBl bl = BlFactory.GetBL();
        OrderBO newOrder;
        PersonBO Person;
        GuestRequestBO myRequest;
        ObservableCollection<HostingUnitBO> myHostingUnits;
        public event Action<OrderBO> AddOrderEvent;
        public Create_Order_UserControl(ObservableCollection<HostingUnitBO> hostingUnits, GuestRequestBO request)
        {
            InitializeComponent();
            myHostingUnits = hostingUnits;
            myRequest = request;
            Person = bl.GetPersonById(myRequest.ClientId);
            GridHostingUnits.DataContext = myHostingUnits;
        }

        private void CrtOrder_Click(object sender, RoutedEventArgs e)
        {
            HostingUnitBO unit = (HostingUnitBO)unitsList.SelectedItem;
            newOrder = new OrderBO
            {
                OrderDate = DateTime.Now,
                ClientFirstName = Person.FirstName,
                ClientLastName = Person.LastName,
                GuestRequest = myRequest,
                HostingUnit = unit,
                Status = OrderStatusBO.PROCESSING,
                HostId = unit.Owner,
            };
            try { bl.AddOrder(newOrder); }
            catch (DuplicateKeyException ex) { MessageBox.Show(ex.Message); return; }
            AddOrderEvent(newOrder);
        }
    }
}
