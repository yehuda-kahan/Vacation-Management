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
    /// Interaction logic for GuestRequestListUserControl.xaml
    /// </summary>
    public partial class GuestRequestListUserControl : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        ObservableCollection<GuestRequestBO> guestRequests;
        HostBO myHost;
        public event Action<OrderBO> AddOrderEvent;
        public GuestRequestListUserControl(HostBO host)
        {
            InitializeComponent();
            myHost = host;
            GetRequestsAccAera();
            clearRequestsWithOdr();
            requestList.DataContext = guestRequests;
        }

        void GetRequestsAccAera()
        {
            guestRequests = new ObservableCollection<GuestRequestBO>();
            var Requests = bl.GetGuestRequests();

            foreach (GuestRequestBO request in Requests)
            {
                foreach (HostingUnitBO unit in myHost.UnitsHost)
                {
                    if (request.Area == unit.Area && request.Status == RequestStatusBO.OPEN)
                    {
                        guestRequests.Add(request);
                        break;
                    }
                }
            }
        }

        void clearRequestsWithOdr()
        {
            foreach (OrderBO order in myHost.OrdersHost)
            {
                int count = guestRequests.Count;
                for (int i = 0; i < count; ++i)
                    if (order.GuestRequest.Key == guestRequests[i].Key)
                    {
                        guestRequests.Remove(guestRequests[i]);
                        count--;
                    }
            }
        }

        private void requestList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Request_Detals_UserControl requestControl = new Request_Detals_UserControl((GuestRequestBO)requestList.SelectedItem, myHost);
            requestControl.AddOrderEvent += RequestControl_AddOrderEvent;
            MaterialDesignThemes.Wpf.DialogHost.Show(requestControl, "RequestListDialog");
        }

        private void RequestControl_AddOrderEvent(OrderBO obj)
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            AddOrderEvent(obj);
        }
    }
}
