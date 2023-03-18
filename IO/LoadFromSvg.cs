namespace IO;
public class ListFigureSvg
{
    public string name;
    public float x1_line;
    public float x2_line;
    public float y1_line;
    public float y2_line;
    public float x1_rect;
    public float x2_rect;
    public float y1_rect;
    public float y2_rect;
    public float r1_ellipse;
    public float r2_ellipse;
    public float y_ellipse;
    public float x_ellipse;
    public string path;
    public List<byte> stroke;
    public List<byte> fill;
    
    public ListFigureSvg(float x1Rect, float x2Rect, float y1Rect,float y2Rect)
    {
        x1_rect = x1Rect;
        x2_rect = x2Rect;
        y1_rect = y1Rect;
        y2_rect = y2Rect;
    }
    public ListFigureSvg(float x1Rect, float x2Rect, float y1Rect)
    {
        x1_rect = x1Rect;
        x2_rect = x2Rect;
        y1_rect = y1Rect;
    }

}
