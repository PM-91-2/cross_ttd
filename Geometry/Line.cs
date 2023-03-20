using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

namespace Geometry 
{
    public class Line : IFigure 
    {
        private List<Vector2> _points;
        private float _angle = 0f;
        private float Eps = 10;

        public string PathData {
            get {
                return string.Format("M {0},{1} L {2},{3}",
                    _points[0].X, _points[0].Y, _points[1].X, _points[1].Y);
            }
        }


        public string InputOutputData => PathData;

        public Line(Vector2 point1, Vector2 point2) {
            _points = new List<Vector2> {
                point1,
                point2
            };

        }

        public bool IsPointOnLine(Vector2 point, float eps) {
         var Distance ((point1.X-point.X)**2+(point1.Y-point.Y)**2)**0.5+
            ((point2.X-point.X)**2+(point2.Y-point.Y)**2)**0.5-
            ((point1.X-point2.X)**2+(point1.Y-ppint2.Y)**2)**0.5
            return Distance <= 2*Eps
        }

        public int IsPointNearVerticle(Vector2 point) {
            float eps = 25.0f;  // Радиус взаимодействия с точкой масштабирования
            if (Vector2.Distance(_points[0], point) < eps) return 0;
            else if (Vector2.Distance(_points[1], point) < eps) return 1;
            else return -1;
        }

        public void Move(Vector2 startPosition, Vector2 newPosition) {
            var moveVector = new Vector2(newPosition.X - startPosition.X,
                newPosition.Y - startPosition.Y);

                // Обновление точек отрезка
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(_points[i].X + moveVector.X, _points[i].Y + moveVector.Y);

        }
        }
        public void Rotate(float angle) {
            // Пересчет угла, чтобы он находился в промежутке от 0 до 360 градусов
            _angle += angle % 360 + 360;
            _angle = _angle % 360;

            // Операция поворота
            float angleConvert = (float)Math.PI * angle / 180;  // Перевод градусов в радианы
            List<Vector2> rotateMatrix = new List<Vector2> {
                new Vector2((float)Math.Cos(angleConvert), -(float)Math.Sin(angleConvert)),
                new Vector2((float)Math.Sin(angleConvert), (float)Math.Cos(angleConvert))
            };  // Матрица поворота
            Vector2 center = Vector2.Multiply(0.5f, _points[1] + _points[0]);  // Центр фигуры
            for (int i = 0; i < 2; i++) _points[i] -= center;  // Перенос в начало координат
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(
                    rotateMatrix[0].X * _points[i].X + rotateMatrix[0].Y * _points[i].Y,
                    rotateMatrix[1].X * _points[i].X + rotateMatrix[1].Y * _points[i].Y);
            for (int i = 0; i < 2; i++) _points[i] += center;  // Перенос в прежнее место

        }
        
        public void Scale(Vector2 startPosition, Vector2 newPosition) {
           if ((startPosition.X - _points[0].X) <= Eps) 
           _points[0] = newPosition;
           else if ((startPosition.X - _points[1].X) <= Eps)
           _points[1] = newPosition;
        }
    }
}
