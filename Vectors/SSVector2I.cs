using System;
using SFMLStart.Utilities;

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

        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public bool Equals(SSVector2I other)
        {
            return _x == other._x && _y == other._y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SSVector2I && Equals((SSVector2I)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return (_x * 397) ^ _y;
            }
        }
        public override string ToString() { return string.Format("X:{0} Y:{1}", _x, _y); }

        public double GetDotProduct(SSVector2I mVector)
        {
            return X*mVector.X + Y*mVector.Y;
        }
        public double GetLength()
        {
            return Math.Sqrt(X*X + Y*Y);
        }
        public double GetAngleBetween(SSVector2I mVector)
        {
            var cos = GetDotProduct(mVector)/(GetLength()*mVector.GetLength());
            return Utils.Math.Angles.ToDegrees((float) Math.Acos(cos));
        }

        public static SSVector2I operator +(SSVector2I mVector1, SSVector2I mVector2I) { return new SSVector2I(mVector1.X + mVector2I.X, mVector1.Y + mVector2I.Y); }
        public static SSVector2I operator -(SSVector2I mVector1, SSVector2I mVector2I) { return new SSVector2I(mVector1.X - mVector2I.X, mVector1.Y - mVector2I.Y); }
        public static SSVector2I operator *(SSVector2I mVector, int mScalar) { return new SSVector2I(mVector.X * mScalar, mVector.Y * mScalar); }
        public static SSVector2I operator *(SSVector2I mVector, float mScalar) { return new SSVector2I((int) (mVector.X*mScalar), (int) (mVector.Y*mScalar)); }
        public static bool operator ==(SSVector2I mVector1, SSVector2I mVector2I) { return mVector1.X == mVector2I.X && mVector1.Y == mVector2I.Y; }
        public static bool operator !=(SSVector2I mVector1, SSVector2I mVector2I) { return mVector1.X != mVector2I.X || mVector1.Y != mVector2I.Y; }
        public static implicit operator SSVector2I(SSVector2F mVector2I) { return new SSVector2I((int) mVector2I.X, (int) mVector2I.Y); }
    }
}