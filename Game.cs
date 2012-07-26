#region
using System;
using System.Linq;
using SFMLStart.Data;
using SFMLStart.Utilities;

#endregion

namespace SFMLStart
{
    public class Game
    {
        public GameWindow GameWindow { get; set; }
        public float GlobalInputDelay { get; set; }
        public Action<float> OnUpdate { get; set; }
        public Action OnDrawBeforeCamera { get; set; }
        public Action OnDrawAfterCamera { get; set; }
        public Action OnDrawAfterDefault { get; set; }

        public void DefineInput(string mInputName, int mInputDelay, Action mActionTrue, Action mActionFalse, InputCombination mInputCombination) { Assets.DefineInput(this, mInputName, mInputDelay, mActionTrue, mActionFalse, mInputCombination); }

        public void Update(float mFrameTime)
        {
            // Multiple input combinations should override single input combinations
            Assets.Inputs.Sort((b, a) => (a.Combination.Inputs.Count.CompareTo(b.Combination.Inputs.Count)));

            foreach (var dataInput in Assets.Inputs.Where(x => x.Game == this))
            {
                if (dataInput.CurrentDelay > 0) dataInput.CurrentDelay -= mFrameTime;

                if (GlobalInputDelay <= 0 && dataInput.CurrentDelay <= 0 &&
                    GameWindow.IsInputCombinationDown(dataInput.Combination))
                {
                    dataInput.ActionTrue.Invoke();
                    dataInput.CurrentDelay = dataInput.MaxDelay;
                    GlobalInputDelay = dataInput.GlobalDelay;

                    foreach (var input in Assets.Inputs.Where(x => dataInput.Combination.Inputs.Any(x.Combination.Inputs.Contains)))
                        input.CurrentDelay = dataInput.MaxDelay;
                }
                else if (dataInput.ActionFalse != null) dataInput.ActionFalse.Invoke();
                else if (!GameWindow.IsInputCombinationDown(dataInput.Combination)) dataInput.CurrentDelay = 0;
            }
            if (GlobalInputDelay > 0) GlobalInputDelay -= mFrameTime;

            if (OnUpdate != null) OnUpdate(mFrameTime);
        }

        public void Draw()
        {
            OnDrawBeforeCamera.SafeInvoke();
            GameWindow.RenderWindow.SetView(GameWindow.Camera.View);
            OnDrawAfterCamera.SafeInvoke();
            GameWindow.RenderWindow.SetView(GameWindow.RenderWindow.DefaultView);
            OnDrawAfterDefault.SafeInvoke();
        }
    }
}