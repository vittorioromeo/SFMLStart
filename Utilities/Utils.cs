#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SFML.Window;
using SFMLStart.Data;
using SFMLStart.Utilities.Timelines;
using SFMLStart.Vectors;

#endregion

namespace SFMLStart.Utilities
{
    public static class Utils
    {
        private static readonly Random Random = new Random();
        public static Stopwatch Stopwatch = new Stopwatch();

        public static void Log(string mString, string mTitle = null, ConsoleColor mColor = ConsoleColor.Cyan)
        {
            if (!Settings.Logging.IsEnabled) return;

            Console.ForegroundColor = mColor;

            if (mTitle != null)
            {
                mTitle = mTitle.Insert(0, "[");
                mTitle = mTitle.Insert(mTitle.Length, "]");
            }

            Console.Write("{0, 1} ", mTitle);
            Console.ResetColor();
            Console.WriteLine(mString);
        }

        public static void Wait(this Timeline mTimeline, int mTime = 0) { mTimeline.AddCommand(new Wait(mTime)); }
        public static void Action(this Timeline mTimeline, Action mAction) { mTimeline.AddCommand(new Do(mAction)); }
        public static void Goto(this Timeline mTimeline, int mIndex = 0, int mTimes = -1) { mTimeline.AddCommand(new Goto(mIndex, mTimes)); }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> mSource, Action<T> mAction)
        {
            if (mAction == null)
                throw new ArgumentNullException("mAction");

            foreach (var item in mSource)
                mAction(item);

            return mSource;
        }

        public static void SafeInvoke(this Action mAction) { if (mAction != null) mAction(); }

        #region Nested type: Assets
        public static class Assets
        {
            public static void SetSoundsVolume(int mVolume) { foreach (var keyValuePair in Data.Assets.Sounds) keyValuePair.Value.Volume = mVolume; }
        }
        #endregion

        #region Nested type: Dialogs
        public static class Dialogs
        {
            public static string InputBox(string title, string promptText, string mDefaultText = "")
            {
                var form = new Form();
                var uneditableTextBox = new TextBox();
                var textBox = new TextBox();
                var buttonOk = new Button();
                var buttonCancel = new Button();

                form.Text = title;
                uneditableTextBox.Multiline = true;
                uneditableTextBox.Enabled = false;
                uneditableTextBox.Text = promptText;
                textBox.Text = mDefaultText;

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                uneditableTextBox.SetBounds(9, 20, 372, 250);
                textBox.SetBounds(12, 280, 372, 20);
                buttonOk.SetBounds(228, 320, 75, 23);
                buttonCancel.SetBounds(309, 320, 75, 23);

                uneditableTextBox.AutoSize = true;
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new Size(396, 350);
                form.Controls.AddRange(new Control[] {uneditableTextBox, textBox, buttonOk, buttonCancel});
                form.ClientSize = new Size(System.Math.Max(300, uneditableTextBox.Right + 10), form.ClientSize.Height);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                var dialogResult = form.ShowDialog();

                return dialogResult == DialogResult.Cancel ? null : textBox.Text;
            }

            public static string InputListBox(string title, IEnumerable<string> items)
            {
                var form = new Form();
                var uneditableTextBox = new ListBox();
                var buttonOk = new Button();
                var buttonCancel = new Button();

                form.Text = title;

                foreach (var item in items) uneditableTextBox.Items.Add(item);

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                uneditableTextBox.SetBounds(9, 20, 372, 250);
                buttonOk.SetBounds(228, 265, 75, 23);
                buttonCancel.SetBounds(309, 265, 75, 23);

                uneditableTextBox.AutoSize = true;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new Size(396, 300);
                form.Controls.AddRange(new Control[] {uneditableTextBox, buttonOk, buttonCancel});
                form.ClientSize = new Size(System.Math.Max(300, uneditableTextBox.Right + 10), form.ClientSize.Height);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                var dialogResult = form.ShowDialog();

                if (dialogResult == DialogResult.Cancel ||
                    uneditableTextBox.SelectedIndex == -1) return null;

                return uneditableTextBox.SelectedItem.ToString();
            }
        }
        #endregion

        #region Nested type: Math
        public static class Math
        {
            public static float Lerp(float mValue1, float mValue2, float mWeight) { return mValue1 + mWeight*(mValue2 - mValue1); }

            public static T Clamp<T>(T value, T max, T min) where T : IComparable<T>
            {
                var result = value;
                if (value.CompareTo(max) > 0) result = max;
                if (value.CompareTo(min) < 0) result = min;
                return result;
            }

            #region Nested type: Angles
            public static class Angles
            {
                public static float ToDegrees(float mRadians) { return mRadians*57.3f; }
                public static float ToRadians(float mDegrees) { return mDegrees/57.3f; }

                public static float WrapDegrees(float mDegrees)
                {
                    while (mDegrees < 0) mDegrees += 360;
                    while (mDegrees > 360) mDegrees -= 360;
                    return mDegrees;
                }

                public static float WrapRadians(float mRadians)
                {
                    while (mRadians < 3.14f) mRadians += 6.24f;
                    while (mRadians > 3.14f) mRadians -= 6.24f;
                    return mRadians;
                }

