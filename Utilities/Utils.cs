#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SFML.Graphics;
using SFML.Window;
using SFMLStart.Data;
using SFMLStart.Utilities.Timelines;
using SFMLStart.Vectors;
using Color = SFML.Graphics.Color;

#endregion

namespace SFMLStart.Utilities
{
    public static class Utils
    {
        private static readonly System.Random RandomGenerator = new System.Random();
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

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> mSource, Action<T> mAction)
        {
            if (mAction == null) throw new ArgumentNullException("mAction");
            foreach (var item in mSource) mAction(item);
            return mSource;
        }
        public static void SafeInvoke(this Action mAction) { if (mAction != null) mAction(); }
        public static Vertex[] GetVertexArray(List<SSVector2F> mPoints, float mMultiplier = 1, Color mColor = default(Color))
        {
            var result = new Vertex[mPoints.Count];

            for (var i = 0; i < mPoints.Count; i++)
                result[i] = new Vertex(new Vector2f(mPoints[i].X*mMultiplier, mPoints[i].Y*mMultiplier)) {Color = mColor};

            return result;
        }

        #region Timeline Shortcuts
        public static void Wait(this Timeline mTimeline, int mTime = 0) { mTimeline.AddCommand(new Wait(mTime)); }
        public static void Action(this Timeline mTimeline, Action mAction) { mTimeline.AddCommand(new Do(mAction)); }
        public static void Goto(this Timeline mTimeline, int mIndex = 0, int mTimes = -1) { mTimeline.AddCommand(new Goto(mIndex, mTimes)); }
        #endregion

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
                public static float TowardsRadians(SSVector2F mStart, SSVector2F mEnd) { return (float) System.Math.Atan2(mEnd.Y - mStart.Y, mEnd.X - mStart.X); }

                public static SSVector2F ToVectorDegrees(float mDegrees) { return new SSVector2F((float) System.Math.Cos(ToRadians(mDegrees)), (float) System.Math.Sin(ToRadians(mDegrees))); }
                public static SSVector2F ToVectorRadians(float mRadians) { return new SSVector2F((float) System.Math.Cos(mRadians), (float) System.Math.Sin(mRadians)); }

                public static float RotateTowardsAngleDegrees(float mStartDegrees, float mTargetDegrees, float mSpeed)
                {
                    if (System.Math.Abs(mStartDegrees - mTargetDegrees) < 0.01f) return mStartDegrees;
                    if (System.Math.Abs(((((mStartDegrees - mTargetDegrees)%360) + 540)%360) - 180) >= 179) return mStartDegrees + 1;
                    return WrapDegrees((float) (mStartDegrees + (System.Math.Sin((mTargetDegrees - mStartDegrees)/57.3f))*mSpeed));
                }

