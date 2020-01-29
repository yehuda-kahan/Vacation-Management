using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using BO;
using BlApi;

namespace PlConsole
{
    class Program
    {
        static IBl bl = BlFactory.GetBL();
        static void Main(string[] args)
        {
            Console.WriteLine(bl.GetPersonById("11223344"));
            // foreach(var item in bl.GetAvalableUnits(DateTime.Now, 5))
            //{
            //    Console.WriteLine(item.ToString());
            //}
            //PersonBO person = new PersonBO()
            //{
            //    Id = "204284376",
            //    Email = "yehudajka@gmail.com",
            //    IdType = IdTypesBO.ID,
            //    FirstName = "yehuda",
            //    LastName = "kahan",
            //    Password = "98765",
            //    PhoneNomber = "+972587271600",
            //    Status = StatusBO.ACTIVE
            //};
            //try
            //{
            //    bl.AddPerson(person);
            //}
            //catch (DuplicateWaitObjectException ex) { Console.WriteLine(ex.Message); }
            //Console.WriteLine(bl.GetPersonByMail("yehudajka@gmail.com"));
            //Console.WriteLine(bl.GetPersonById("204284376"));
            //try { Console.WriteLine(bl.GetRequest(20000002).ToString()); }
            //catch (MissingMemberException ex) { Console.WriteLine(ex); }
            HostingUnitBO unit = new HostingUnitBO
            {
                Key = 11116,
                Owner = "11223344",
                HostingUnitName = "villa",
                Diary = new bool[12, 31],
                Status = StatusBO.פעיל,
                Area = AreaLocationBO.CENTER
            };
            uint a = new uint();
            try { a = bl.AddUnit(unit); }
            catch (DuplicateKeyException ex) { Console.WriteLine(ex); }
            Console.WriteLine(a);
        }
    }
}
