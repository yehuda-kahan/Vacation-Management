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
using System.Data.Linq;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for AddRequestDialogUserControl.xaml
    /// </summary>
    public partial class AddRequestDialogUserControl : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        private GuestRequestBO request = new GuestRequestBO();
        internal event Action UpdList;


        public AddRequestDialogUserControl(string id)
        {
            request.EntryDate = DateTime.Now;
            request.LeaveDate = DateTime.Now.AddDays(1);
           
            InitializeComponent();
            EntryDate.DisplayDateStart = DateTime.Now;
            request.ClientId =id;
            request.CreateDate = DateTime.Now;
            UserControlGrid.DataContext = request;
            comboArea.SelectedIndex = (int)request.Area;
            comboJacuzzi.SelectedIndex = (int)request.Jacuzzi;
            comboPool.SelectedIndex = (int)request.Pool;
            comboGarden.SelectedIndex = (int)request.Garden;
            comboChildrensAttractions.SelectedIndex = (int)request.ChildrensAttractions;
            if (request.Status == RequestStatusBO.CANCELLED || request.Status == RequestStatusBO.ORDERED)
                CreateBut.IsEnabled = false;
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
        private void EntryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EntryDate.SelectedDate.HasValue)
            {
                LeaveDate.DisplayDateStart = EntryDate.SelectedDate.Value.AddDays(1);
                LeaveDate.DataContext = LeaveDate;
            }
        }
        private void CreateBut_Click(object sender, RoutedEventArgs e)
        {
            uint temp = 0;
            try { temp = bl.AddRequest(request); }
            catch (DuplicateKeyException ex) { MessageBox.Show("here1"); }
            catch (FormatException ex) { MessageBox.Show("here2"); }//TODO mess box
            UpdList();
        }

        private void EntryDate_DialogOpened(object sender, MaterialDesignThemes.Wpf.DialogOpenedEventArgs eventArgs)
        {

        }
    }
}
