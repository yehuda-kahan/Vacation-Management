using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DO;

namespace Dal
{
    public class HostinhUnitXml
    {
        public uint Key { get; set; }
        public string Owner { get; set; }   // ID of the Owner Host 
        public string HostingUnitName { get; set; }
        [XmlIgnore]
        public bool[,] Diary { set; get; }

        [XmlArray("Diary")]
        public bool[] DiaryDto
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(12); }
        }
        public AreaLocation Area { get; set; }
        public Status Status { get; set; }

        public override string ToString()
        {
            return "Hosting unit Key : " + Key
                + "\nHosting unit name : " + HostingUnitName
                + "\nOwner ID: " + Owner
                + "\n";
        }
    }

    public static class Tools
    {
        public static T[] Flatten<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            T[] arrFlattened = new T[rows * columns];
            for (int j = 0; j < columns; j++) for (int i = 0; i < rows; i++)
                {
                    var test = arr[i, j];
                    arrFlattened[i + j * rows] = arr[i, j];
                }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int j = 0; j < rows; j++)
                for (int i = 0; i < columns; i++)
                    arrExpanded[i, j] = arr[i + j * rows];
            return arrExpanded;
        }
    }

}
