using System.Numerics;
 
namespace IO;
 
public class ListFigureSvg
{
    public string name;
    public float x_radius;
    public float y_radius;
    public List<byte> stroke;
    public List<byte> fill;
    public List<Vector2> points;
    public float angle;
 
    // for rectangle
    public ListFigureSvg(List<Vector2> point, string id, List<byte> color_stroke, List<byte> color_fill,bool flag)
    {
        points = point;
        stroke = color_stroke;
        fill = color_fill;
        name = id; 
    }
    // for ellipse
    public ListFigureSvg(float rx, float ry, List<Vector2> P, string id, List<byte> color_stroke, List<byte> color_fill, float angl)
    {
        x_radius = rx;
        y_radius = ry;
        points = P;
        stroke = color_stroke;
        fill = color_fill;
        name = id;
        angle = angl;
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
