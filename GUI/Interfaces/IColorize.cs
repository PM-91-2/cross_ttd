using System.Drawing;

namespace CrossTTD;

public interface IColorize
{
    void ColorizeShape(Figure CurrentFigure, Color color);
    void SetColorLine(Figure CurrentLine, Color color);
    
}

public class Figure
{
}