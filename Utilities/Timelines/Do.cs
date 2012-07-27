using System;

namespace SFMLStart.Utilities.Timelines
{
    public class Do : Command
    {
        public Do(Action mAction) { Action = mAction; }

        public Action Action { get; set; }

        public override void Initialize() { }
        public override Command Clone() { return new Do((Action)Action.Clone()); }

        public override void Update()
        {
            Action.Invoke();
            Timeline.NextCommand();
        }

        public override void Reset() { }
    }
}