using System;
using System.Collections.Generic;
using SFMLStart.Utilities;

namespace SFMLStart.Data
{
    public static class Input
    {
        static Input() { Binds = new List<Bind>(); }
        public static List<Bind> Binds { get; private set; }

        public static void Bind(Game mGame, string mBindName, int mBindDelay, Action mActionTrue, Action mActionFalse, params KeyCombination[] mInputs)
        {
            foreach (var obj in mInputs)
            {
                Binds.Add(new Bind(mGame, mBindName, mBindDelay, obj, mActionTrue,
                                   mActionFalse));

                Utils.Log(string.Format("<<{0}>> defined", mBindName), "Bind", ConsoleColor.Red);
            }
        }
    }
}