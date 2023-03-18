using System;
using System.Collections.Generic;
using System.Numerics;

namespace Geometry {
    public class BezierCurve : IFigure {
        private const string NAME = "Curve";
        
        private List<Vector2> _points;
        private List<Vector2> _bounds;
        private Vector2 _position;

        public string PathData =>
            String.Format("M {0},{1} C {2},{3} {4},{5} {6},{7}",
                _points[0].X, _points[0].Y, _points[1].X, _points[1].Y,
                _points[2].X, _points[2].Y, _points[3].X, _points[3].Y);

        public string InputOutputData => 
            String.Format("Name: {0} Start_point: {1} Control_point1: {2} Control_point2: {3} End_point: {4}", NAME, _points[0], _points[1], _points[2], _points[3]);

        public string BoundsData => string.Format("M {0},{1} L {2},{3} {4},{5} {6},{7} Z",
            _bounds[0].X, _bounds[0].Y, _bounds[1].X, _bounds[1].Y,
            _bounds[3].X, _bounds[3].Y, _bounds[2].X, _bounds[2].Y);
        
        private BezierCurve(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4) {
            _points = new List<Vector2> {
                point1,
                point2,
                point3,
                point4
            };

            _position = _points[0];
        }

        public bool IsPointInFigure(Vector2 point) {
            return true;
        }

        public int IsPointNearVerticle(Vector2 point) => 2;

        // todo: change implementation
        public void Move(Vector2 startPosition, Vector2 newPosition) {
            var moveVector = new Vector2(newPosition.X - startPosition.X,
                newPosition.Y - startPosition.Y);
            
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Vector2(_points[i].X + moveVector.X, _points[i].Y + moveVector.Y);

        }

        public void Rotate(float angle) { }

        public void Scale(Vector2 point, int flag) { }

        public void SortPoints() { }
        
        
    }
}
