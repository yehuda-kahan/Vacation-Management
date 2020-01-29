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
    /// Interaction logic for DiaryUserControl.xaml
    /// </summary>
    public partial class UnitUserCuntrol : UserControl
    {
        HostingUnitBO myUnit;
        static IBl bl = BlFactory.GetBL();
        public event Action UpdUnit;

        public UnitUserCuntrol(HostingUnitBO unit)
        {
            InitializeComponent();
            viewCalender.SelectionMode = CalendarSelectionMode.SingleRange;
            myUnit = unit;
            myUnit.Diary[1, 1] = true;
            myUnit.Diary[1, 2] = true;
            myUnit.Diary[1, 3] = true;
            GridCalender.DataContext = myUnit;
            comArea.SelectedIndex = (int)myUnit.Area;
            viewCalender.DisplayDateStart = DateTime.Now;
            Initilize_Calender_Detalse();
            delBtn();
        }

        void delBtn()
        {
            if (myUnit.Status == StatusBO.לא_פעיל)
            {
                Act_Inact_Unit_Btn.Background = Brushes.DarkGreen;
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Plus;
                Act_Inact_Unit_Btn.ToolTip = "הפעל יחידה זו";
            }
        }

        void Initilize_Calender_Detalse()
        {
            //DateTime startDate = default;
            //DateTime endDate;
            //CalendarDateRange dateRange;
            //bool StartFlag = false;

            for (int i = 0; i < 12; ++i)
            {
                for (int j = 0; j < 31; ++j)
                {
                    if (myUnit.Diary[i, j] == true)
                        viewCalender.BlackoutDates.Add(new CalendarDateRange(new DateTime(DateTime.Now.Year, i + 1, j + 1)));
                    //// for the first resevation day
                    //if (StartFlag == false && myUnit.Diary[i, j] == true)
                    //{
                    //    startDate = new DateTime(DateTime.Now.Year, i + 1, j + 1);
                    //    StartFlag = true;
                    //}
                    //// for the last resevation day
                    //if (StartFlag == true && myUnit.Diary[i, j] == false)
                    //{
                    //    endDate = new DateTime(DateTime.Now.Year, i + 1, j + 1).AddDays(-1);
                    //    dateRange = new CalendarDateRange(startDate, endDate);
                    //    viewCalender.BlackoutDates.Add(dateRange);
                    //    StartFlag = false;
                    //}
                }
            }

        }

        private void MarkDays_Click(object sender, RoutedEventArgs e)
        {

            if (viewCalender.SelectedDates.Count > 0)
            {
                List<DateTime> temp = viewCalender.SelectedDates.ToList();
                temp.Sort();
                viewCalender.SelectedDates.Clear();
                viewCalender.BlackoutDates.Add(new CalendarDateRange(temp.First(), temp.Last()));
                DateTime run = temp.First();
                for (; run <= temp.Last(); run = run.AddDays(1))
                    myUnit.SetDates(run);
                bl.UpdUnit(myUnit);
            }
        }

        private void viewCalender_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void Upd_Click(object sender, RoutedEventArgs e)
        {
            if ((string)UpdBut.Content == "עריכה")
            {
                UpdBut.Content = "שמירה";
                UnitName.IsEnabled = true;
            }
            else if ((string)UpdBut.Content == "שמירה")
            {
                bl.UpdUnit(myUnit);  // no need to catch exception
                UpdBut.Content = "עריכה";
                UnitName.IsEnabled = false;
            }
        }

        private void deleteUnit_Click(object sender, RoutedEventArgs e)
        {
            if (myUnit.Status == StatusBO.פעיל)
            {
                var orders = bl.GetOdrsOfHost(myUnit.Owner);
                if (orders != null)
                {
                    foreach (OrderBO item in orders)
                    {
                        if (item.HostingUnit.Key == myUnit.Key &&
                            (item.Status == OrderStatusBO.MAIL_SENT || item.Status == OrderStatusBO.PROCESSING))
                        {
                            MessageBox.Show("canot remove this unit because it has an open order");
                            return;
                        }

                    }
                }
                myUnit.Status = StatusBO.לא_פעיל;
                bl.UpdUnit(myUnit);
                
            }
            else if (myUnit.Status == StatusBO.לא_פעיל)
            {
                myUnit.Status = StatusBO.פעיל;
                bl.UpdUnit(myUnit);
            }
            UpdUnit();
        }
    }
}
