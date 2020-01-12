using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    static class Cloning
    {
        internal static T Clone<T>(this T original)
        {
            T target = (T)Activator.CreateInstance(original.GetType());
            var infos = original.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var item in infos)
            {
                if (item.FieldType.IsValueType || item.FieldType.Equals(typeof(string)))
                    item.SetValue(target, item.GetValue(original));
                else if (item.FieldType.IsArray)
                {
                    item.SetValue(target, ((Array)(item.GetValue(original))).Clone());
                }
                else
                {
                    item.SetValue(target, item.GetValue(original).Clone());
                }
            }
            return target;
        }
    }
}

