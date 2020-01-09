using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    static class IndexerBO
    {
        internal static void SetDates(this HostingUnitBO unit, DateTime date)
        {
            unit.Diary[date.Day - 1, date.Month - 1] = true;
        }

        internal static bool GetDates(this HostingUnitBO unit, DateTime date)
        {
            return unit.Diary[date.Day - 1, date.Month - 1];
        }
    }
}
