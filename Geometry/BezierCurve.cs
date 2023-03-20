using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using IO;

namespace Geometry {
    public class BezierCurve : IFigure {
        
        private List<Vector2> _points;
        private List<Vector2> _bounds;
        private Vector2 _position;
        public List<byte> ArgbFill { get; set; }
        public List<byte> ArgbStroke { get; set; }
        public string PathData =>
            String.Format("M {0},{1} C {2},{3} {4},{5} {6},{7}",
                _points[0].X, _points[0].Y, _points[1].X, _points[1].Y,
                _points[2].X, _points[2].Y, _points[3].X, _points[3].Y);

        public ListFigureSvg ExportData => new ListFigureSvg(_points, "Curve", ArgbFill, ArgbStroke);
       
        public string BoundsData => string.Format("M {0},{1} L {2},{3} {4},{5} {6},{7} Z",
            _bounds[0].X, _bounds[0].Y, _bounds[1].X, _bounds[1].Y,
            _bounds[3].X, _bounds[3].Y, _bounds[2].X, _bounds[2].Y);

        public BezierCurve(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, List<byte> argb_fill, List<byte> argb_stroke) {
            _points = new List<Vector2> {
                point1,
                point2,
                point3,
                point4
            };
            ArgbStroke = argb_stroke;
            ArgbFill = argb_fill;
            DefineBounds(); 
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

        public void SortPoints() {
            _bounds = _bounds.OrderBy(v => v.X).ThenByDescending(v => v.Y).ToList();
        }

        private void DefineBounds() {
            float eps = 10f;
            float MaxX = _points[0].X;
            float MinX = _points[0].X;
            float MaxY = _points[0].Y;
            float MinY = _points[0].Y;
            
            for (int i = 1; i < _points.Count(); i++) {
                if (_points[i].X > MaxX) MaxX = _points[i].X;
                if (_points[i].X < MinX) MinX = _points[i].X;
                if (_points[i].X > MaxY) MaxY = _points[i].Y;
                if (_points[i].X < MinY) MinY = _points[i].Y;
            }

            _bounds = new List<Vector2>() {
                new Vector2(MaxX + eps, MaxY + eps),
                new Vector2(MaxX + eps, MinY - eps),
                new Vector2(MinX - eps, MaxY + eps),
                new Vector2(MinX - eps, MinY- eps)
            };
        }
    }
}
