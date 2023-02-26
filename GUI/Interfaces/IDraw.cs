namespace CrossTTD;

public interface IDraw
{
    void DrawLine(Dot A, Dot B);
    void DrawRectangle( Dot C);
    void DrawCircle(Dot[] Dots);
}

public class Dot
{
}