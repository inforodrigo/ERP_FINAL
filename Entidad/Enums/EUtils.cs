using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.Enums
{
    public class EUtils
    {
        public static DateTime NowDate
        {
            get { return TimeZoneInfo.ConvertTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time")); }
        }
    }
}
