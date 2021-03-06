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
    public class UnitsXmlHandler
    {
        public string UnitsPath = @"HostingUnitXML.xml";
        List<HostinhUnitXml> hostinhUnitXmls;
        XmlSerializer xs = new XmlSerializer(typeof(List<HostinhUnitXml>));

        HostinhUnitXml ConverterTioHostingUnitXml(HostingUnit unit)
        {
            HostinhUnitXml target = new HostinhUnitXml();
            target.Diary = unit.Diary;
            target.HostingUnitName = unit.HostingUnitName;
            target.Key = unit.Key;
            target.Owner = unit.Owner;
            target.Status = unit.Status;
            return target;
        }
        HostingUnit convertToHostingUnitDo(HostinhUnitXml unitXml)
        {
            HostingUnit target = new HostingUnit();
            target.Diary = unitXml.Diary;
            target.HostingUnitName = unitXml.HostingUnitName;
            target.Key = unitXml.Key;
            target.Owner = unitXml.Owner;
            target.Status = unitXml.Status;
            return target;
        }
        public void CreateUnitFile()
        {
            FileStream fsout = new FileStream(UnitsPath, FileMode.Create);
           hostinhUnitXmls = new List<HostinhUnitXml>();
            xs.Serialize(fsout, hostinhUnitXmls);
            fsout.Close();
        }

        public void load()
        {
            DalXml.hostingUnits = new List<HostingUnit>();
            FileStream fsin = new FileStream(UnitsPath, FileMode.Open);
            List<HostinhUnitXml> temp = (List<HostinhUnitXml>)xs.Deserialize(fsin);
            foreach (HostinhUnitXml item in temp)
                DalXml.hostingUnits.Add(convertToHostingUnitDo(item));
            fsin.Close();
        }

        public void Save()
        {
            CreateUnitFile();
            FileStream fsout = new FileStream(UnitsPath, FileMode.Create);
            foreach (HostingUnit item in DalXml.hostingUnits)
                hostinhUnitXmls.Add(ConverterTioHostingUnitXml(item));
            xs.Serialize(fsout, hostinhUnitXmls);
            fsout.Close();
        }
    }
}
