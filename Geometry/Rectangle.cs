using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

namespace Geometry {
	public class Rectangle : IFigure {
		// Поля
		private readonly List<Vector2> _points;
		private List<Vector2> _localePoints;
		private List<Vector2> _rotatePoints;
		private List<Vector2> _movedPoints;

		private Vector2 _position;

		// Свойства
		public List<Vector2> Points { get; set; }
		public Vector2 Position { get; set; }

		// Конструктор
		public Rectangle(Vector2 point1, Vector2 point2, Vector2 position) {
			_points = new List<Vector2> {
				point1,
				point2,
				new Vector2(point1.X, point2.Y),
				new Vector2(point2.X, point1.Y)
			};

			// Порядок точек: левая верхняя, левая нижняя, правая верхняя, правая нижняя
			_points = _points.OrderBy(v => v.X).ThenByDescending(v => v.Y).ToList();
			_position = position;
		}

		// Методы
		public bool IsPointInFigure(Vector2 point, float eps) {
			var point1 = GeometryUtils.RectangleSideProduct(point, _points[0], _points[1]);
			var point2 = GeometryUtils.RectangleSideProduct(point, _points[1], _points[3]);
			var point3 = GeometryUtils.RectangleSideProduct(point, _points[3], _points[2]);
			var point4 = GeometryUtils.RectangleSideProduct(point, _points[2], _points[0]);

			return point1 > 0 && point2 > 0 && point3 > 0 && point4 > 0;
		}
		public void Move(Vector2 startPosition, Vector2 newPosition) {
			_movedPoints = _points;
			var moveVector = new Vector2(newPosition.X - startPosition.X,
				newPosition.Y - startPosition.Y);

			for (int i = 0; i < _points.Count; i++)
				_movedPoints[i] = new Vector2(_points[i].X + moveVector.X, _points[i].Y + moveVector.Y);
		}
		public void Rotate(float angle) {
			float angleConvert = MathF.PI * angle / 180;
			var localePoints = _rotatePoints = _points;

			List<Vector2> M = new List<Vector2> {
				new Vector2((float)Math.Cos(angleConvert), -(float)Math.Sin(angleConvert)),
				new Vector2((float)Math.Sin(angleConvert), (float)Math.Cos(angleConvert))
			};

			for (int i = 0; i < _points.Count; i++) localePoints[i] -= _position;

			for (int i = 0; i < _points.Count; i++)
				_rotatePoints[i] = new Vector2((M[0].X * localePoints[i].X + M[0].Y * localePoints[i].Y + _position.X),
					(M[1].X * localePoints[i].X + M[1].Y * localePoints[i].Y) + _position.Y);
		}
		public void Scale(float scaleX, float scaleY) {
			Vector2 center = Vector2.Multiply(0.5f, _points[3] - _points[0]);
			for (int i = 0; i < 4; i++) _points[i] -= center;
			for (int i = 0; i < 4; i++) _points[i] = new Vector2(_points[i].X * scaleX, _points[i].Y * scaleY);
			for (int i = 0; i < 4; i++) _points[i] += center;
		}
	}
}
