using System.Drawing;

namespace Logic.Common;

public enum FigureTransformationButtonType
{
    Rotate ,
    Scale,
    Move
}

public struct TransformationValue
{
    public Point Coordinate { get; }
    public double Degrees { get; }
    public Size Size { get; }

    public TransformationValue(double degrees)
    {
        this.Degrees = degrees;
        this.Size = new Size();
        this.Coordinate = new Point();
    }
    
    public TransformationValue(Size size)
    {
        this.Degrees = 0.0;
        this.Size = size;
        this.Coordinate = new Point();
    }
    
    public TransformationValue(Point point)
    {
        this.Degrees = 0.0;
        this.Size = new Size();
        this.Coordinate = point;
    }
}
