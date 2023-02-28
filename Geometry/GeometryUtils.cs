using System.Numerics;
namespace Geometry {
	public static class GeometryUtils {
		public static float RectangleProduct(Vector2 point, Vector2 firstPoint, Vector2 secondPoint) {
			return (secondPoint.X - firstPoint.X) * (point.Y - firstPoint.Y) - (secondPoint.Y - firstPoint.Y) * (point.X - firstPoint.X);
		}
	}
}
