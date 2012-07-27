namespace SFMLStart.Utilities.Timelines
{
    public abstract class Command
    {
        public Timeline Timeline { protected get; set; }
        public abstract void Initialize();
        public abstract Command Clone();
        public abstract void Update();
        public abstract void Reset();
    }
}