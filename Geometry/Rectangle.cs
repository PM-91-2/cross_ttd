using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    internal class Geometry
    {
        // Общий интерфейс
        public interface IFigure  // : ICloneable
        {
            IEnumerable<Vector2> Points(double eps);  // Контур фигуры, eps - мин. расстояние между точками
            //bool IsContain(Vector2 p, double eps);  // Принадлежность точки фигуре
            //void Translate(Vector2 to);  // Перемещение
            //void Scale(double scale);  // Масштаб
            //void Rotate(double angle);  // Вращение
        }

        public class Rectangle
        {
            private List<Vector2> points;
            private Vector2 position;
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
            public bool isContain(Vector2 point, double eps)
            {
                // Дима
                return true;
            }
            public void Move(Vector2 startPosition, Vector2 newPosition)
            {
                // Маша
            }
            public void Rotate(double angle)
            {
                // Саша
            }
            public void Scale()
            {

            }
        }

        //IFigure Intersect(IFigure figure1);  // Пересечение
        //IFigure Union(IFigure figure1);  // Объединение
        //IFigure Substract(IFigure figure1);  // Разность



        // Прямоугольник










        void Draw(igraphic gl) { }
        public interface igraphic
        {
            void drawline();
            void drawcircle();
            void drawarc();
        }
    }
}
