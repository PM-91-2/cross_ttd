using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Geometry
{
    public interface IFigure
    {
        List<Vector2> Points { get; set; }
        Vector2 Position { get; set; }
        bool IsPointInFigure(Vector2 point, float eps);
        void Move(Vector2 startPosition, Vector2 newPosition);
        void Rotate(float angle);
        void Scale(float scaleX, float scaleY);
    }
}
