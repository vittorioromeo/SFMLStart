namespace SFMLStart.Utilities.Timelines
{
    public class Goto : Command
    {
        public Goto(int mIndex, int mTimes = -1)
        {
            TargetIndex = mIndex;
            TargetLabel = null;
            Times = CurrentTimes = mTimes;
        }

        protected Goto(string mLabel, int mTimes = -1)
        {
            TargetIndex = -1;
            TargetLabel = mLabel;
            Times = CurrentTimes = mTimes;
        }

        public int TargetIndex { get; set; }
        public string TargetLabel { get; set; }
        public int CurrentTimes { get; set; }
        public int Times { get; set; }

        public override void Initialize() { }

        public override Command Clone() { return TargetLabel != null ? new Goto(TargetLabel, Times) : new Goto(TargetIndex, Times); }

        public override void Update()
        {
            if (Times == 0) Timeline.NextCommand();
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