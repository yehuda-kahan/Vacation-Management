using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ClientBO
    {
        public Person PersonalInfo { get; set; }
        public List<GuestRequest> ClientRequests { get; set; }
    }
}
