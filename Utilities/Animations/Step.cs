namespace SFMLStart.Utilities.Animations
{
    public class Step
    {
        public Step(int mX, int mY, int mFrames)
        {
            X = mX;
            Y = mY;
            Frames = mFrames;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Frames { get; set; }
    }
}