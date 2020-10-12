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
    public class GuestRequestXmlHandler
    {

        public  string GuestRequestPath = @"GuestRequestXML.xml";
        XmlSerializer xs = new XmlSerializer(DalXml.guestRequests.GetType());
        public void CreateGuestRequestFile()
        {
            FileStream fsout = new FileStream(GuestRequestPath, FileMode.Create);
            xs.Serialize(fsout, DalXml.guestRequests);   
            fsout.Close();
        }

        public void load()
        {
            FileStream fsin = new FileStream(GuestRequestPath, FileMode.Open);
            DalXml.guestRequests = (List<GuestRequest>)xs.Deserialize(fsin);
            fsin.Close();
        }

        public void Save()
        {
            FileStream fsout = new FileStream(GuestRequestPath, FileMode.Create);
            xs.Serialize(fsout, DalXml.guestRequests);
            fsout.Close();
        }
    }
}


