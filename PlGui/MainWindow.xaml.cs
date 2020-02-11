using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BlApi;
using System.Data.Linq;
using System.ComponentModel;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BackgroundWorker downloadBankXml = new BackgroundWorker();
        static IBl bl = BlFactory.GetBL();
        public ClientBO client;
        public ObservableCollection<GuestRequestBO> requests;

        public MainWindow()
        {
            InitializeComponent();
            downloadBankXml.DoWork += DownloadBankXml_DoWork;
            downloadBankXml.RunWorkerCompleted += DownloadBankXml_RunWorkerCompleted;
        }

        private void DownloadBankXml_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void DownloadBankXml_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.downloadBankXml();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left)//TODO
            //    this.DragMove();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((Button)sender).Name == "Minimize")
                MinimizeIcon.Visibility = Visibility.Visible;
            if (((Button)sender).Name == "Maximize")
                MaximizeIcon.Visibility = Visibility.Visible;
            if (((Button)sender).Name == "Close")
                CloseIcone.Visibility = Visibility.Visible;

        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((Button)sender).Name == "Minimize")
                MinimizeIcon.Visibility = Visibility.Hidden;
            if (((Button)sender).Name == "Maximize")
                MaximizeIcon.Visibility = Visibility.Hidden;
            if (((Button)sender).Name == "Close")
                CloseIcone.Visibility = Visibility.Hidden;
        }

        private void Button_Click_ClouseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(this);
            else
                SystemCommands.MaximizeWindow(this);
        }

        private void Button_Click_MimimizeWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (TabControl_Login.SelectedIndex == 0)
                LogOut_Click(null, null);
        }

        private void MailCheck(object sender, KeyEventArgs e)
        {
            if (UserMail.Text == "")
                ErorrMail.Visibility = Visibility.Collapsed;
            if (!bl.IsValidMail(UserMail.Text) && UserMail.Text != "")
                ErorrMail.Visibility = Visibility.Visible;
            if (bl.IsValidMail(UserMail.Text))
                ErorrMail.Visibility = Visibility.Collapsed;
        }

        #region client

        private void LogIn_But(object sender, RoutedEventArgs e)
        {
            PersonBO temp = null;
            try
            {
                temp = bl.GetPersonByMail(UserMail.Text);
                if (Password.Password == temp.Password)
                {
                    client = bl.GetClient(temp.Id);
                    ClientInfo.DataContext = client;
                    requests = new ObservableCollection<GuestRequestBO>(client.ClientRequests); // making the list request of the guest
                    ListRequest.DataContext = requests;
                    clientLogin.Visibility = Visibility.Collapsed;
                    clientWindow.Visibility = Visibility.Visible;
                }
                ErorrInput.Visibility = Visibility.Visible;
            }
            catch (MissingMemberException ex)
            {
                ErorrInput.Visibility = Visibility.Visible;
            }
        }

        private void UserMail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (UserMail.Text == "")
                ErorrMail.Visibility = Visibility.Collapsed;
            if (!bl.IsValidMail(UserMail.Text) && UserMail.Text != "")
                ErorrMail.Visibility = Visibility.Visible;
            if (bl.IsValidMail(UserMail.Text))
                ErorrMail.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            downloadBankXml.RunWorkerAsync();
        }

        private void ChangeInfoBut_click(object sender, RoutedEventArgs e)
        {
            if ((string)ChangeInfoBut.Content == "עריכה")
            {
                ChangeInfoBut.Content = "שמירה";
                FirstName.IsEnabled = true;
                LastName.IsEnabled = true;
                Email.IsEnabled = true;
                Phone.IsEnabled = true;
            }
            else if ((string)ChangeInfoBut.Content == "שמירה")
            {
                client.PersonalInfo.FirstName = FirstName.Text;
                client.PersonalInfo.LastName = LastName.Text;
                client.PersonalInfo.Email = Email.Text;
                client.PersonalInfo.PhoneNomber = Phone.Text;
                bl.UpdPerson(client.PersonalInfo);
                ChangeInfoBut.Content = "עריכה";
                FirstName.IsEnabled = false;
                LastName.IsEnabled = false;
                Email.IsEnabled = false;
                Phone.IsEnabled = false;
            }
        }

        private void ListRequest_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void list_click(object sender, MouseButtonEventArgs e)
        {
            GuestRequestBO request = (GuestRequestBO)ListRequest.SelectedItem;
            DialogRequestUserControl requestUserControl = new DialogRequestUserControl(request);
            MaterialDesignThemes.Wpf.DialogHost.Show(requestUserControl, "clientWinDialog");
        }



        private void createRequest(object sender, RoutedEventArgs e)
        {
            AddRequestDialogUserControl requestUserControlNew = new AddRequestDialogUserControl(client.PersonalInfo.Id);
            requestUserControlNew.UpdList += RequestUserControlNew_UpdList;
            MaterialDesignThemes.Wpf.DialogHost.Show(requestUserControlNew, "clientWinDialog");

        }

        private void RequestUserControlNew_UpdList()
        {
            requests = new ObservableCollection<GuestRequestBO>(bl.GetClient(client.PersonalInfo.Id).ClientRequests);
            ListRequest.DataContext = requests;
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void SingUp_click(object sender, RoutedEventArgs e)
        {
            UserControlSingUp addPerson = new UserControlSingUp();
            addPerson.OpenClientWin += AddPerson_OpenClientWin;
            MaterialDesignThemes.Wpf.DialogHost.Show(addPerson, "SingUpFourm");
        }

        private void AddPerson_OpenClientWin(string obj)
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            client = bl.GetClient(obj);
            ClientInfo.DataContext = client;
            requests = new ObservableCollection<GuestRequestBO>(client.ClientRequests); // making the list request of the guest
            ListRequest.DataContext = requests;
            clientLogin.Visibility = Visibility.Collapsed;
            clientWindow.Visibility = Visibility.Visible;
        }

        #endregion client


        /////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                        Host Functions
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Host


        HostBO host;
        ObservableCollection<OrderBO> hostOrders;
        ObservableCollection<OrderBO> hostAprrovedOrds;

        private void Host_LogIn_But(object sender, RoutedEventArgs e)
        {
            PersonBO temp = null;
            try
            {
                temp = bl.GetPersonByMail(HostUserMail.Text);
                if (HostPassword.Password == temp.Password)
                {
                    try
                    {
                        host = bl.GetHost(temp.Id);
                        HostInfo.DataContext = host;
                        hostOrders = new ObservableCollection<OrderBO>(host.OrdersHost); // making the list request of the guest
                        hostAprrovedOrds = new ObservableCollection<OrderBO>(host.AppovedOrdersHost);
                        AprrovedOrderList.DataContext = hostAprrovedOrds;
                        OrderList.DataContext = hostOrders;
                        HostLogin.Visibility = Visibility.Collapsed;
                        HostWindow.Visibility = Visibility.Visible;
                    }
                    catch (MissingMemberException ex)
                    {
                        MessageBox.Show("אינך רשום במערכת כמארח");
                        AddHoset_Click(temp.Id);
                    }//TODO 
                }
                HostErorrInput.Visibility = Visibility.Visible;
            }
            catch (MissingMemberException ex)
            {
                HostErorrInput.Visibility = Visibility.Visible;
            }
        }

        private void Host_SingUp_click(object sender, RoutedEventArgs e)
        {
            UserControlSingUp addPerson = new UserControlSingUp();
            addPerson.OpenClientWin += AddHoset_Click;
            MaterialDesignThemes.Wpf.DialogHost.Show(addPerson, "HostLoginDialog");
        }
        private void AddHoset_Click(string obj)
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            host = new HostBO();
            host.PersonalInfo = bl.GetPersonById(obj);
            host.BankDetales = new BankBranchBO();
            UserControlBankInfo AddBankDeitels = new UserControlBankInfo(host);
            AddBankDeitels.OpenHostWin += AddBankDeitels_OpenHostWin;
            MaterialDesignThemes.Wpf.DialogHost.Show(AddBankDeitels, "HostLoginDialog");
        }

        private void AddBankDeitels_OpenHostWin(string obj)
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            host = bl.GetHost(obj);
            HostInfo.DataContext = host;
            hostOrders = new ObservableCollection<OrderBO>(host.OrdersHost); // making the list request of the guest
            OrderList.DataContext = hostOrders;
            HostLogin.Visibility = Visibility.Collapsed;
            HostWindow.Visibility = Visibility.Visible;
        }


        private void Host_UserMail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (HostUserMail.Text == "")
                HostErorrMail.Visibility = Visibility.Collapsed;
            if (!bl.IsValidMail(HostUserMail.Text) && HostUserMail.Text != "")
                HostErorrMail.Visibility = Visibility.Visible;
            if (bl.IsValidMail(HostUserMail.Text))
                HostErorrMail.Visibility = Visibility.Collapsed;
        }

        private void OrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderUserControl1 test = new OrderUserControl1((OrderBO)OrderList.SelectedItem);
            MaterialDesignThemes.Wpf.DialogHost.Show(test, "HostWinDialog");
            test.UpdListOrder += Test_UpdListOrder;
        }
        private void AprrovedOrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderUserControl1 test = new OrderUserControl1((OrderBO)AprrovedOrderList.SelectedItem);
            MaterialDesignThemes.Wpf.DialogHost.Show(test, "HostWinDialog");
            test.UpdListOrder += Test_UpdListOrder;
        }

        private void Test_UpdListOrder()
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            hostOrders = new ObservableCollection<OrderBO>(bl.GetOdrsOfHost(host.PersonalInfo.Id));
            OrderList.DataContext = hostOrders;
            hostAprrovedOrds = new ObservableCollection<OrderBO>(host.AppovedOrdersHost);
            AprrovedOrderList.DataContext = hostAprrovedOrds;
        }


        private void myUnitsBut_Click(object sender, RoutedEventArgs e)
        {
            MyHostingUnitsUserControl myUnits = new MyHostingUnitsUserControl(host.UnitsHost, host.PersonalInfo.Id);
            MaterialDesignThemes.Wpf.DialogHost.Show(myUnits, "HostWinDialog");
        }

        private void guestRequstBut_Click(object sender, RoutedEventArgs e)
        {
            host = bl.GetHost(host.PersonalInfo.Id);
            GuestRequestListUserControl listRequestControl = new GuestRequestListUserControl(host);
            listRequestControl.AddOrderEvent += ListRequestControl_AddOrderEvent;
            MaterialDesignThemes.Wpf.DialogHost.Show(listRequestControl, "HostWinDialog");
        }

        private void ListRequestControl_AddOrderEvent(OrderBO obj)
        {
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            hostOrders = new ObservableCollection<OrderBO>(bl.GetOdrsOfHost(host.PersonalInfo.Id));
            OrderList.DataContext = hostOrders;
        }



        private void HostChangeInfoBut_Click(object sender, RoutedEventArgs e)
        {
            if ((string)HostChangeInfoBut.Content == "עריכה")
            {
                HostChangeInfoBut.Content = "שמירה";
                HostFirstName.IsEnabled = true;
                HostLastName.IsEnabled = true;
                HostEmail.IsEnabled = true;
                HostPhone.IsEnabled = true;
            }
            else if ((string)HostChangeInfoBut.Content == "שמירה")
            {
                bl.UpdPerson(host.PersonalInfo);
                HostChangeInfoBut.Content = "עריכה";
                HostFirstName.IsEnabled = false;
                HostLastName.IsEnabled = false;
                HostEmail.IsEnabled = false;
                HostPhone.IsEnabled = false;
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            HostLogin.Visibility = Visibility.Visible;
            HostWindow.Visibility = Visibility.Collapsed;
            clientLogin.Visibility = Visibility.Visible;
            clientWindow.Visibility = Visibility.Collapsed;
            UserMail.Text = "";
            Password.Password = "";
            HostUserMail.Text = "";
            HostPassword.Password = "";
            ErorrMail.Visibility = Visibility.Collapsed;
            ErorrInput.Visibility = Visibility.Collapsed;
            HostErorrInput.Visibility = Visibility.Collapsed;
            HostErorrMail.Visibility = Visibility.Collapsed;
        }

        private void HostChangeBankInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserControlBankInfo AddBankDeitels = new UserControlBankInfo(host);
            AddBankDeitels.OpenHostWin += AddBankDeitels_OpenHostWin;
            MaterialDesignThemes.Wpf.DialogHost.Show(AddBankDeitels, "HostLoginDialog");
        }
        #endregion Host

        #region manager

        ObservableCollection<GuestRequestBO> requestsManag;
        ObservableCollection<OrderBO> ordersManag;

        private void Manager_UserMail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (ManagerUserMail.Text == "")
            {
                ManagerErorrMail.Visibility = Visibility.Collapsed;
                ManagerUserMail.BorderBrush = Brushes.Gray;
                ManagerLogInBut.IsEnabled = true;
            }
            if (!bl.IsValidMail(ManagerUserMail.Text) && ManagerUserMail.Text != "")
            {
                ManagerErorrMail.Visibility = Visibility.Visible;
                ManagerUserMail.BorderBrush = Brushes.Red;
                ManagerLogInBut.IsEnabled = false;

            }
            if (bl.IsValidMail(ManagerUserMail.Text))
            {
                ManagerErorrMail.Visibility = Visibility.Hidden;
                ManagerUserMail.BorderBrush = Brushes.Gray;
                ManagerLogInBut.IsEnabled = true;
            }
        }

        private void Manager_LogIn_But(object sender, RoutedEventArgs e)
        {
            //TODO 
            requestsManag = new ObservableCollection<GuestRequestBO>(bl.GetGuestRequests());
            ordersManag = new ObservableCollection<OrderBO>(bl.GetAppOrders());
            requestListManager.DataContext = requestsManag;
            AppListOrdManager.DataContext = ordersManag;
            ManagerLogin.Visibility = Visibility.Collapsed;
            ManagerWindow.Visibility = Visibility.Visible;
        }

        private void requestListManager_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogRequestUserControl RequestControl = new DialogRequestUserControl((GuestRequestBO)requestListManager.SelectedItem);
            MaterialDesignThemes.Wpf.DialogHost.Show(RequestControl, "ManagerDialog");
        }


        #endregion manager

        private void AppListOrdManager_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderUserControl1 orderControl = new OrderUserControl1((OrderBO)AppListOrdManager.SelectedItem);
            MaterialDesignThemes.Wpf.DialogHost.Show(orderControl, "ManagerDialog");
        }
    }
}