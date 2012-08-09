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

        public float X { get { return _x; } set { _x = value; } }
        public float Y { get { return _y; } set { _y = value; } }

        public bool Equals(SSVector2F other)
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
                return (int)(_x * 397) ^ (int)_y;
            }
        }
        public override string ToString() { return string.Format("X:{0} Y:{1}", _x, _y); }

        public static SSVector2F operator +(SSVector2F mVector1, SSVector2F mVector2I) { return new SSVector2F(mVector1.X + mVector2I.X, mVector1.Y + mVector2I.Y); }
        public static SSVector2F operator -(SSVector2F mVector1, SSVector2F mVector2I) { return new SSVector2F(mVector1.X - mVector2I.X, mVector1.Y - mVector2I.Y); }
        public static SSVector2F operator *(SSVector2F mVector, int mScalar) { return new SSVector2F(mVector.X * mScalar, mVector.Y * mScalar); }
        public static SSVector2F operator *(SSVector2F mVector, float mScalar) { return new SSVector2F((int)(mVector.X * mScalar), (int)(mVector.Y * mScalar)); }
        public static bool operator ==(SSVector2F mVector1, SSVector2F mVector2I) { return mVector1.X == mVector2I.X && mVector1.Y == mVector2I.Y; }
        public static bool operator !=(SSVector2F mVector1, SSVector2F mVector2I) { return mVector1.X != mVector2I.X || mVector1.Y != mVector2I.Y; }
        public static implicit operator SSVector2F(SSVector2I mVector2I) { return new SSVector2F(mVector2I.X, mVector2I.Y); }
    }
}