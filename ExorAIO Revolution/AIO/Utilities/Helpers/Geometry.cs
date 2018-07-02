using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.ThirdParty;

namespace AIO.Utilities
{
    using Path = List<IntPoint>;
    using Paths = List<List<IntPoint>>;

    /// <summary>
    ///     Class that contains the geometry related methods.
    /// </summary>
    public static class Geometry
    {
        #region Constants

        private const int CircleLineSegmentN = 22;

        #endregion

        #region Public Methods and Operators

        public static Paths ClipPolygons(List<Polygon> polygons)
        {
            var subj = new Paths(polygons.Count);
            var clip = new Paths(polygons.Count);
            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }

            var solution = new Paths();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);
            return solution;
        }

        public static void DrawCircleOnMinimap(
            Vector3 center,
            float radius,
            Color color,
            int thickness = 1,
            int quality = 100)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(
                    new Vector3(
                        center.X + radius * (float)Math.Cos(angle),
                        center.Y,
                        center.Z + radius * (float)Math.Sin(angle))
                );
            }
            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                Render.WorldToMinimap(a, out Vector2 aonScreen);
                Render.WorldToMinimap(b, out Vector2 bonScreen);

                Render.Line(aonScreen, bonScreen, color);
            }
        }

        public static bool IsInside(this Vector3 point, Polygon poly)
        {
            return !point.IsOutside(poly);
        }

        public static bool IsInside(this Vector2 point, Polygon poly)
        {
            return !point.IsOutside(poly);
        }

        public static bool IsOutside(this Vector3 point, Polygon poly)
        {
            var p = new IntPoint(point.X, point.Y);
            return Clipper.PointInPolygon(p, poly.ToClipperPath()) != 1;
        }

        public static bool IsOutside(this Vector2 point, Polygon poly)
        {
            var p = new IntPoint(point.X, point.Y);
            return Clipper.PointInPolygon(p, poly.ToClipperPath()) != 1;
        }

        public static bool Intersects(this Rectangle rectangle, Circle circle)
        {
            var rectangleHalfWidth = rectangle.Width / 2;
            var cx = Math.Abs(circle.Center.X - rectangle.End.X - rectangleHalfWidth);
            var xDist = rectangleHalfWidth + circle.Radius;
            if (cx > xDist)
            {
                return false;
            }

            var rectangleHalfHeight = rectangle.Start.Distance(rectangle.End) / 2;
            var cy = Math.Abs(circle.Center.Y - rectangle.End.Y - rectangleHalfHeight);
            var yDist = rectangleHalfHeight + circle.Radius;
            if (cy > yDist)
            {
                return false;
            }

            if (cx <= rectangleHalfWidth || cy <= rectangleHalfHeight)
            {
                return true;
            }

            var xCornerDist = cx - rectangleHalfWidth;
            var yCornerDist = cy - rectangleHalfHeight;

            var xCornerDistSq = xCornerDist * xCornerDist;
            var yCornerDistSq = yCornerDist * yCornerDist;

            var maxCornerDistSq = circle.Radius * circle.Radius;
            return xCornerDistSq + yCornerDistSq <= maxCornerDistSq;
        }

        /// <summary>
        ///     Returns the position on the path after t milliseconds at speed speed.
        /// </summary>
        public static Vector2 PositionAfter(this List<Vector2> self, int t, int speed, int delay = 0)
        {
            var distance = Math.Max(0, t - delay) * speed / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int)to.Distance(from);
                if (d > distance)
                {
                    return from + distance * (to - from).Normalized();
                }

                distance -= d;
            }

            return self[self.Count - 1];
        }

        public static Vector3 SwitchZy(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        /// <summary>
        ///     Converts a Vector3 to Vector2
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static Vector2 To2D(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static Polygon ToPolygon(this Path v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector3(point.X, point.Y, point.Z));
            }

            return polygon;
        }

        //Clipper
        public static List<Polygon> ToPolygons(this Paths v)
        {
            return v.Select(path => path.ToPolygon()).ToList();
        }

        #endregion

        public static class Util
        {
            #region Public Methods and Operators

            public static void DrawLineInWorld(Vector3 start, Vector3 end, int width, Color color)
            {
                Render.WorldToScreen(start, out Vector2 from);
                Render.WorldToScreen(end, out Vector2 to);
                Render.Line(from, to, color);
            }

            #endregion
        }

        public class Circle
        {
            #region Fields

            public Vector3 Center;

            public float Radius;

            #endregion

            #region Constructors and Destructors

            public Circle(Vector3 center, float radius)
            {
                Center = center;
                Radius = radius;
            }

            #endregion

            #region Public Methods and Operators

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();
                var outRadius = overrideWidth > 0 ? overrideWidth : (offset + Radius) / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN);
                for (var i = 1; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector3(
                        Center.X + outRadius * (float)Math.Cos(angle),
                        Center.Y,
                        Center.Z + outRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }

            #endregion
        }

        public class Polygon
        {
            #region Fields

            public List<Vector3> Points = new List<Vector3>();

            #endregion

            #region Public Methods and Operators

            public void Add(Vector3 point)
            {
                Points.Add(point);
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    var nextIndex = Points.Count - 1 == i ? 0 : i + 1;
                    Util.DrawLineInWorld(Points[i], Points[nextIndex], width, color);
                }
            }

            public bool IsInside(Vector2 point)
            {
                return !IsOutside(point);
            }

            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, ToClipperPath()) != 1;
            }

            public List<IntPoint> ToClipperPath()
            {
                var result = new List<IntPoint>(Points.Count);
                result.AddRange(Points.Select(point => new IntPoint(point.X, point.Y)));

                return result;
            }

            #endregion
        }

        /// <summary>
        ///     Represents a rectangle polygon.
        /// </summary>
        public class Rectangle : Polygon
        {
            #region Fields

            /// <summary>
            ///     The end
            /// </summary>
            public Vector3 End;

            /// <summary>
            ///     The start
            /// </summary>
            public Vector3 Start;

            /// <summary>
            ///     The width
            /// </summary>
            public float Width;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Rectangle" /> class.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="end">The end.</param>
            /// <param name="width">The width.</param>
            public Rectangle(Vector3 start, Vector3 end, float width)
            {
                Start = start;
                End = end;
                Width = width;
                UpdatePolygon();
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Gets the direction.
            /// </summary>
            /// <value>
            ///     The direction.
            /// </value>
            public Vector3 Direction()
            {
                return (End - Start).Normalized();
            }

            /// <summary>
            ///     Gets the perpendicular.
            /// </summary>
            /// <value>
            ///     The perpendicular.
            /// </value>
            public Vector3 Perpendicular()
            {
                return Direction().Perpendicular();
            }

            /// <summary>
            ///     Updates the polygon.
            /// </summary>
            /// <param name="offset">The offset.</param>
            /// <param name="overrideWidth">Width of the override.</param>
            public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
            {
                var getWidth = (overrideWidth > 0 ? overrideWidth : Width + offset);
                var pos = offset * Direction();

                Points.Clear();
                Points.Add(Start + getWidth * Perpendicular() - pos);
                Points.Add(Start - getWidth * Perpendicular() - pos);
                Points.Add(End - getWidth * Perpendicular() + pos);
                Points.Add(End + getWidth * Perpendicular() + pos);
            }

            #endregion
        }

        public class Ring
        {
            #region Fields

            public Vector3 Center;

            public float Radius;

            public float RingRadius; //actually radius width.

            #endregion

            #region Constructors and Destructors

            public Ring(Vector3 center, float radius, float ringRadius)
            {
                Center = center;
                Radius = radius;
                RingRadius = ringRadius;
            }

            #endregion

            #region Public Methods and Operators

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();
                var outRadius = (offset + Radius + RingRadius) / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN);
                var innerRadius = Radius - RingRadius - offset;
                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector3(
                        Center.X - outRadius * (float)Math.Cos(angle),
                        Center.Y,
                        Center.Z - outRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }
                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector3(
                        Center.X + innerRadius * (float)Math.Cos(angle),
                        Center.Y,
                        Center.Z - innerRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }

            #endregion
        }

        /// <summary>
        ///     Represnets a sector polygon.
        /// </summary>
        public class Sector : Polygon
        {
            #region Fields

            /// <summary>
            ///     The angle
            /// </summary>
            public float Angle;

            /// <summary>
            ///     The center
            /// </summary>
            public Vector3 Center;

            /// <summary>
            ///     The direction
            /// </summary>
            public Vector3 Direction;

            /// <summary>
            ///     The radius
            /// </summary>
            public float Radius;

            /// <summary>
            ///     The quality
            /// </summary>
            private readonly int _quality;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sector" /> class.
            /// </summary>
            /// <param name="center">The center.</param>
            /// <param name="direction">The direction.</param>
            /// <param name="angle">The angle.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="quality">The quality.</param>
            public Sector(Vector3 center, Vector3 direction, float angle, float radius, int quality = 20)
            {
                Center = center;
                Direction = (direction - center).Normalized();
                Angle = angle;
                Radius = radius;
                _quality = quality;
                UpdatePolygon();
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Rotates Line by angle/radian
            /// </summary>
            /// <param name="point1"></param>
            /// <param name="point2"></param>
            /// <param name="value"></param>
            /// <param name="radian">True for radian values, false for degree</param>
            /// <returns></returns>
            public Vector2 RotateLineFromPoint(Vector2 point1, Vector2 point2, float value, bool radian = true)
            {
                var angle = !radian ? value * Math.PI / 180 : value;
                var line = Vector2.Subtract(point2, point1);
                var newline = new Vector2
                {
                    X = (float)(line.X * Math.Cos(angle) - line.Y * Math.Sin(angle)),
                    Y = (float)(line.X * Math.Sin(angle) + line.Y * Math.Cos(angle))
                };
                return Vector2.Add(newline, point1);
            }

            /// <summary>
            ///     Updates the polygon.
            /// </summary>
            /// <param name="offset">The offset.</param>
            public void UpdatePolygon(int offset = 0)
            {
                Points.Clear();
                var outRadius = (Radius + offset) / (float)Math.Cos(2 * Math.PI / _quality);
                Points.Add(Center);
                var side1 = Direction.Rotated(-Angle * 0.5f);
                for (var i = 0; i <= _quality; i++)
                {
                    var cDirection = side1.Rotated(i * Angle / _quality).Normalized();
                    Points.Add(
                        new Vector3(
                            Center.X + outRadius * cDirection.X,
                            Center.Y,
                            Center.Z + outRadius * cDirection.Z));
                }
            }

            #endregion
        }
    }
}