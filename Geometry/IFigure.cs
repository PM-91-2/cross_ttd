using System.Numerics;
using System.Collections.Generic;

namespace Geometry {
    public interface IFigure {
        string InputOutputData { get; }

        string PathData { get; }
        string BoundsData { get;  }
        bool IsPointInFigure(Vector2 point);
        public int IsPointNearVerticle(Vector2 point);
        void Move(Vector2 startPosition, Vector2 newPosition);
        void Rotate(float angle);
        void Scale(Vector2 point, int flag);

        void SortPoints();
    }
}

