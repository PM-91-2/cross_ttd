using System.Collections.Generic;
using System.Numerics;

namespace Geometry {
    public class BezierCurve : IFigure {
        private List<Vector2> _points;
        private Vector2 _position;

        public string PathData { get; }

        public string InputOutputData { get; }

        private BezierCurve(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4) {
            _points = new List<Vector2> {
                point1,
                point2,
                point3,
                point4
            };

            _position = _points[0];
        }

        public bool IsPointInFigure(Vector2 point, float eps) {
            return true;
        }

        public void Move(Vector2 startPosition, Vector2 newPosition) { }

        public void Rotate(float angle) { }

        public void Scale(float scaleX, float scaleY) { }
    }
}
