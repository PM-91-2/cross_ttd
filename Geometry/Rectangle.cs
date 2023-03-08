using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

namespace Geometry {
    public class Rectangle : IFigure {
        private List<Vector2> _points;
        private Vector2 _position;

        public string PathData {
            get {
                return string.Format("M {0},{1} L {2},{3} {4},{5} {6},{7} Z",
                    _points[0].X, _points[0].Y, _points[1].X, _points[1].Y,
                    _points[3].X, _points[3].Y, _points[2].X, _points[2].Y);
            }
        }

        public Rectangle(Vector2 point1, Vector2 point2) {
            _points = new List<Vector2> {
                point1,
                point2,
                new Vector2(point1.X, point2.Y),
                new Vector2(point2.X, point1.Y)
            };

            // Порядок точек: левая верхняя, левая нижняя, правая верхняя, правая нижняя
            _points = _points.OrderBy(v => v.X).ThenByDescending(v => v.Y).ToList();
            _position = _points[0];
        }

        public bool IsPointInFigure(Vector2 point, float eps) {
            var point1 = GeometryUtils.RectangleSideProduct(point, _points[0], _points[1]);
            var point2 = GeometryUtils.RectangleSideProduct(point, _points[1], _points[3]);
            var point3 = GeometryUtils.RectangleSideProduct(point, _points[3], _points[2]);
            var point4 = GeometryUtils.RectangleSideProduct(point, _points[2], _points[0]);

            return point1 > 0 && point2 > 0 && point3 > 0 && point4 > 0;
        }

        public void Move(Vector2 startPosition, Vector2 newPosition) {
            var moveVector = new Vector2(newPosition.X - startPosition.X,
                newPosition.Y - startPosition.Y);

            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(_points[i].X + moveVector.X, _points[i].Y + moveVector.Y);
        }

        public void Rotate(float angle) {
            float angleConvert = (float)Math.PI * angle / 180;
            List<Vector2> rotateMatrix = new List<Vector2> {
                new Vector2((float)Math.Cos(angleConvert), -(float)Math.Sin(angleConvert)),
                new Vector2((float)Math.Sin(angleConvert), (float)Math.Cos(angleConvert))
            };

            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(
                    (rotateMatrix[0].X * (_points[i].X - _position.X) +
                     rotateMatrix[0].Y * (_points[i].Y - _position.Y) + _position.X),
                    (rotateMatrix[1].X * (_points[i].X - _position.X) +
                     rotateMatrix[1].Y * (_points[i].Y - _position.Y) + _position.Y));
        }

        public void Scale(float scaleX, float scaleY) {
            Vector2 center = Vector2.Multiply(0.5f, _points[3] - _points[0]);

            for (int i = 0; i < 4; i++) _points[i] -= center;
            for (int i = 0; i < 4; i++) _points[i] = new Vector2(_points[i].X * scaleX, _points[i].Y * scaleY);
            for (int i = 0; i < 4; i++) _points[i] += center;
        }
    }
}