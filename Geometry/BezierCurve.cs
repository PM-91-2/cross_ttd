using System.Numerics;

namespace Geometry {
    public class BezierCurve : IFigure {
        public string PathData { get; }

        public bool IsPointInFigure(Vector2 point, float eps) {
            return true;
        }

        public void Move(Vector2 startPosition, Vector2 newPosition) { }

        public void Rotate(float angle) { }

        public void Scale(float scaleX, float scaleY) { }
    }
}
