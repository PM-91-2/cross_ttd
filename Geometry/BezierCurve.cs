using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using IO;

namespace Geometry {
    public class BezierCurve : IFigure {
        private List<Vector2> _points;
        private List<Vector2> _bounds;
        private Vector2 _position;
        private float boundEps = 0.01f;
        private float _angle;
        public List<byte> ArgbFill { get; set; }
        public List<byte> ArgbStroke { get; set; }

        public string PathData =>
            String.Format("M {0},{1} C {2},{3} {4},{5} {6},{7}",
                _points[0].X, _points[0].Y, _points[1].X, _points[1].Y,
                _points[2].X, _points[2].Y, _points[3].X, _points[3].Y);

        public ListFigureSvg ExportData => new ListFigureSvg(_points, "bezie",ArgbStroke , ArgbFill,1);

        public string BoundsData => string.Format("M {0},{1} L {2},{3} {4},{5} {6},{7} Z",
            _bounds[0].X, _bounds[0].Y, _bounds[1].X, _bounds[1].Y,
            _bounds[3].X, _bounds[3].Y, _bounds[2].X, _bounds[2].Y);

        public BezierCurve(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, List<byte> argb_fill, List<byte> argb_stroke) {
            _points = new List<Vector2> {
                point1,
                point2,
                point3,
                point4
            };
            ArgbStroke = argb_stroke;
            ArgbFill = argb_fill;
            DefineBounds();
            SortPoints();
            _position = _points[0];
        }

        public bool IsPointInFigure(Vector2 point) {
            var point1 = GeometryUtils.RectangleSideProduct(point, _bounds[0], _bounds[1]);
            var point2 = GeometryUtils.RectangleSideProduct(point, _bounds[1], _bounds[3]);
            var point3 = GeometryUtils.RectangleSideProduct(point, _bounds[3], _bounds[2]);
            var point4 = GeometryUtils.RectangleSideProduct(point, _bounds[2], _bounds[0]);

            return point1 > 0 && point2 > 0 && point3 > 0 && point4 > 0;
        }

        public int IsPointNearVerticle(Vector2 point) {
            float eps = 25.0f; // Радиус взаимодействия с точкой масштабирования
            if (Vector2.Distance(_bounds[0], point) < eps) return 0;
            else if (Vector2.Distance(_bounds[1], point) < eps) return 1;
            else if (Vector2.Distance(_bounds[2], point) < eps) return 2;
            else if (Vector2.Distance(_bounds[3], point) < eps) return 3;
            else return -1;
        }

        // todo: change implementation
        public void Move(Vector2 startPosition, Vector2 newPosition) {
            // Поиск вектора перемещения
            var moveVector = new Vector2(newPosition.X - startPosition.X,
                newPosition.Y - startPosition.Y);

            // Обновление точек фигуры
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(_points[i].X + moveVector.X, _points[i].Y + moveVector.Y);

            // Обновление точек рамки
            for (int i = 0; i < _bounds.Count; i++)
                _bounds[i] = new Vector2(_bounds[i].X + moveVector.X, _bounds[i].Y + moveVector.Y);
        }

        public void Rotate(float angle) {
            // Пересчет угла, чтобы он находился в промежутке от 0 до 360 градусов
            _angle += angle % 360 + 360;
            _angle = _angle % 360;

            // Операция поворота
            float angleConvert = (float)Math.PI * angle / 180; // Перевод градусов в радианы
            List<Vector2> rotateMatrix = new List<Vector2> {
                new Vector2((float)Math.Cos(angleConvert), -(float)Math.Sin(angleConvert)),
                new Vector2((float)Math.Sin(angleConvert), (float)Math.Cos(angleConvert))
            }; // Матрица поворота
            Vector2 center = Vector2.Multiply(0.5f, _points[2] + _points[1]); // Центр фигуры
            for (int i = 0; i < 4; i++) _points[i] -= center; // Перенос в начало координат
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(
                    rotateMatrix[0].X * _points[i].X + rotateMatrix[0].Y * _points[i].Y,
                    rotateMatrix[1].X * _points[i].X + rotateMatrix[1].Y * _points[i].Y);
            for (int i = 0; i < 4; i++) _points[i] += center; // Перенос в прежнее место

            // Вспомогательные массивы для обновления рамки
            float x1 = new float[] { _points[0].X, _points[1].X, _points[2].X, _points[3].X }.Min();
            float x2 = new float[] { _points[0].X, _points[1].X, _points[2].X, _points[3].X }.Max();
            float y1 = new float[] { _points[0].Y, _points[1].Y, _points[2].Y, _points[3].Y }.Min();
            float y2 = new float[] { _points[0].Y, _points[1].Y, _points[2].Y, _points[3].Y }.Max();

            // Пересчет точек рамки
            _bounds[0] = new Vector2(x1 - boundEps, y2 + boundEps);
            _bounds[1] = new Vector2(x1 - boundEps, y1 - boundEps);
            _bounds[2] = new Vector2(x2 + boundEps, y2 + boundEps);
            _bounds[3] = new Vector2(x2 + boundEps, y1 - boundEps);
        }

