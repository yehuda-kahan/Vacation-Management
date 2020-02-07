﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DO;

namespace Dal
{
    public class PersonXmlHandler
    {
        static private string path = @"../../../../XMLFiles/PersonPath.xml";
        public string PersonPath { get { return path; } }
        XmlSerializer xs = new XmlSerializer(typeof(List<Person>));
        public void CreatePersonFile()
        {
            FileStream fsout = new FileStream(PersonPath, FileMode.Create);
            fsout.Close();
        }

        public void load()
        {
            FileStream fsin = new FileStream(path, FileMode.Open);
            DalXml.persons = (List<Person>)xs.Deserialize(fsin);
            fsin.Close();
        }

        public void Save()
        {
            FileStream fsout = new FileStream(path, FileMode.Open);
            xs.Serialize(fsout, DalXml.persons);
            fsout.Close();
        }
    }
}
