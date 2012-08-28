#region
using System;

#endregion

namespace SFMLStart.Data
{
    public class Bind
    {
        public Bind(Game mGame, string mName, float mDelay, KeyCombination mCombination, Action mActionTrue,
                    Action mActionFalse, float mGlobalDelay = 0)
        {
            Game = mGame;
            Name = mName;
            KeyCombination = mCombination;
            MaxDelay = mDelay;
            ActionTrue = mActionTrue;
            ActionFalse = mActionFalse;
            GlobalDelay = mGlobalDelay;
        }

        public Game Game { get; set; }
        public string Name { get; set; }
        public KeyCombination KeyCombination { get; set; }
        public float GlobalDelay { get; set; }
        public float MaxDelay { get; set; }
        public float CurrentDelay { get; set; }
        public Action ActionTrue { get; set; }
        public Action ActionFalse { get; set; }
    }
}