        public void Scale(Vector2 point, int flag) {
            float kx = 1; // Коэффициент масштаба по оси Х
            float ky = 1; // Коэффициент масштаба по оси Y
            int[] left = new int[] { 1, 0 };
            int[] down = new int[] { 0, 2 };
            int[] right = new int[] { 2, 3 };
            int[] up = new int[] { 3, 1 };
            int[] pointOrder = new int[] { 0, 2, 3, 1 }; // Порядок вершин при обходе против часовой стрелки
            int[] signX = new int[] { 1, 1, -1, -1 }; // Для отступа рамки вокруг фигуры
            int[] signY = new int[] { -1, 1, -1, 1 }; // Для отступа рамки вокруг фигуры
            int idx = flag; // Индекс вершины, за которую масштабируют
            int idx_op = pointOrder[(Array.IndexOf(pointOrder, idx) + 2) % 4]; // Индекс противоположной вершины

            // Для блока выворачивания фигуры
            bool x_changeble = point.X * -signX[idx] + _bounds[idx_op].X * -signX[idx_op] > 2 * Math.Abs(boundEps);
            bool y_changeble = point.Y * -signY[idx] + _bounds[idx_op].Y * -signY[idx_op] > 2 * Math.Abs(boundEps);

            // Если доступно изменение по Х
            if (x_changeble) {
                // Пересчет коэф. масштаба по Х
                kx = Math.Abs(
                    (point.X + boundEps * signX[idx] - (_bounds[idx_op].X + boundEps * signX[idx_op]))
                    / (_bounds[idx].X + boundEps * signX[idx] - (_bounds[idx_op].X + boundEps * signX[idx_op]))
                );

                // Поиск индекса вершины(не под курсором), которую нужно переместить по Х
                int idx_near;
                if (Array.IndexOf(left, idx) != -1) idx_near = left[(Array.IndexOf(left, idx) + 1) % 2];
                else idx_near = right[(Array.IndexOf(right, idx) + 1) % 2];

                // Обновление точек рамки
                _bounds[idx_near] = new Vector2(point.X, _bounds[idx_near].Y);
                _bounds[idx] = new Vector2(point.X, _bounds[idx].Y);
            }

            // Если доступно изменение по Y
            if (y_changeble) {
                // Пересчет коэф. масштаба по Y
                ky = Math.Abs(
                    (point.Y + boundEps * signY[idx] - (_bounds[idx_op].Y + boundEps * signY[idx_op]))
                    / (_bounds[idx].Y + boundEps * signY[idx] - (_bounds[idx_op].Y + boundEps * signY[idx_op]))
                );

                // Поиск индекса вершины(не под курсором), которую нужно переместить по Х
                int idx_near;
                if (Array.IndexOf(up, idx) != -1) idx_near = up[(Array.IndexOf(up, idx) + 1) % 2];
                else idx_near = down[(Array.IndexOf(down, idx) + 1) % 2];

                // Обновление точек рамки
                _bounds[idx_near] = new Vector2(_bounds[idx_near].X, point.Y);
                _bounds[idx] = new Vector2(_bounds[idx].X, point.Y);
            }

            // Обновление точек фигуры
            for (int i = 0; i < 4; i++) {
                _points[i] = new Vector2(
                    _bounds[idx_op].X + boundEps * signX[idx_op] + (_points[i].X - (_bounds[idx_op].X + boundEps * signX[idx_op])) * kx,
                    _bounds[idx_op].Y + boundEps * signY[idx_op] + (_points[i].Y - (_bounds[idx_op].Y + boundEps * signY[idx_op])) * ky);
            }
        }

        public void SortPoints() {
            _bounds = _bounds.OrderBy(v => v.X).ThenByDescending(v => v.Y).ToList();
        }

        private void DefineBounds() {
            float eps = 10.0f;
            float MaxX = _points[0].X;
            float MinX = _points[0].X;
            float MaxY = _points[0].Y;
            float MinY = _points[0].Y;

            for (int i = 1; i < _points.Count(); i++) {
                if (_points[i].X > MaxX) MaxX = _points[i].X;
                if (_points[i].X < MinX) MinX = _points[i].X;
                if (_points[i].Y > MaxY) MaxY = _points[i].Y;
                if (_points[i].Y < MinY) MinY = _points[i].Y;
            }

            _bounds = new List<Vector2>() {
                new Vector2(MaxX + eps, MaxY + eps),
                new Vector2(MaxX + eps, MinY - eps),
                new Vector2(MinX - eps, MaxY + eps),
                new Vector2(MinX - eps, MinY - eps)
            };
        }
    }
}
