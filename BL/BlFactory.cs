using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    internal static class BlFactory
    {
        internal static IBl GetBL()
        {
            return new BL.BlImp();
        }
    }
}

