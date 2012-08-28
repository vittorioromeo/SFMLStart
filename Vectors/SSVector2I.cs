#region
using System;
using SFMLStart.Utilities;

#endregion

namespace SFMLStart.Vectors
{
    public struct SSVector2I
    {
        private int _x;
        private int _y;

        public SSVector2I(int mX, int mY)
        {
            _x = mX;
            _y = mY;
        }
        public SSVector2I(SSVector2F mVector2F)
        {
            _x = (int) mVector2F.X;
            _y = (int) mVector2F.Y;
        }

        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public bool Equals(SSVector2I mVector) { return _x == mVector._x && _y == mVector._y; }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SSVector2I && Equals((SSVector2I) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_x*397) ^ _y;
            }
        }
        public override string ToString() { return string.Format("X:{0} Y:{1}", _x, _y); }
        #region Utilities
        public double GetDotProduct(SSVector2I mVector) { return X*mVector.X + Y*mVector.Y; }
        public double GetLength() { return Math.Sqrt(X*X + Y*Y); }
        public double GetAngleBetween(SSVector2I mVector)
        {
            var cos = GetDotProduct(mVector)/(GetLength()*mVector.GetLength());
            return Utils.Math.Angles.ToDegrees((float) Math.Acos(cos));
        }
        public SSVector2I GetNormalized() { return new SSVector2I((int) (X/GetLength()), (int) (Y/GetLength())); }
        public float ToAngleDegrees() { return Utils.Math.Angles.ToDegrees(ToAngleRadians()); }
        public float ToAngleRadians() { return (float) Math.Atan2(Y, X); }
        #endregion
        #region Operator Overloads
        public static SSVector2I operator +(SSVector2I mVector1, SSVector2I mVector2) { return new SSVector2I(mVector1.X + mVector2.X, mVector1.Y + mVector2.Y); }
        public static SSVector2I operator -(SSVector2I mVector1, SSVector2I mVector2) { return new SSVector2I(mVector1.X - mVector2.X, mVector1.Y - mVector2.Y); }
        public static SSVector2I operator *(SSVector2I mVector, int mScalar) { return new SSVector2I(mVector.X*mScalar, mVector.Y*mScalar); }
        public static SSVector2I operator *(SSVector2I mVector, float mScalar) { return new SSVector2I((int) (mVector.X*mScalar), (int) (mVector.Y*mScalar)); }
        public static SSVector2I operator /(SSVector2I mVector, int mScalar) { return new SSVector2I(mVector.X/mScalar, mVector.Y/mScalar); }
        public static SSVector2I operator /(SSVector2I mVector, float mScalar) { return new SSVector2I((int) (mVector.X/mScalar), (int) (mVector.Y/mScalar)); }
        public static bool operator ==(SSVector2I mVector1, SSVector2I mVector2) { return mVector1.X == mVector2.X && mVector1.Y == mVector2.Y; }
        public static bool operator !=(SSVector2I mVector1, SSVector2I mVector2) { return mVector1.X != mVector2.X || mVector1.Y != mVector2.Y; }
        public static implicit operator SSVector2F(SSVector2I mVector) { return new SSVector2F(mVector); }
        #endregion
    }
}