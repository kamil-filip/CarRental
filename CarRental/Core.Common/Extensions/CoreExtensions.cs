using Core.Common.Core;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Extensions
{
    public static class CoreExtensions
    {
        static Dictionary<string, bool> BrowsableProperties = new Dictionary<string, bool>();
        static Dictionary<string, PropertyInfo[]> BrowsablePropertyInfos = new Dictionary<string, PropertyInfo[]>();

        public static bool IsBrowsable(this object obj, PropertyInfo property)
        {
            string key = string.Format("{0}.{1}", obj.GetType(), property.Name);

            if (!BrowsableProperties.ContainsKey(key))
            {
                bool browsable = property.IsNavigable();
                BrowsableProperties.Add(key, browsable);
            }

            return BrowsableProperties[key];
        }

        public static bool IsNavigable(this PropertyInfo property)
        {
            bool navigable = true;

            object[] attributes = property.GetCustomAttributes(typeof(NotNavigableAttribute), true);
            if (attributes.Length > 0)
                navigable = false;

            return navigable;
        }

        public static bool IsNavigable<T>(this ObjectBase obj, Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo.IsNavigable();
        }

        public static bool IsNavigable(this ObjectBase obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo.IsNavigable();
        }

    }
}
