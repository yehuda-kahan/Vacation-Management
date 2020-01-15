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


namespace PlGui
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static IBl bl = BlFactory.GetBL();

        public MainWindow()
        {
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
            if (!bl.IsValidMail(UserMail.Text))
                MessageBox.Show("mail problem");
            PersonBO temp = null;
            try
            {
                temp=bl.GetPersonByMail(UserMail.Text);
                if (Password.Password == temp.Password)
                {
                    clientLogin.Visibility = Visibility.Collapsed;
                    clientWindow.Visibility = Visibility.Visible;
                }
                else
                    MessageBox.Show("worng password");
            }
            catch(MissingMemberException ex)
            {
                MessageBox.Show("the maill was not found in the system "+ ex.ToString());
            }
        }
    }
}
