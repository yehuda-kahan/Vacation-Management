﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class ClientBO
    {
        public PersonBO PersonalInfo { get; set; }
        public IEnumerable<GuestRequestBO> ClientRequests { get; set; }
    }
}
