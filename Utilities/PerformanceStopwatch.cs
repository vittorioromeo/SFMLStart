#region
using System.Runtime.InteropServices;
using System.Timers;
#endregion

namespace SFMLStart.Utilities
{
    public class PerformanceStopwatch
    {
		Timer _timer = new Timer(1);
		private long _acc = 0;

		public PerformanceStopwatch()
		{
			_timer.Elapsed += (sender, e) => ++_acc;
		}

		public double Elapsed { get { return _acc; } }
		public void Start() { _acc = 0; _timer.Start(); }
		public void Stop() { _timer.Stop();  }
    }
}