using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace PlConsole
{
    class Program
    {

        static IBl bl = BlFactory.GetBL();


        static void Main(string[] args)
        {
            // Console.WriteLine(bl.GetPerson("11223344"));
            // foreach(var item in bl.GetAvalableUnits(DateTime.Now, 5))
            //{
            //    Console.WriteLine(item.ToString());
            //}
            PersonBO person = new PersonBO()
            {
                Id = "204284376",
                Email = "yehudajka@gmail.com",
                IdType = IdTypesBO.ID,
                FirstName = "yehuda",
                LastName = "kahan",
                Password = "98765",
                PhoneNomber = "+972587271600",
                Status = StatusBO.ACTIVE
            };
            try
            {
                bl.AddPerson(person);
            }
            catch (DuplicateWaitObjectException ex) { Console.WriteLine(ex.Message); }
            //Console.WriteLine(bl.GetPersonByMail("yehudajka@gmail.com"));
            //Console.WriteLine(bl.GetPersonById("204284376"));
            
        }
    }
}
