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
    public class HostXmlHandler
    {
        public  string HostPath = "HostXML.xml";

        XmlSerializer xs = new XmlSerializer(typeof(List<Host>));
        public void CreateHostFile()
        {
            FileStream fsout = new FileStream(HostPath, FileMode.Create);
            xs.Serialize(fsout, DalXml.hosts);
            fsout.Close();
        }

        public void load()
        {
            FileStream fsin = new FileStream(HostPath, FileMode.Open);
            List<Host> temoHosts = (List<Host>)xs.Deserialize(fsin);
            DalXml.hosts = temoHosts;
            fsin.Close();
        }

        public void Save()
        {
            FileStream fsout = new FileStream(HostPath, FileMode.Open);
            xs.Serialize(fsout, DalXml.hosts);
            fsout.Close();
        }
    }
}
