using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public static class Indexer
    {
        public static void SetDates(this HostingUnit unit, DateTime date)
        {
            unit.Diary[date.Day - 1, date.Month - 1] = true;
        }

        public static bool GetDates(this HostingUnit unit, DateTime date)
        {
            return unit.Diary[date.Day - 1, date.Month - 1];
        }
    }
}
