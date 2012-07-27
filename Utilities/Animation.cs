#region
using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFMLStart.Utilities.Animations;

#endregion

namespace SFMLStart.Utilities
{
    public class Animation
    {
        private float _frameTimeNext;
        private bool _isBackwards;

        public Animation(Tileset mTileset, bool mIsLooped = true, bool mIsPingPong = false)
        {
            Debug.Assert(mTileset != null);

            Tileset = mTileset;
            IsLooped = mIsLooped;
            IsPingPong = mIsPingPong;
            Steps = new List<Step>();
        }

        public List<Step> Steps { get; set; }
        public Step CurrentStep { get; set; }
        public int CurrentFrame { get; set; }
        public Tileset Tileset { get; set; }
        public bool IsLooped { get; set; }
        public bool IsPingPong { get; set; }

        public void Reset()
        {
            if (Steps.Count <= 0) return;
            _isBackwards = false;
            CurrentStep = Steps[0];
            CurrentFrame = 0;
        }

        public void AddStep(int mTileX, int mTileY, int mFrames)
        {
            Steps.Add(new Step(mTileX, mTileY, mFrames));
            if (CurrentStep == null) CurrentStep = Steps[0];
        }

        public void AddStep(string mTilesetLabel, int mFrames)
        {
            Steps.Add(new Step(Tileset.Labels[mTilesetLabel].X, Tileset.Labels[mTilesetLabel].Y, mFrames));
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

            for (var i = 0; i < _frameTimeNext; i++) StepForward();

            _frameTimeNext = 0;
        }

        public void StepForward()
        {
            if (CurrentStep == null) Reset();
            if (CurrentStep == null) return;

            if (CurrentFrame == CurrentStep.Frames) NextStep();
            else CurrentFrame++;
        }

        public IntRect GetCurrentSubRect() { return CurrentStep != null ? Tileset.GetTextureRect(CurrentStep.X, CurrentStep.Y) : new IntRect(0, 0, 0, 0); }
    }
}