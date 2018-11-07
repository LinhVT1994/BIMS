using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Utilities
{
    class LoggingHelper
    {
        public static bool WriteDown(string containt)
        {
            Debug.WriteLine(containt);
            // add the datetime is written down.
            return false;
        }
    }
}
