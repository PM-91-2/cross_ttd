using System.Numerics;

namespace IO;

public class ListFigureSvg
{
    public string name;
    public float r1_ellipse;
    public float r2_ellipse;
    public List<byte> stroke;
    public List<byte> fill;
    public Vector2 P1_rect;
    public Vector2 P2_rect;
    public Vector2 P_ellipse;
    public Vector2 P1_line;
    public Vector2 P2_line;
    public List<Vector2> points_bezie;
    
    // for rectangle
    public ListFigureSvg(Vector2 P1, Vector2 P2, string id, List<byte> color_stroke, List<byte> color_fill)
    {
        P1_rect = P1;
        P2_rect = P2;
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
    public ListFigureSvg(Vector2 P1, Vector2 P2, string id, List<byte> color_stroke, List<byte> color_fill, bool flag)
    {
        P1_line = P1;
        P2_line = P2;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
    
    // for bezie
    public ListFigureSvg(List<Vector2> point, string id, List<byte> color_stroke, List<byte> color_fill)
    {
        points_bezie = point;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
}
