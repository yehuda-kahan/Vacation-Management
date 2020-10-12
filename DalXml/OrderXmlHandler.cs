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
        public string OrderPath = @"OrderXML.xml";
        XmlSerializer xs = new XmlSerializer(DalXml.orders.GetType());
        public void CreateOrderFile()
        {
            
            FileStream fsout = new FileStream(OrderPath, FileMode.Create);
            xs.Serialize(fsout, new List<Order>());
            fsout.Close();
        }

        public void load()
        {
            FileStream fsin = new FileStream(OrderPath, FileMode.Open);
            DalXml.orders = new List<Order>();
            DalXml.orders = (List<Order>)xs.Deserialize(fsin);
            fsin.Close();
        }

        public void Save()
        {
            CreateOrderFile();
            FileStream fsout = new FileStream(OrderPath, FileMode.Create);
            xs.Serialize(fsout, DalXml.orders);
            fsout.Close();
        }
    }
}
