using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU
{
    internal static class ConvertExtentions
    {
        internal static double AsDouble(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(double);

            double result;
            if (!double.TryParse(obj, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out result))
                throw new ArgumentException(string.Format("Invalid value for double. Value: {0}", obj));
            return result;
        }

        internal static int AsInt(this object item, int defaultInt = default(int))
        {
            if (item == null)
                return defaultInt;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultInt;

            return result;
        }
    }
}
