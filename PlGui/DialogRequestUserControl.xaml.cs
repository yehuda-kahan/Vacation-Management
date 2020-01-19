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
using BlApi;
using BO;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for DialogRequestUserControl.xaml
    /// </summary>
    public partial class DialogRequestUserControl : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        private GuestRequestBO request;

        public DialogRequestUserControl(GuestRequestBO guestRequest)
        {
            InitializeComponent();
            request = guestRequest;
            UserControlGrid.DataContext = request;
            comboArea.SelectedIndex = (int)request.Area;
            comboJacuzzi.SelectedIndex = (int)request.Jacuzzi;
            comboPool.SelectedIndex = (int)request.Pool;
            comboGarden.SelectedIndex = (int)request.Garden;
            comboChildrensAttractions.SelectedIndex = (int)request.ChildrensAttractions;
            if (request.Status == RequestStatusBO.CANCELLED || request.Status == RequestStatusBO.ORDERED)
                ChangBut.IsEnabled = true;
        }


        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).Name == "comboArea")
                request.Area = (AreaLocationBO)comboArea.SelectedIndex;
            else if (((ComboBox)sender).Name == "comboJacuzzi")
                request.Jacuzzi = (ThreeOptionsBO)comboJacuzzi.SelectedIndex;
            else if (((ComboBox)sender).Name == "comboPool")
                request.Pool = (ThreeOptionsBO)comboPool.SelectedIndex;
            else if (((ComboBox)sender).Name == "comboGarden")
                request.Garden = (ThreeOptionsBO)comboGarden.SelectedIndex;
            else if (((ComboBox)sender).Name == "comboChildrensAttractions")
                request.ChildrensAttractions = (ThreeOptionsBO)comboChildrensAttractions.SelectedIndex;
            else if (((ComboBox)sender).Name == "comboUnitType")
                request.Type = (UnitTypeBO)comboUnitType.SelectedIndex;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "PlusAdBut")
            {
                if (Convert.ToInt32(adults.Text) >= 99)
                    return;
                else
                    adults.Text = Convert.ToString((Convert.ToInt32(adults.Text) + 1));
            }
            else if (((Button)sender).Name == "PlusChBut")
            {
                if (Convert.ToInt32(children.Text) >= 99)
                    return;
                else
                    children.Text = Convert.ToString((Convert.ToInt32(children.Text) + 1));
            }
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "MinAdBut")
            {
                if (Convert.ToInt32(children.Text) <= 0)
                    return;
                else
                    children.Text = Convert.ToString((Convert.ToInt32(children.Text) - 1));
            }
            else if (((Button)sender).Name == "MinChBut")
            {
                if (Convert.ToInt32(children.Text) <= 0)
                    return;
                else
                    children.Text = Convert.ToString((Convert.ToInt32(children.Text) - 1));
            }
        }

        private void ChangBut_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ChangBut.Content == "עריכה")
            {
                ChangBut.Content = "שמור";
                EntryDate.IsEnabled = true;
                EntryDate.DisplayDateStart = DateTime.Today;
                LeaveDate.IsEnabled = true;
                adults.IsEnabled = true;
                children.IsEnabled = true;
                comboJacuzzi.IsEnabled = true;
                comboArea.IsEnabled = true;
                comboGarden.IsEnabled = true;
                comboPool.IsEnabled = true;
                comboArea.IsEnabled = true;
                comboChildrensAttractions.IsEnabled = true;
                comboUnitType.IsEnabled = true;
                MinAdBut.IsEnabled = true;
                PlusAdBut.IsEnabled = true;
                MinChBut.IsEnabled = true;
                PlusChBut.IsEnabled = true;

            }
            else if ((string)ChangBut.Content == "שמור")
            {
                ChangBut.Content = "עריכה";
                EntryDate.IsEnabled = false;
                LeaveDate.IsEnabled = false;
                adults.IsEnabled = false;
                children.IsEnabled = false;
                comboJacuzzi.IsEnabled = false;
                comboArea.IsEnabled = false;
                comboGarden.IsEnabled = false;
                comboPool.IsEnabled = false;
                comboArea.IsEnabled = false;
                comboChildrensAttractions.IsEnabled = false;
                comboUnitType.IsEnabled = false;
                MinAdBut.IsEnabled = false;
                PlusAdBut.IsEnabled = false;
                MinChBut.IsEnabled = false;
                PlusChBut.IsEnabled = false;
            }
        }

       
    }
}