                public static float RotateTowardsTargetRadians(SSVector2F mStart, SSVector2F mEnd, float mStartRadians, float mSpeed)
                {
                    // the fuck is this?
                    var x = mEnd.X - mStart.X;
                    var y = mEnd.Y - mStart.Y;
                    var desiredAngle = (float) System.Math.Atan2(y, x);
                    var difference = WrapRadians(desiredAngle - mStartRadians);
                    return WrapRadians(mStartRadians + difference);
                }
            }
            #endregion

            #region Nested type: Collision
            public static class Collision
            {
                private static double Angle2D(double mX1, double mY1, double mX2, double mY2)
                {
                    var theta1 = System.Math.Atan2(mY1, mX1);
                    var theta2 = System.Math.Atan2(mY2, mX2);
                    var dtheta = theta2 - theta1;
                    while (dtheta > System.Math.PI) dtheta -= System.Math.PI*2;
                    while (dtheta < -System.Math.PI) dtheta += System.Math.PI*2;

                    return (dtheta);
                }

                public static bool IsPointInPolygon(IEnumerable<SSVector2F> mPolygonPoints, SSVector2F mPoint, float mDivisor = 1)
                {
                    // This is PNPOLY by W. Randolph Franklin
                    // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html

                    int i, j;
                    var c = false;
                    var p = mPoint/mDivisor;

                    var pointsToCheck = mPolygonPoints.Select(x => x/mDivisor).ToList();

                    for (i = 0, j = pointsToCheck.Count - 1; i < pointsToCheck.Count; j = i++)
                    {
                        var p1 = pointsToCheck[i];
                        var p2 = pointsToCheck[j];

                        if (((p1.Y >= p.Y) != (p2.Y >= p.Y)) && (p.X <= (p2.X - p1.X)*(p.Y - p1.Y)/(p2.Y - p1.Y) + p1.X)) c = !c;
                    }

                    return c;
                }

                public static bool IsPointInPolygon2(List<SSVector2F> mPolygonPoints, SSVector2F mPoint)
                {
                    int i;
                    double angle = 0;

                    for (i = 0; i < mPolygonPoints.Count; i++)
                    {
                        var p1 = new SSVector2F(mPolygonPoints[i].X - mPoint.X, mPolygonPoints[i].Y - mPoint.Y);
                        var p2 = new SSVector2F(mPolygonPoints[(i + 1)%mPolygonPoints.Count].X - mPoint.X,
                                                mPolygonPoints[(i + 1)%mPolygonPoints.Count].Y - mPoint.Y);
                        angle += Angle2D(p1.X, p1.Y, p2.X, p2.Y);
                    }

                    return !(System.Math.Abs(angle) < System.Math.PI);
                }
            }
            #endregion

            #region Nested type: Distances
            public static class Distances
            {
                public static int Manhattan(int mX1, int mY1, int mX2, int mY2) { return System.Math.Abs(mX1 - mX2) + System.Math.Abs(mY1 - mY2); }

                public static double Euclidean(int mX1, int mY1, int mX2, int mY2) { return System.Math.Sqrt((mX1 - mX2)*(mX1 - mX2) + (mY1 - mY2)*(mY1 - mY2)); }
                public static double Euclidean(float mX1, float mY1, float mX2, float mY2) { return System.Math.Sqrt((mX1 - mX2)*(mX1 - mX2) + (mY1 - mY2)*(mY1 - mY2)); }
                public static double Euclidean(double mX1, double mY1, double mX2, double mY2) { return System.Math.Sqrt((mX1 - mX2)*(mX1 - mX2) + (mY1 - mY2)*(mY1 - mY2)); }
            }
            #endregion

            #region Nested type: Vectors
            public static class Vectors
            {
                public static SSVector2F Lerp(SSVector2F mVector1, SSVector2F mVector2, float value)
                {
                    return new SSVector2F(mVector1.X + (mVector2.X - mVector1.X)*value,
                                          mVector1.Y + (mVector2.Y - mVector1.Y)*value);
                }
                public static SSVector2I Lerp(SSVector2I mVector1, SSVector2I mVector2, float value)
                {
                    return new SSVector2I((int) (mVector1.X + (mVector2.X - mVector1.X)*value),
                                          (int) (mVector1.Y + (mVector2.Y - mVector1.Y)*value));
                }

                public static SSVector2F OrbitRadians(SSVector2F mParent, float mRadians, float mRadius)
                {
                    return new SSVector2F((float) (mParent.X + System.Math.Cos((mRadians))*mRadius),
                                          (float) (mParent.Y + System.Math.Sin((mRadians))*mRadius));
                }
                public static SSVector2F OrbitDegrees(SSVector2F mParent, float mDegrees, float mRadius)
                {
                    return new SSVector2F((float) (mParent.X + System.Math.Cos(Angles.ToRadians(mDegrees))*mRadius),
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
        }
        #endregion

        #region Nested type: Random
        public static class Random
        {
            public static int Next(int mMin, int mMax) { return RandomGenerator.Next(mMin, mMax); }
            public static double NextDouble() { return RandomGenerator.NextDouble(); }
            public static int NextSign() { return RandomGenerator.Next(0, 50) > 25 ? 1 : -1; }
        }
        #endregion
    }
}