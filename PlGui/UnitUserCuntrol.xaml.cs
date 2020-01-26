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
       
        public UnitUserCuntrol(HostingUnitBO unit)
        {
            InitializeComponent();
            viewCalender.SelectionMode = CalendarSelectionMode.SingleRange;
            myUnit = unit;
            GridCalender.DataContext = myUnit;
            comArea.SelectedIndex = (int)myUnit.Area;
            //myUnit.Diary[0, 10] = true;
            //myUnit.Diary[0, 11] = true;
            Initilize_Calender_Detalse();
        }


        void Initilize_Calender_Detalse()
        {
            DateTime startDate = default;
            DateTime endDate;
            CalendarDateRange dateRange;
            bool StartFlag = false;

            for (int i = 0; i < 12; ++i)
            {
                for (int j = 0; j < 31; ++j)
                {
                    // for the first resevation day
                    if (StartFlag == false && myUnit.Diary[i, j] == true)
                    {
                        startDate = new DateTime(DateTime.Now.Year, i + 1, j + 1);
                        StartFlag = true;
                    }
                    // for the last resevation day
                    if (StartFlag == true && myUnit.Diary[i, j] == false)
                    {
                        endDate = new DateTime(DateTime.Now.Year, i + 1, j + 1).AddDays(-1);
                        dateRange = new CalendarDateRange(startDate, endDate);
                        viewCalender.BlackoutDates.Add(dateRange);
                        StartFlag = false;
                    }
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
    }
}
