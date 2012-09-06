#region
using System;
using SFMLStart.Utilities;

#endregion

namespace SFMLStart.Vectors
{
    public struct SSVector2F
    {
        private float _x;
        private float _y;

        public SSVector2F(float mX, float mY)
        {
            _x = mX;
            _y = mY;
        }
        public SSVector2F(SSVector2I mVector2I)
        {
            _x = mVector2I.X;
            _y = mVector2I.Y;
        }

        public float X { get { return _x; } set { _x = value; } }
        public float Y { get { return _y; } set { _y = value; } }

        public bool Equals(SSVector2F mVector) { return _x == mVector._x && _y == mVector._y; }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SSVector2I && Equals((SSVector2I) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int) (_x*397) ^ (int) _y;
            }
        }
        public override string ToString() { return string.Format("X:{0} Y:{1}", _x, _y); }

        #region Utilities
        public double GetDotProduct(SSVector2F mVector) { return X*mVector.X + Y*mVector.Y; }
        public double GetLength() { return Math.Sqrt(X*X + Y*Y); }
        public double GetAngleBetween(SSVector2F mVector)
        {
            var cos = GetDotProduct(mVector)/(GetLength()*mVector.GetLength());
            return Utils.Math.Angles.ToDegrees((float) Math.Acos(cos));
        }
        public SSVector2F GetNormalized() { return new SSVector2F((float) (X/GetLength()), (float) (Y/GetLength())); }
        public float ToAngleDegrees() { return Utils.Math.Angles.ToDegrees(ToAngleRadians()); }
        public float ToAngleRadians() { return (float) Math.Atan2(Y, X); }
        #endregion

        #region Operator Overloads
        public static SSVector2F operator +(SSVector2F mVector1, SSVector2F mVector2) { return new SSVector2F(mVector1.X + mVector2.X, mVector1.Y + mVector2.Y); }
        public static SSVector2F operator -(SSVector2F mVector1, SSVector2F mVector2) { return new SSVector2F(mVector1.X - mVector2.X, mVector1.Y - mVector2.Y); }
        public static SSVector2F operator *(SSVector2F mVector, int mScalar) { return new SSVector2F(mVector.X*mScalar, mVector.Y*mScalar); }
        public static SSVector2F operator *(SSVector2F mVector, float mScalar) { return new SSVector2F(mVector.X*mScalar, mVector.Y*mScalar); }
        public static SSVector2F operator /(SSVector2F mVector, int mScalar) { return new SSVector2F(mVector.X/mScalar, mVector.Y/mScalar); }
        public static SSVector2F operator /(SSVector2F mVector, float mScalar) { return new SSVector2F(mVector.X/mScalar, mVector.Y/mScalar); }
        public static bool operator ==(SSVector2F mVector1, SSVector2F mVector2) { return mVector1.X == mVector2.X && mVector1.Y == mVector2.Y; }
        public static bool operator !=(SSVector2F mVector1, SSVector2F mVector2) { return mVector1.X != mVector2.X || mVector1.Y != mVector2.Y; }
        public static explicit operator SSVector2I(SSVector2F mVector) { return new SSVector2I(mVector); }
        #endregion
    }
}