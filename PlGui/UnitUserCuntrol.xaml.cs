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

        public UnitUserCuntrol(HostingUnitBO unit)
        {
            InitializeComponent();
            myUnit = unit;
            Calendar myCalender = Initilize_Calender_Detalse();
            viewCalender.Child = myCalender;

        }
        Calendar Initilize_Calender_Detalse()
        {
            DateTime startDate = default;
            DateTime endDate;

            CalendarDateRange dateRange;
            Calendar calendar = new Calendar();
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
                        endDate = new DateTime(DateTime.Now.Year, i + 1, j + 1);
                        dateRange = new CalendarDateRange(startDate, endDate);
                        calendar.BlackoutDates.Add(dateRange);
                        StartFlag = false;
                    }
                }
            }
            return calendar;
        }

    }
}
