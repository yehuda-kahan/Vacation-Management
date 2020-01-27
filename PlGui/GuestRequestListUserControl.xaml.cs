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

        public GuestRequestListUserControl(HostBO host)
        {
            InitializeComponent();
            myHost = host;
            GetRequestsAccAera();
        }

        void GetRequestsAccAera()
        {
            guestRequests = new ObservableCollection<GuestRequestBO>();
            var grupRequestByArea = bl.GetRequestByArea();

            foreach (HostingUnitBO unit in myHost.UnitsHost)
            {
                foreach (var area in grupRequestByArea)
                {
                    if (unit.Area == area.Key)
                    {
                        foreach (var request in area)
                            guestRequests.Add(request);
                    }
                }
            }
            guestRequests = new ObservableCollection<GuestRequestBO>(guestRequests.Distinct());
        }
    }
}
