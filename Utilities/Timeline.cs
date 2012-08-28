#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SFMLStart.Utilities.Timelines;

#endregion

namespace SFMLStart.Utilities
{
    public class Timeline
    {
        private readonly Dictionary<string, Command> _commandsLabeled;
        private float _frameTimeNext;

        public Timeline()
        {
            Commands = new List<Command>();
            _commandsLabeled = new Dictionary<string, Command>();
            Parameters = new Dictionary<string, object>();
        }

        internal bool Ready { get; set; }
        public Command CommandCurrent { get; set; }
        public List<Command> Commands { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public bool Finished { get; set; }

        public void AddCommand(Command mCommand, string mLabel = null)
        {
            mCommand.Timeline = this;
            mCommand.Initialize();

            if (mLabel != null) _commandsLabeled[mLabel] = mCommand;

            Commands.Add(mCommand);
            if (CommandCurrent == null) CommandCurrent = mCommand;
        }
        public void RemoveCommand(Command mCommand)
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
}