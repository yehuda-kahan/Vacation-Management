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
using System.Net.Mail;
using System.Threading;
using System.ComponentModel;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for Create_Order_UserControl.xaml
    /// </summary>
    public partial class Create_Order_UserControl : UserControl
    {
        public static IBl bl = BlFactory.GetBL();
        OrderBO newOrder;
        PersonBO clientPerson;
        HostBO host;
        GuestRequestBO myRequest;
        ObservableCollection<HostingUnitBO> myHostingUnits;
        public event Action<OrderBO> AddOrderEvent;
        BackgroundWorker worker;
        uint resultKey = 0;

        public Create_Order_UserControl(ObservableCollection<HostingUnitBO> hostingUnits, GuestRequestBO request)
        {
            InitializeComponent();
            myHostingUnits = hostingUnits;
            myRequest = request;
            clientPerson = bl.GetPersonById(myRequest.ClientId);
            GridHostingUnits.DataContext = myHostingUnits;
        }

        private void CrtOrder_Click(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            HostingUnitBO unit = (HostingUnitBO)unitsList.SelectedItem;
            host = bl.GetHost(unit.Owner);
            if (!host.CollectingClearance)
            {
                MessageBox.Show("אינך יכול ליצור הזמנה מפני שלא נתת הרשאה לחיוב חשבונך", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            newOrder = new OrderBO
            {
                OrderDate = DateTime.Now,
                ClientFirstName = clientPerson.FirstName,
                ClientLastName = clientPerson.LastName,
                GuestRequest = myRequest,
                HostingUnit = unit,
                Status = OrderStatusBO.PROCESSING,
                HostId = unit.Owner,
            };
            Email email = new Email(myRequest, unit, host, clientPerson);
            worker.RunWorkerAsync(email);

            try { resultKey = bl.AddOrder(newOrder); }
            catch (DuplicateKeyException ex) { MessageBox.Show(ex.Message); return; }
            AddOrderEvent(newOrder);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Email email = (Email)e.Argument;
            bl.SendMail(email);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try { bl.UpdStatusOrder(resultKey, OrderStatusBO.MAIL_SENT); }
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); }
            catch (MissingMemberException ex) { MessageBox.Show(ex.Message); }
            AddOrderEvent(newOrder);
        }
    }
}

