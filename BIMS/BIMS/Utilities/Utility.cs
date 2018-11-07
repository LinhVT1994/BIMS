using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Utilities
{
    /**
    * Utility class contains methods what is usually used.
    * 
    *
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
    class Utility
    {
        private static Stopwatch watch = null;
        private static string message = null;
        public static void StartCountingTime(string message)
        {
            Utility.message = message;
            if (watch == null)
            {
                watch = System.Diagnostics.Stopwatch.StartNew();
            }
            if (!watch.IsRunning)
            {
                watch.Start();
            }
        }
       
        public static void StopCountingTime()
        {
            if (watch.IsRunning)
            {
                var elapsedMs = watch.ElapsedMilliseconds;
                Debug.WriteLine("   Message: " + Utility.message+", Time to run: " + elapsedMs);
                watch.Stop();
            }
        }
    }
}
