using System.Collections.Generic;
using System.Numerics;
using IO;

namespace Geometry {
    public interface IFigure {
        ListFigureSvg ExportData { get; }
        string PathData { get; }
        string BoundsData { get; }
        List<byte> ArgbFill { get; set; }
        List<byte> ArgbStroke { get; set; }
        bool IsPointInFigure(Vector2 point);
        public int IsPointNearVerticle(Vector2 point);
        void Move(Vector2 startPosition, Vector2 newPosition);
        void Rotate(float angle);
        void Scale(Vector2 point, int flag);
        void SortPoints();
    }
}
