using System;

namespace SFMLStart.Utilities.Timelines
{
    public class GotoConditional : Goto
    {
        public GotoConditional(Func<bool> mCondition, int mIndex, int mTimes) : base(mIndex, mTimes)
        {
            Condition = mCondition;
            TargetIndex = mIndex;
            TargetLabel = null;
            Times = CurrentTimes = mTimes;
        }

        public GotoConditional(Func<bool> mCondition, string mLabel, int mTimes) : base(mLabel, mTimes)
        {
            Condition = mCondition;
            TargetIndex = -1;
            TargetLabel = mLabel;
            Times = CurrentTimes = mTimes;
        }

        public Func<bool> Condition { get; set; }

        public override Command Clone() { return TargetLabel != null ? new GotoConditional(Condition, TargetLabel, Times) : new GotoConditional(Condition, TargetIndex, Times); }

        public override void Update()
        {
            if (Condition.Invoke() ||
                Times == 0) Timeline.NextCommand();
            else
            {
                if (TargetLabel != null) Timeline.JumpToCommand(TargetLabel);
                else Timeline.JumpToCommand(TargetIndex);

                Times--;
            }
        }

        public override void Reset() { CurrentTimes = Times; }
    }
}