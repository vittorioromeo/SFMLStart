namespace SFMLStart.Utilities.Animations
{
    public class Step
    {
        public Step(string mLabel, int mFrames)
        {
            Label = mLabel;
            Frames = mFrames;
        }

        public int Frames { get; private set; }
        public string Label { get; private set; }
    }
}