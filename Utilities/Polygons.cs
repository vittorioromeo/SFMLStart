#region
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFMLStart.Vectors;

#endregion

namespace SFMLStart.Utilities
{
    public class PolygonI : IEnumerable<SSVector2I>
    {
        private readonly List<SSVector2I> _points;

        public PolygonI(params SSVector2I[] mPoints) { _points = mPoints.ToList(); }

        #region IEnumerable<SSVector2I> Members
        public IEnumerator<SSVector2I> GetEnumerator() { return _points.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public Vertex[] GetVertexArray(float mMultiplier = 1, Color mColor = default(Color)) { return Utils.GetVertexArray(_points.Select(x => new SSVector2F(x.X, x.Y)).ToList(), mMultiplier, mColor); }

        public bool IsIntersecting(SSVector2I mPoint, float mDivisor = 1)
        {
            return Utils.Math.Collision.IsPointInPolygon(_points.Select(x => new SSVector2F(x.X, x.Y)), mPoint,
                                                         mDivisor);
        }
        public bool IsIntersecting2(SSVector2F mPoint) { return Utils.Math.Collision.IsPointInPolygon2(_points.Select(x => new SSVector2F(x.X, x.Y)).ToList(), mPoint); }
    }

    public class PolygonF : IEnumerable<SSVector2F>
    {
        private readonly List<SSVector2F> _points;

        public PolygonF(params SSVector2F[] mPoints) { _points = mPoints.ToList(); }

        #region IEnumerable<SSVector2F> Members
        public IEnumerator<SSVector2F> GetEnumerator() { return _points.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public Vertex[] GetVertexArray(float mMultiplier = 1, Color mColor = default(Color)) { return Utils.GetVertexArray(_points, mMultiplier, mColor); }

        public bool IsIntersecting(SSVector2F mPoint, float mDivisor = 1) { return Utils.Math.Collision.IsPointInPolygon(this, mPoint, mDivisor); }
        public bool IsIntersecting2(SSVector2F mPoint) { return Utils.Math.Collision.IsPointInPolygon2(_points, mPoint); }
    }
}