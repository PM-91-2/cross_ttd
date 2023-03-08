using System.Numerics;

namespace Geometry {
    public interface IFigure {
        string PathData { get; }
        bool IsPointInFigure(Vector2 point, float eps);
        void Move(Vector2 startPosition, Vector2 newPosition);
        void Rotate(float angle);
        void Scale(float scaleX, float scaleY);
    }
}

