using System.Numerics;
using Geometry;

namespace LinearAlgebra.Test
{
    public class Matrix2x2Test
    {

        [SetUp]
        public void setup()
        {

        }

        //умножение матрицы на число
        [Test]

        public void matrix_23_12and4__81248returned()
        {
            //arrange
            Matrix.Matrix2x2 MatX = new Matrix.Matrix2x2(new float[] { 2, 3, 1, 2 });

            float a = 4;

            Matrix.Matrix2x2 expected = new Matrix.Matrix2x2(new float[] { 8, 12, 4, 8 });
            //act
            Matrix.Matrix2x2 actual = MatX * a;

            //assert
            Assert.That(actual.v00, Is.EqualTo(8));
            Assert.That(actual.v01, Is.EqualTo(12));
            Assert.That(actual.v10, Is.EqualTo(4));
            Assert.That(actual.v11, Is.EqualTo(8));
        }

        //умножение вектора на матрицу 
        [Test]
        public void vector12_matrix_23_12_85returned()
        {
            //arrange

            Vector2 s = new Vector2(new float[] { 1, 2 });

            Matrix.Matrix2x2 m = new Matrix.Matrix2x2(new float[] { 2, 3, 1, 2 });


            Vector2 expected = new Vector2(new float[] { 8, 5 });
            //act
            Vector2 actual = new Vector2(s.X * m.v00 + s.Y * m.v01, s.X * m.v10 + s.Y * m.v11);

            //assert
            Assert.AreEqual(expected, actual);


        }
    }
    
    /*public class ListFigureSvgtest
    {

        [SetUp]
        public void setup()
        {

        }

        //линия
        [Test]
        
        public void line00_11returned()
        {
            //arrange
            
            Vector2 = new List<Vector2> [0, 0];
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            
            var expected = "line";
            var actual = IFigure.(points).ToString();
            
            Assert.AreEqual(expected, actual);

            //assert
            Assert.That(actual.v00, Is.EqualTo(8));
            Assert.That(actual.v01, Is.EqualTo(12));
            Assert.That(actual.v10, Is.EqualTo(4));
            Assert.That(actual.v11, Is.EqualTo(8));


        }
    }*/
    
}







