﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Person
    {
        public string Id { get; set; }
        public IdTypes IdType { get; set; }
        public Status Status { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNomber { get; set; }

        public override string ToString()
        {
            return "ID : " + Id
                + "\nLast name : " + LastName
                + "\nFirst name : " + FirstName
                + "\nEmail : " + Email
                + "\nPhone nomber : " + PhoneNomber
                + "\n";
        }
    }
}
