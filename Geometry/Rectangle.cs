using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

namespace Geometry {
    public class Rectangle : IFigure {
        private List<Vector2> _points;
        private List<Vector2> _bounds;
        private float _angle = 0f;
        private float boundEps = 10;

        public string PathData {
            get {
                return string.Format("M {0},{1} L {2},{3} {4},{5} {6},{7} Z",
                    _points[0].X, _points[0].Y, _points[1].X, _points[1].Y,
                    _points[3].X, _points[3].Y, _points[2].X, _points[2].Y);
            }
        }

        public string BoundsData =>
            string.Format("M {0},{1} L {2},{3} {4},{5} {6},{7} Z",
                _bounds[0].X, _bounds[0].Y, _bounds[1].X, _bounds[1].Y,
                _bounds[3].X, _bounds[3].Y, _bounds[2].X, _bounds[2].Y);

        public string InputOutputData => PathData;

        public Rectangle(Vector2 point1, Vector2 point2) {
            _points = new List<Vector2> {
                point1,
                point2,
                new Vector2(point1.X, point2.Y),
                new Vector2(point2.X, point1.Y)
            };

            // Порядок точек: левая нижняя, левая верхняя, правая нижняя, правая верхняя
            SortPoints();
            _bounds = new List<Vector2> { _points[0], _points[1], _points[2], _points[3] };
        }

        public bool IsPointInFigure(Vector2 point) {
            var point1 = GeometryUtils.RectangleSideProduct(point, _points[0], _points[1]);
            var point2 = GeometryUtils.RectangleSideProduct(point, _points[1], _points[3]);
            var point3 = GeometryUtils.RectangleSideProduct(point, _points[3], _points[2]);
            var point4 = GeometryUtils.RectangleSideProduct(point, _points[2], _points[0]);

            return point1 > 0 && point2 > 0 && point3 > 0 && point4 > 0;
        }

        public int IsPointNearVerticle(Vector2 point) {
            float eps = 25.0f;
            if (Vector2.Distance(_bounds[0], point) < eps) {
                return 0;
            } else if (Vector2.Distance(_bounds[1], point) < eps) {
                return 1;
            } else if (Vector2.Distance(_bounds[2], point) < eps) {
                return 2;
            } else if (Vector2.Distance(_bounds[3], point) < eps) {
                return 3;
            } else {
                return -1;
            }
        }

        public void Move(Vector2 startPosition, Vector2 newPosition) {
            var moveVector = new Vector2(newPosition.X - startPosition.X,
                newPosition.Y - startPosition.Y);

            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(_points[i].X + moveVector.X, _points[i].Y + moveVector.Y);

            for (int i = 0; i < _bounds.Count; i++)
                _bounds[i] = new Vector2(_bounds[i].X + moveVector.X, _bounds[i].Y + moveVector.Y);
        }

