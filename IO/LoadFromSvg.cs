using System.Numerics;

namespace IO;

public class ListFigureSvg
{
    public string name;
    public float r1_ellipse;
    public float r2_ellipse;
    public List<byte> stroke;
    public List<byte> fill;
    public Vector2 P_ellipse;
    public List<Vector2> points;
    
    // for rectangle
    public ListFigureSvg(List<Vector2> point, string id, List<byte> color_stroke, List<byte> color_fill,bool flag)
    {
        points = point;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
    // for ellipse
    public ListFigureSvg(float rx, float ry, Vector2 P, string id, List<byte> color_stroke, List<byte> color_fill)
    {
        r1_ellipse = rx;
        r2_ellipse = ry;
        P_ellipse = P;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
    // for line
    public ListFigureSvg(List<Vector2> point, string id, List<byte> color_stroke, List<byte> color_fill)
    {
        points = point;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
    
    // for bezie
    public ListFigureSvg(List<Vector2> point, string id, List<byte> color_stroke, List<byte> color_fill,int a)
    {
        points = point;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
}
