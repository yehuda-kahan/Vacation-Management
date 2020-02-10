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
        static private string path = @"../../../../XMLFiles/HostXML.xml";
        public string HostPath { get { return path; } }
        XmlSerializer xs = new XmlSerializer(typeof(List<Host>));
        public void CreateHostFile()
        {
            FileStream fsout = new FileStream(HostPath, FileMode.Create);
            fsout.Close();
        }

        public void load()
        {
            FileStream fsin = new FileStream(HostPath, FileMode.Open);
            DalXml.hosts = (List<Host>)xs.Deserialize(fsin);
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
