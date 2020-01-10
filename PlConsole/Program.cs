using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;

namespace PlConsole
{
    class Program
    {
      
        static IBl bb = BlFactory.GetBL();
       

        static void Main(string[] args)
        {
            Console.WriteLine(bb.GetPerson("11223344"));
           }
    }
}
