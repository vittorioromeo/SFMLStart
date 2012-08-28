#region
using System;
using System.Collections.Generic;
using SFMLStart.Utilities.Animations;

#endregion

namespace SFMLStart.Utilities
{
    public class Animation
    {
        private float _frameTimeNext;
        private bool _isBackwards;

        public Animation(bool mIsLooped = true, bool mIsPingPong = false)
        {
            IsLooped = mIsLooped;
            IsPingPong = mIsPingPong;
            Steps = new List<Step>();
        }

        public List<Step> Steps { get; set; }
        public Step CurrentStep { get; set; }
        public int CurrentFrame { get; set; }
        public bool IsLooped { get; set; }
        public bool IsPingPong { get; set; }

        public void Reset()
        {
            if (Steps.Count <= 0) return;
            _isBackwards = false;
            CurrentStep = Steps[0];
            CurrentFrame = 0;
        }

        public void AddStep(string mLabel, int mFrames)
        {
            var step = new Step(mLabel, mFrames);
            Steps.Add(step);
            if (CurrentStep == null) CurrentStep = Steps[0];
        }

        private void NextStep()
        {
            if (!_isBackwards)
            {
                if (Steps.Count >
                    Steps.IndexOf(CurrentStep) + 1)
                {
                    CurrentStep = Steps[Steps.IndexOf(CurrentStep) + 1];
                    CurrentFrame = 0;
                }
                else if (IsLooped)
                {
                    if (!IsPingPong)
                    {
                        CurrentStep = Steps[0];
                        CurrentFrame = 0;
                    }
                    else
                    {
                        _isBackwards = true;
                        CurrentFrame = 0;
                    }
                }
            }
            else
            {
                if (Steps.IndexOf(CurrentStep) - 1 >= 0)
                {
                    CurrentStep = Steps[Steps.IndexOf(CurrentStep) - 1];
                    CurrentFrame = 0;
                }
                else if (IsLooped)
                {
                    if (!IsPingPong)
                    {
                        CurrentStep = Steps[Steps.Count - 1];
                        CurrentFrame = 0;
                    }
                    else
                    {
                        _isBackwards = false;
                        CurrentFrame = 0;
                    }
                }
            }
        }

        public void Update(float mFrameTime)
        {
            _frameTimeNext += mFrameTime;
            if (_frameTimeNext < 1) return;
            var remainder = _frameTimeNext - (int) Math.Floor(_frameTimeNext);

            for (var i = 0; i < _frameTimeNext; i++) StepForward();

            _frameTimeNext = remainder;
        }

        public void StepForward()
        {
            if (CurrentStep == null) Reset();
            if (CurrentStep == null) return;

            if (CurrentFrame == CurrentStep.Frames) NextStep();
            else CurrentFrame++;
        }
        public string GetCurrentLabel() { return CurrentStep != null ? CurrentStep.Label : null; }

        public Animation Clone()
        {
            var result = new Animation(IsLooped, IsPingPong);
            result.Steps = new List<Step>(Steps);
            result.CurrentStep = result.Steps[0];
            return result;
        }
    }
}