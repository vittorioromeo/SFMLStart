#region
using System.Runtime.InteropServices;

#endregion

namespace SFMLStart.Utilities
{
    public class PerformanceStopwatch
    {
        private static readonly double Frequency = GetFrequency();
        private long _start;
        private long _stop;

        public double Elapsed { get { return (_stop - _start)/Frequency; } }

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        // static - so this value used in all instances of 

        // Note this is static- called once, before any constructor!
        private static double GetFrequency()
        {
            long tempfrequency;
            QueryPerformanceFrequency(out tempfrequency);
            return tempfrequency; // implicit casting to double from long
        }

        public void Start() { QueryPerformanceCounter(out _start); }
        public void Stop() { QueryPerformanceCounter(out _stop); }
    }
}