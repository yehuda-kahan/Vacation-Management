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


namespace PlGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static IBl bl = BlFactory.GetBL();
        public ClientBO client;
        ObservableCollection<GuestRequestBO> requests;

        public MainWindow()
        {
            List<int> LongListToTestComboVirtualization = new List<int>(Enumerable.Range(0, 1000));
            InitializeComponent();
            

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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
            if (tabControl.SelectedIndex == 1)
            {
                //AdminPasswordBorder.Visibility = Visibility.Collapsed;
                //AdminPasswordBorder.Visibility = Visibility.Visible;
            }
        }



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

            MaterialDesignThemes.Wpf.DialogHost.Show(requestUserControl);

        }            
    }
}
