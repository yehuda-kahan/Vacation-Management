using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BO;

namespace PlGui
{
    public class EnumToHebrew : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value.GetType().Name == "StatusBO")
            {
                if ((StatusBO)value == StatusBO.ACTIVE)
                    return "פעיל";
                if ((StatusBO)value == StatusBO.NOT_ACTIVE)
                    return "לא פעיל";
            }
            if (value.GetType().Name == "IdTypesBO")
            {
                if ((IdTypesBO)value == IdTypesBO.DRIVING_LICENSE)
                    return "רשיון נהיגה";
                if ((IdTypesBO)value == IdTypesBO.ID)
                    return "ת.ז.";
                if ((IdTypesBO)value == IdTypesBO.PASSPORT)
                    return "דרכון";
            }
            if (value.GetType().Name == "RequestStatusBO")
            {
                if ((RequestStatusBO)value == RequestStatusBO.OPEN)
                    return "פתוח";
                if ((RequestStatusBO)value == RequestStatusBO.CANCELLED)
                    return "מבוטלת";
                if ((RequestStatusBO)value == RequestStatusBO.EXPIRED)
                    return "פג תוקף";
                if ((RequestStatusBO)value == RequestStatusBO.ORDERED)
                    return "הוזמן";
            }
            if (value.GetType().Name == "OrderStatusBO")
            {
                if ((OrderStatusBO)value == OrderStatusBO.APPROVED)
                    return "אושר";
                if ((OrderStatusBO)value == OrderStatusBO.CANCELED)
                    return "מבוטלת";
                if ((OrderStatusBO)value == OrderStatusBO.MAIL_SENT)
                    return "נשלח מייל";
                if ((OrderStatusBO)value == OrderStatusBO.NO_CLIENT_RESPONSE)
                    return "נסגר מחוסר תגובה";
                if ((OrderStatusBO)value == OrderStatusBO.PROCESSING)
                    return "בתהליך";
                if ((OrderStatusBO)value == OrderStatusBO.UNIT_NOT_AVALABELE)
                    return "יחידה לא זמינה";
            }
            if (value.GetType().Name == "AreaLocationBO")
            {
                if ((AreaLocationBO)value == AreaLocationBO.ALL)
                    return "הכל";
                if ((AreaLocationBO)value == AreaLocationBO.CENTER)
                    return "מרכז";
                if ((AreaLocationBO)value == AreaLocationBO.JERUSALEM)
                    return "ירושלים";
                if ((AreaLocationBO)value == AreaLocationBO.NORTH)
                    return "צפון";
                if ((AreaLocationBO)value == AreaLocationBO.SOUTH)
                    return "דרום";
            }
                return "";



        }



        //public enum AreaLocationBO { ALL, NORTH, SOUTH, CENTER, JERUSALEM }


        //public enum ThreeOptionsBO { YES, MAYBE, NO }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




    public class DateTimeToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString(format: "dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            return text;
        }
    }

    public class StringToUintConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return default;
            uint num = (uint)value;
            return num.ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return default;
            string str = (String)value;
            return uint.Parse(str);
        }
    }
}



