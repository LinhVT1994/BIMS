using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Utilities
{
    class Unility
    {
        private static Stopwatch watch = null;
        private static string message = null;
        public static void StartCountingTime(string message)
        {
            Unility.message = message;
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
                Debug.WriteLine("   Message: " + Unility.message+", Time to run: " + elapsedMs);
                watch.Stop();
            }
        }
    }
}
