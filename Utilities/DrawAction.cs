#region
using System;

#endregion

namespace SFMLStart.Utilities
{
    internal class DrawAction
    {
        internal DrawAction(Action mAction, int mPriority)
        {
            Action = mAction;
            Priority = mPriority;
        }

        internal Action Action { get; set; }
        internal int Priority { get; set; }
    }
}