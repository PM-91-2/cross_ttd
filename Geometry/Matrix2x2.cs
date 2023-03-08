using System.Numerics;

namespace Geometry {
    public static class Matrix {
        public class Matrix2x2 {
            public float[] values { get; set; }
            public float v00 { get; set; }
            public float v01 { get; set; }
            public float v10 { get; set; }
            public float v11 { get; set; }

            public Matrix2x2(float[] _values) {
                v00 = _values[0];
                v01 = _values[1];
                v10 = _values[2];
                v11 = _values[3];
                values = _values;
            }

            public static Matrix2x2 operator *(Matrix2x2 m, float a) {
                return new Matrix2x2(new float[] {
                    m.v00 * a, m.v01 * a,
                    m.v10 * a, m.v11 * a
                });
            }

            public static Vector2 operator *(Vector2 v, Matrix2x2 m) {
                return new Vector2(v.X * m.v00 + v.Y * m.v01, v.X * m.v10 + v.Y * m.v11);
            }
        }
    }
}
