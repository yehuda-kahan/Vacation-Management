using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DO;

namespace Dal
{
    public class OrderXmlHandler
    {
        static private string path = @"../../../../XMLFiles/OrderXML.xml";
        public string OrderPath { get { return path; } }
        XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
        public void CreateOrderFile()
        {
            FileStream fsout = new FileStream(OrderPath, FileMode.Create);
            fsout.Close();
        }

        public void load()
        {
            FileStream fsin = new FileStream(OrderPath, FileMode.Open);
            DalXml.orders = (List<Order>)xs.Deserialize(fsin);
            fsin.Close();
        }

        public void Save()
        {
            FileStream fsout = new FileStream(OrderPath, FileMode.Open);
            xs.Serialize(fsout, DalXml.orders);
            fsout.Close();
        }
    }
}
