#region
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace SFMLStart.Utilities
{
    public class Timeline
    {
        private readonly Dictionary<string, SSTimelineCommand> _commandsLabeled;
        private float _frameTimeNext;

        public Timeline()
        {
            Commands = new List<SSTimelineCommand>();
            _commandsLabeled = new Dictionary<string, SSTimelineCommand>();
            Parameters = new Dictionary<string, object>();
        }

        internal bool Ready { get; set; }
        public SSTimelineCommand CommandCurrent { get; set; }
        public List<SSTimelineCommand> Commands { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public bool Finished { get; set; }

        public void AddCommand(SSTimelineCommand mCommand, string mLabel = null)
        {
            mCommand.Timeline = this;
            mCommand.Initialize();

            if (mLabel != null) _commandsLabeled[mLabel] = mCommand;

            Commands.Add(mCommand);
            if (CommandCurrent == null) CommandCurrent = mCommand;
        }

        public void RemoveCommand(SSTimelineCommand mCommand)
        {
            Debug.Assert(Commands.Count > 0);
            Debug.Assert(Commands.Contains(mCommand));

            Commands.Remove(mCommand);
        }

        public void NextCommand()
        {
            Debug.Assert(CommandCurrent != null);

            var index = Commands.IndexOf(CommandCurrent);

            CommandCurrent = Commands.Count > index + 1 ? Commands[index + 1] : null;
        }

        public void JumpToCommand(int mIndex)
        {
            Debug.Assert(Commands.Count > mIndex);

            CommandCurrent = Commands[mIndex];
        }

        public void JumpToCommand(string mLabel) { CommandCurrent = _commandsLabeled[mLabel]; }

        public void StepForward()
        {
            do
            {
                if (CommandCurrent == null)
                {
                    Finished = true;
                    Ready = false;
                    break;
                }

                CommandCurrent.Update();
            } while (Ready);
        }

        public void Update(float mFrameTime)
        {
            _frameTimeNext += mFrameTime;
            if (_frameTimeNext < 1) return;

            var remainder = _frameTimeNext - (int) Math.Floor(_frameTimeNext);

            if (Finished) return;
            Ready = true;

            for (var i = 0; i < Math.Floor(_frameTimeNext); i++) StepForward();

            _frameTimeNext = remainder;
        }

        public void Reset()
        {
            Finished = false;
            foreach (var command in Commands) command.Reset();
            CommandCurrent = Commands.Count > 0 ? Commands[0] : null;
        }

        public Timeline Clone()
        {
            var result = new Timeline();

            foreach (var command in Commands) result.Commands.Add(command.Clone());

            return result;
        }
    }

    public abstract class SSTimelineCommand
    {
        public Timeline Timeline { protected get; set; }
        public abstract void Initialize();
        public abstract SSTimelineCommand Clone();
        public abstract void Update();
        public abstract void Reset();
    }

    public class SSTCWait : SSTimelineCommand
    {
        public SSTCWait(int mFrames)
        {
            Variable = null;
            Frames = CurrentFrame = mFrames;
        }

        public SSTCWait(string mVariable) { Variable = mVariable; }

        public string Variable { get; set; }
        public int CurrentFrame { get; set; }
        public int Frames { get; set; }

        public override void Initialize() { if (Variable != null) Frames = CurrentFrame = (int) Timeline.Parameters[Variable]; }
        public override SSTimelineCommand Clone() { return new SSTCWait(Frames); }

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

    public class SSTCAction : SSTimelineCommand
    {
        public SSTCAction(Action mAction) { Action = mAction; }

        public Action Action { get; set; }

        public override void Initialize() { }
        public override SSTimelineCommand Clone() { return new SSTCAction((Action) Action.Clone()); }

        public override void Update()
        {
            Action.Invoke();
            Timeline.NextCommand();
        }

        public override void Reset() { }
    }

    public class SSTCGoto : SSTimelineCommand
    {
        public SSTCGoto(int mIndex, int mTimes = -1)
        {
            TargetIndex = mIndex;
            TargetLabel = null;
            Times = CurrentTimes = mTimes;
        }

        protected SSTCGoto(string mLabel, int mTimes = -1)
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
        public override SSTimelineCommand Clone() { return TargetLabel != null ? new SSTCGoto(TargetLabel, Times) : new SSTCGoto(TargetIndex, Times); }

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

    public class SSTCGotoConditional : SSTCGoto
    {
        public SSTCGotoConditional(Func<bool> mCondition, int mIndex, int mTimes) : base(mIndex, mTimes)
        {
            Condition = mCondition;
            TargetIndex = mIndex;
            TargetLabel = null;
            Times = CurrentTimes = mTimes;
        }

        public SSTCGotoConditional(Func<bool> mCondition, string mLabel, int mTimes) : base(mLabel, mTimes)
        {
            Condition = mCondition;
            TargetIndex = -1;
            TargetLabel = mLabel;
            Times = CurrentTimes = mTimes;
        }

        public Func<bool> Condition { get; set; }

        public override SSTimelineCommand Clone() { return TargetLabel != null ? new SSTCGotoConditional(Condition, TargetLabel, Times) : new SSTCGotoConditional(Condition, TargetIndex, Times); }

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