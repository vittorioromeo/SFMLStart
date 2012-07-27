namespace SFMLStart.Utilities.Timelines
{
    public class Wait : Command
    {
        public Wait(int mFrames)
        {
            Variable = null;
            Frames = CurrentFrame = mFrames;
        }

        public Wait(string mVariable) { Variable = mVariable; }

        public string Variable { get; set; }
        public int CurrentFrame { get; set; }
        public int Frames { get; set; }

        public override void Initialize() { if (Variable != null) Frames = CurrentFrame = (int) Timeline.Parameters[Variable]; }
        public override Command Clone() { return new Wait(Frames); }

        public override void Update()
        {
            Timeline.Ready = false;
            if (Variable != null) Frames = (int) Timeline.Parameters[Variable];

            CurrentFrame--;
            if (CurrentFrame > 0) return;

            Timeline.NextCommand();
            Reset();
        }

        public override void Reset() { CurrentFrame = Frames; }
    }
}