using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    class Customer
    {
        public Person PersonalInfo { get; set; }
        public List<GuestRequest> CustomerRequests { get; set; }
    }
}
