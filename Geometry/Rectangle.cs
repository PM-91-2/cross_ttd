using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

namespace Geometry
{
    public class Rectangle : IFigure
    {
        // Поля
        private List<Vector2> points;
        private Vector2 position;
        private List<Vector2> locale_points;
        private List<Vector2> rotate_points;
        private List<Vector2> moved_points;


        // Свойства
        public List<Vector2> Points { get; set; }
        public Vector2 Position { get; set; }


        // Конструктор
        public Rectangle(Vector2 point1, Vector2 point2, Vector2 _position)
        {
            points = new List<Vector2>
            {
                point1,
                point2,
                new Vector2(point1.X, point2.Y),
                new Vector2(point2.X, point1.Y)
            };

            // Порядок точек: левая верхняя, левая нижняя, правая верхняя, правая нижняя
            points = points.OrderBy(v => v.X).ThenByDescending(v => v.Y).ToList();
            position = _position;
        }


		// Методы
		public bool IsPointInFigure(Vector2 point, float eps) {
	        var point1 = GeometryUtils.RectangleSideProduct(point, points[0], points[1]);
	        var point2 = GeometryUtils.RectangleSideProduct(point, points[1], points[3]);
	        var point3 = GeometryUtils.RectangleSideProduct(point, points[3], points[2]);
	        var point4 = GeometryUtils.RectangleSideProduct(point, points[2], points[0]);

	        return point1 > 0 && point2 > 0 && point3 > 0 && point4 > 0;
        }
        public void Move(Vector2 startPosition, Vector2 newPosition)
        {
            moved_points = points;
            var moveVector = new Vector2(newPosition.X - startPosition.X,
                                            newPosition.Y - startPosition.Y);
            for (int i=0; i < points.Count; i++)
                moved_points[i] = new Vector2(points[i].X + moveVector.X, points[i].Y + moveVector.Y);

        }
	    public void Rotate(float angle)
        {
            float angle_convert = MathF.PI * angle / 180;
            var locale_points = rotate_points = points;
            List<Vector2> M = new List<Vector2>{ new Vector2((float)Math.Cos(angle_convert), -(float)Math.Sin(angle_convert)),
                                                 new Vector2((float)Math.Sin(angle_convert), (float)Math.Cos(angle_convert)) };

            for (int i = 0; i < points.Count; i++) locale_points[i] -= position;

            for (int i = 0; i < points.Count; i++)
                rotate_points[i] = new Vector2((M[0].X * locale_points[i].X + M[0].Y * locale_points[i].Y + position.X),
                                               (M[1].X * locale_points[i].X + M[1].Y * locale_points[i].Y) + position.Y);
        }
		public void Scale(float scaleX, float scaleY)
        {
            Vector2 center = Vector2.Multiply(0.5f, points[3] - points[0]);
            for (int i = 0; i < 4; i++) points[i] -= center;
            for (int i = 0; i < 4; i++) points[i] = new Vector2(points[i].X * scaleX, points[i].Y * scaleY);
            for (int i = 0; i < 4; i++) points[i] += center;

        }
    }
}
