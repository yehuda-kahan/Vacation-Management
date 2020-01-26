using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BO
{
    public static class IndexerBO
    {
        public static void SetDates(this HostingUnitBO unit, DateTime date)
        {
            unit.Diary[date.Month - 1, date.Day - 1] = true;
        }

        public  static bool GetDates(this HostingUnitBO unit, DateTime date)
        {
            return unit.Diary[date.Month - 1, date.Day - 1];
        }
    }
}
