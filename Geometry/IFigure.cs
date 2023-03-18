using System.Numerics;

namespace Geometry {
    public interface IFigure {
        string PathData { get; }
        bool IsPointInFigure(Vector2 point);
        public bool isPointNearVerticle(Vector2 point);
        void Move(Vector2 startPosition, Vector2 newPosition);
        void Rotate(float angle);
        void Scale(Vector2 point);
    }
}