                public static float TowardsDegrees(SSVector2F mStart, SSVector2F mEnd) { return ToDegrees(TowardsRadians(mStart, mEnd)); }
                public static float TowardsRadians(SSVector2F mStart, SSVector2F mEnd) { return (float)System.Math.Atan2(mEnd.Y - mStart.Y, mEnd.X - mStart.X); }
                public static SSVector2F ToVectorDegrees(float mDegrees) { return new SSVector2F((float)System.Math.Cos(ToRadians(mDegrees)), (float)System.Math.Sin(ToRadians(mDegrees))); }
                public static SSVector2F ToVectorRadians(float mRadians) { return new SSVector2F((float)System.Math.Cos(mRadians), (float)System.Math.Sin(mRadians)); }

                public static float RotateTowardsAngleDegrees(float mAngle, float mAngleDesired, float mSpeed)
                {
                    if (System.Math.Abs(mAngle - mAngleDesired) < 0.01f) return mAngle;
                    if (System.Math.Abs(((((mAngle - mAngleDesired)%360) + 540)%360) - 180) >= 179) return mAngle + 1;
                    return WrapDegrees((float) (mAngle + (System.Math.Sin((mAngleDesired - mAngle)/57.3f))*mSpeed));
                }

                public static float RotateTowardsTargetRadians(SSVector2F mStart, SSVector2F mTarget, float mCurrentRadians, float mTurnSpeed)
                {
                    var x = mTarget.X - mStart.X;
                    var y = mTarget.Y - mStart.Y;
                    var desiredAngle = (float) System.Math.Atan2(y, x);
                    var difference = WrapRadians(desiredAngle - mCurrentRadians);
                    return WrapRadians(mCurrentRadians + difference);
                }
            }
            #endregion

            #region Nested type: Distances
            public static class Distances
            {
                public static int Manhattan(int mX1, int mY1, int mX2, int mY2) { return System.Math.Abs(mX1 - mX2) + System.Math.Abs(mY1 - mY2); }
                public static double Euclidean(int mX1, int mY1, int mX2, int mY2) { return System.Math.Sqrt((mX1 - mX2)*(mX1 - mX2) + (mY1 - mY2)*(mY1 - mY2)); }
            }
            #endregion

            #region Nested type: Vectors
            public static class Vectors
            {
                public static int DotProduct(SSVector2I v1, SSVector2I v2)
                {
                    return
                        (
                            v1.X*v2.X +
                            v1.Y*v2.Y
                        );
                }

                public static int Length(SSVector2I mVector) { return (int)System.Math.Sqrt(mVector.X * mVector.X + mVector.Y * mVector.Y); }
                public static SSVector2I Normalize(SSVector2I mVector) { return new SSVector2I(mVector.X / Length(mVector), mVector.Y / Length(mVector)); }
                public static long NormalizeLong(SSVector2I mVector) { return mVector.X * mVector.X + mVector.Y * mVector.Y; }
                public static float ToAngleDegrees(SSVector2F mVector) { return Angles.ToDegrees(ToAngleRadians(mVector)); }
                public static float ToAngleRadians(SSVector2F mVector) { return (float)System.Math.Atan2(mVector.Y, mVector.X); }

                


                public static SSVector2F Lerp(SSVector2F mVector1, SSVector2F mVector2, float value)
                {
                    return new SSVector2F(mVector1.X + (mVector2.X - mVector1.X) * value,
                                        mVector1.Y + (mVector2.Y - mVector1.Y)*value);
                }

                public static SSVector2I Lerp(SSVector2I mVector1, SSVector2I mVector2, float value)
                {
                    return new SSVector2I((int)(mVector1.X + (mVector2.X - mVector1.X) * value),
                                        (int) (mVector1.Y + (mVector2.Y - mVector1.Y)*value));
                }

                public static SSVector2F OrbitRadians(SSVector2F mParent, float mRadians, float mRadius)
                {
                    return new SSVector2F((float)(mParent.X + System.Math.Cos((mRadians)) * mRadius),
                                        (float) (mParent.Y + System.Math.Sin((mRadians))*mRadius));
                }

                public static SSVector2F OrbitDegrees(SSVector2F mParent, float mDegrees, float mRadius)
                {
                    return new SSVector2F((float)(mParent.X + System.Math.Cos(Angles.ToRadians(mDegrees)) * mRadius),
                                        (float) (mParent.Y + System.Math.Sin(Angles.ToRadians(mDegrees))*mRadius));
                }

                public static SSVector2I RotatePointRadians(SSVector2I mPoint, SSVector2I mOrigin, float mRadians)
                {
                    var s = (float) System.Math.Sin(mRadians);
                    var c = (float) System.Math.Cos(mRadians);

                    // translate point back to origin:
                    mPoint.X -= mOrigin.X;
                    mPoint.Y -= mOrigin.Y;

                    // rotate point
                    var xnew = mPoint.X*c - mPoint.Y*s;
                    var ynew = mPoint.X*s + mPoint.Y*c;

                    // translate point back:
                    mPoint.X = (int) (xnew + mOrigin.X);
                    mPoint.Y = (int) (ynew + mOrigin.Y);

                    return mPoint;
                }
            }
            #endregion

            #region Nested type: ToDeprecate
            public static class ToDeprecate
            {
                public static Vector2f Lerp(Vector2f mVector1, Vector2f mVector2, float value)
                {
                    return new Vector2f(mVector1.X + (mVector2.X - mVector1.X) * value,
                                        mVector1.Y + (mVector2.Y - mVector1.Y) * value);
                }
            }
            #endregion
        }
        #endregion

        #region Nested type: RandomGenerator
        public static class RandomGenerator
        {
            public static int GetNextInt(int mMin, int mMax) { return Random.Next(mMin, mMax); }
            public static double GetNextDouble() { return Random.NextDouble(); }
        }
        #endregion
    }
}