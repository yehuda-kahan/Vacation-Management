using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public static class Cloning
    {
        internal static T Clone<T>(this T original)
        {
            T target = (T)Activator.CreateInstance(original.GetType());
            PropertyInfo[] infos;

            infos = original.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var item in infos)
            {
                if (item.PropertyType.IsValueType || item.PropertyType.Equals(typeof(string)))
                    item.SetValue(target, item.GetValue(original));
                //else
                //{
                
                //}
            }
            return target;
        }
    }
}

