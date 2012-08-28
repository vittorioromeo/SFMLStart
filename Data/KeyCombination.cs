#region
using System.Collections.Generic;

#endregion

namespace SFMLStart.Data
{
    public class KeyCombination
    {
        public KeyCombination(params object[] mInputs)
        {
            Inputs = new List<string>();
            foreach (var obj in mInputs) Inputs.Add(obj.GetType() + obj.ToString());
        }

        public List<string> Inputs { get; private set; }
    }
}