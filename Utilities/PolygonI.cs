#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
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
        public Vertex[] GetVertexArray(float mMultiplier = 1, Color mColor = default(Color))
        {
            var result = new Vertex[_points.Count];

            for (var i = 0; i < _points.Count; i++)
            {
                var point = _points[i];
                result[i] = new Vertex(new Vector2f(point.X*mMultiplier, point.Y*mMultiplier)) {Color = mColor};
            }

            return result;
        }

        public bool IsIntersecting(SSVector2I mPoint, float mDivisor = 1)
        {
            // This is PNPOLY by W. Randolph Franklin
            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html

            int i, j;
            var c = false;
            var p = new SSVector2F(mPoint)/mDivisor;

            for (i = 0, j = _points.Count - 1; i < _points.Count; j = i++)
            {
                var p1 = new SSVector2F(_points[i])/mDivisor;
                var p2 = new SSVector2F(_points[j])/mDivisor;

                if (((p1.Y > p.Y) != (p2.Y > p.Y)) && (p.X < (p2.X - p1.X)*(p.Y - p1.Y)/(p2.Y - p1.Y) + p1.X)) c = !c;
            }

            return c;
        }

        public bool IsIntersecting2(SSVector2I mPoint)
        {
            int i;
            double angle = 0;

            for (i = 0; i < _points.Count; i++)
            {
                var p1 = new SSVector2I(_points[i].X - mPoint.X, _points[i].Y - mPoint.Y);
                var p2 = new SSVector2I(_points[(i + 1)%_points.Count].X - mPoint.X,
                                        _points[(i + 1)%_points.Count].Y - mPoint.Y);
                angle += Angle2D(p1.X, p1.Y, p2.X, p2.Y);
            }

            return !(Math.Abs(angle) < Math.PI);
        }

        private static double Angle2D(double mX1, double mY1, double mX2, double mY2)
        {
            var theta1 = Math.Atan2(mY1, mX1);
            var theta2 = Math.Atan2(mY2, mX2);
            var dtheta = theta2 - theta1;
            while (dtheta > Math.PI) dtheta -= Math.PI*2;
            while (dtheta < -Math.PI) dtheta += Math.PI*2;

            return (dtheta);
        }
    }
}