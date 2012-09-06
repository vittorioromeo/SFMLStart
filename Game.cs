#region
using System;
using System.Collections.Generic;
using System.Linq;
using SFMLStart.Data;
using SFMLStart.Utilities;

#endregion

namespace SFMLStart
{
    public class Game
    {
        private readonly List<DrawAction> _drawActions;

        public Game() { _drawActions = new List<DrawAction>(); }

        public GameWindow GameWindow { get; internal set; }
        public Action<float> OnUpdate { get; set; }
        public Action OnDrawBeforeCamera { get; set; }
        public Action OnDrawAfterDefault { get; set; }
        public float GlobalInputDelay { get; set; }

        public void Bind(string mBindName, int mBindDelay, Action mActionTrue, Action mActionFalse, KeyCombination mKeyCombination) { Input.Bind(this, mBindName, mBindDelay, mActionTrue, mActionFalse, mKeyCombination); }

        public void AddDrawAction(Action mAction, int mPriority = 0) { _drawActions.Add(new DrawAction(mAction, mPriority)); }
        public void RemoveDrawAction(Action mAction) { _drawActions.Remove(_drawActions.First(x => x.Action == mAction)); }

        public void Update(float mFrameTime)
        {
            // Multiple input combinations should override single input combinations
            Input.Binds.Sort((b, a) => (a.KeyCombination.Inputs.Count.CompareTo(b.KeyCombination.Inputs.Count)));

            foreach (var dataInput in Input.Binds.Where(x => x.Game == this))
            {
                if (dataInput.CurrentDelay > 0) dataInput.CurrentDelay -= mFrameTime;

                if (GlobalInputDelay <= 0 && dataInput.CurrentDelay <= 0 &&
                    GameWindow.IsInputCombinationDown(dataInput.KeyCombination))
                {
                    dataInput.ActionTrue.Invoke();
                    dataInput.CurrentDelay = dataInput.MaxDelay;
                    GlobalInputDelay = dataInput.GlobalDelay;

                    foreach (
                        var input in
                            Input.Binds.Where(x => dataInput.KeyCombination.Inputs.Any(x.KeyCombination.Inputs.Contains))
                        )
                        input.CurrentDelay = dataInput.MaxDelay;
                }
                else if (dataInput.ActionFalse != null) dataInput.ActionFalse.Invoke();
                else if (!GameWindow.IsInputCombinationDown(dataInput.KeyCombination)) dataInput.CurrentDelay = 0;
            }
            if (GlobalInputDelay > 0) GlobalInputDelay -= mFrameTime;

            if (OnUpdate != null) OnUpdate(mFrameTime);
        }

        public void Draw()
        {
            OnDrawBeforeCamera.SafeInvoke();
            GameWindow.RenderWindow.SetView(GameWindow.Camera.View);
            foreach (var drawAction in _drawActions.OrderBy(x => x.Priority)) drawAction.Action.SafeInvoke();
            GameWindow.RenderWindow.SetView(GameWindow.RenderWindow.DefaultView);
            OnDrawAfterDefault.SafeInvoke();
        }
    }
}