        public void Rotate(float angle) {
            _angle += angle % 360 + 360;
            _angle = _angle % 360;
            float eps = 0.01f;
            float angleConvert = (float)Math.PI * angle / 180;
            List<Vector2> rotateMatrix = new List<Vector2> {
                new Vector2((float)Math.Cos(angleConvert), -(float)Math.Sin(angleConvert)),
                new Vector2((float)Math.Sin(angleConvert), (float)Math.Cos(angleConvert))
            };
            Vector2 center = Vector2.Multiply(0.5f, _points[2] - _points[1]);
            for (int i = 0; i < 4; i++) _points[i] -= center;
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(
                    rotateMatrix[0].X * _points[i].X + rotateMatrix[0].Y * _points[i].Y,
                    rotateMatrix[1].X * _points[i].X + rotateMatrix[1].Y * _points[i].Y);
            for (int i = 0; i < 4; i++) _points[i] += center;

            if (_angle >= (270f + eps) && _angle < 360f) {
                _bounds[0] = new Vector2(_points[1].X - boundEps, _points[0].Y + boundEps);
                _bounds[1] = new Vector2(_points[1].X - boundEps, _points[3].Y - boundEps);
                _bounds[2] = new Vector2(_points[2].X + boundEps, _points[0].Y + boundEps);
                _bounds[3] = new Vector2(_points[2].X + boundEps, _points[3].Y - boundEps);
            } else if (_angle >= (180f + eps) && _angle < 270) {
                _bounds[0] = new Vector2(_points[3].X - boundEps, _points[1].Y + boundEps);
                _bounds[1] = new Vector2(_points[3].X - boundEps, _points[2].Y - boundEps);
                _bounds[2] = new Vector2(_points[0].X + boundEps, _points[1].Y + boundEps);
                _bounds[3] = new Vector2(_points[0].X + boundEps, _points[2].Y - boundEps);
            } else if (_angle >= (90f + eps) && _angle < 180) {
                _bounds[0] = new Vector2(_points[2].X - boundEps, _points[3].Y + boundEps);
                _bounds[1] = new Vector2(_points[2].X - boundEps, _points[0].Y - boundEps);
                _bounds[2] = new Vector2(_points[1].X + boundEps, _points[3].Y + boundEps);
                _bounds[3] = new Vector2(_points[1].X + boundEps, _points[0].Y - boundEps);
            } else {
                _bounds[0] = new Vector2(_points[0].X - boundEps, _points[2].Y + boundEps);
                _bounds[1] = new Vector2(_points[0].X - boundEps, _points[1].Y - boundEps);
                _bounds[2] = new Vector2(_points[3].X + boundEps, _points[2].Y + boundEps);
                _bounds[3] = new Vector2(_points[3].X + boundEps, _points[1].Y - boundEps);
            }
        }

        public void Scale(Vector2 point, int flag) {
            if (flag == 3) {
                _bounds[1] = new Vector2(_bounds[1].X, point.Y);
                _bounds[2] = new Vector2(point.X, _bounds[2].Y);
                _bounds[3] = new Vector2(point.X, point.Y);
            } else if (flag == 2) {
                _bounds[0] = new Vector2(_bounds[0].X, point.Y);
                _bounds[3] = new Vector2(point.X, _bounds[3].Y);
                _bounds[2] = new Vector2(point.X, point.Y);
            } else if (flag == 1) {
                _bounds[3] = new Vector2(_bounds[3].X, point.Y);
                _bounds[0] = new Vector2(point.X, _bounds[0].Y);
                _bounds[1] = new Vector2(point.X, point.Y);
            } else if (flag == 0) {
                _bounds[2] = new Vector2(_bounds[2].X, point.Y);
                _bounds[1] = new Vector2(point.X, _bounds[1].Y);
                _bounds[0] = new Vector2(point.X, point.Y);
            }

            if (_angle >= 270f && _angle < 360f)
            {
                float up1 = _points[0].X - _points[1].X;
                float up2 = _points[2].X - _points[0].X;
                float pX = 1 / (up1 + up2) * ((_bounds[2].X - boundEps) - (_bounds[0].X + boundEps));

                float left1 = _points[1].Y - _points[0].Y;
                float left2 = _points[3].Y - _points[1].Y;
                float pY = 1 / (left1 + left2) * ((_bounds[0].Y - boundEps) - (_bounds[1].Y + boundEps));

                _points[0] = new Vector2(_bounds[0].X + boundEps + up1 * pX, _bounds[0].Y - boundEps);
                _points[1] = new Vector2(_bounds[0].X + boundEps, _bounds[0].Y - boundEps - left1 * pY);
                _points[2] = new Vector2(_bounds[2].X - boundEps, _bounds[2].Y - boundEps - left2 * pY);
                _points[3] = new Vector2(_bounds[1].X + boundEps + up2 * pX, _bounds[3].Y + boundEps);
            }
            // для еще 3 четвертей
        }

        public void SortPoints() {
            _points = _points.OrderBy(v => v.X).ThenByDescending(v => v.Y).ToList();
            //_bounds = new List<Vector2> {
            //    _points[0] + new Vector2(-10f, 10f), _points[1] + new Vector2(-10f, -10f), _points[2] + new Vector2(10f, 10f),
            //    _points[3] + new Vector2(10f, -10f)
            //};
        }
    }
}
