using System.Collections.Generic;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Geometry;
using IO;


namespace CrossTTD;

public partial class MainWindow : Window {
    public List<Geometry.IFigure> figureArray = new List<Geometry.IFigure>();

    // public List<Geometry.IFigure> boundsArray = new List<Geometry.IFigure>();
    private PointerPoint moveSavePoint;
    private List<bool> moveFlagArray = new List<bool>();
    private List<bool> scaleFlagArray = new List<bool>();
    private List<bool> IsActive = new List<bool>();
    private int _pointflag = -1;

    public MainWindow() {
        InitializeComponent();
    }

    private void ButtonProfilesOnClick(object? sender, RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }

    private void ButtonSettingsOnClick(object? sender, RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }

    private void ButtonFilesOnClick(object? sender, RoutedEventArgs e)
    {
        IO.Svg svg = new IO.Svg();
        var figures = svg.LoadFromSVG();
        for (int i = 0; i < figures.Count; i++)
        {
            if (figures[i].name == "rectangle")
            {
                CreateRectangle(figures[i].P1_rect,figures[i].P2_rect,figures[i].fill,figures[i].stroke);
            }
        }
    }

    private void ButtonToolsOnClick(object? sender, RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }

    private void ButtonLineOnClick(object? sender, RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }

    private List<Path> DrawFigure(IFigure figure, List<byte> argb_fill, List<byte> arbg_stroke) {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();
        var tmp = figure.PathData;

        pathFigure.Data = Avalonia.Media.Geometry.Parse(figure.PathData);
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Avalonia.Media.Color.FromArgb(argb_fill[0], argb_fill[1], argb_fill[2], argb_fill[3]);
        pathFigure.Fill = mySolidColorBrush;
        SolidColorBrush mySolidColorBrush2 = new SolidColorBrush();
        mySolidColorBrush2.Color = Avalonia.Media.Color.FromArgb(arbg_stroke[0], arbg_stroke[1], arbg_stroke[2], arbg_stroke[3]);
        pathFigure.Stroke = mySolidColorBrush2;
        pathFigure.StrokeThickness = 4;

        var pathBounds = new Path();
        var tmpbounds = figure.BoundsData;
        pathBounds.Data = Avalonia.Media.Geometry.Parse(figure.BoundsData);
        SolidColorBrush mySolidColorBrushBounds = new SolidColorBrush();
        mySolidColorBrushBounds.Color = Avalonia.Media.Color.FromArgb(255, 0, 0, 0);
        pathBounds.Stroke = mySolidColorBrushBounds;
        pathBounds.StrokeThickness = 2;

        return new List<Path>() { pathFigure, pathBounds };
    }


    private Path DrawBounds(IFigure figure) {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();
        var tmp = figure.BoundsData;
        pathFigure.Data = Avalonia.Media.Geometry.Parse(figure.BoundsData);
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Avalonia.Media.Color.FromArgb(255, 0, 0, 0);
        pathFigure.Stroke = mySolidColorBrush;
        pathFigure.StrokeThickness = 2;

        return pathFigure;
    }
    
    private void DrawAll() {
        for (int i = 0; i < figureArray.Count; i++) {
            List<Path> pathFigure = DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                new List<byte>() { 255, 90, 255, 0 }); //todo: fix tmp args
            // Path pathFigureBounds = DrawBounds(figureArray[i]);
            Grid grid = new Grid();
            grid.Children.Add(pathFigure[0]);
            grid.Children.Add(pathFigure[1]);
            ThisCanv.Children.Add(grid);
        }
    }

    private void ButtonSquareOnClick(object? sender, RoutedEventArgs e) {
        CreateRectangle(new Vector2(1f, 1f), new Vector2(300f, 400f), new List<byte>() { 255, 255, 255, 0 },
            new List<byte>() { 255, 90, 255, 0 });
    }

    private void ButtonCircleOnClick(object? sender, RoutedEventArgs e) {
        Ellipse ellipse = new Ellipse();
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Avalonia.Media.Color.FromArgb(255, 255, 255, 0);
        ellipse.Fill = mySolidColorBrush;
        ellipse.StrokeThickness = 2;
        ellipse.Stroke = Avalonia.Media.Brushes.Black;

        ellipse.Width = 100;
        ellipse.Height = 50;

        ThisCanv.Children.Add(ellipse);
    }

    private void ButtonCurvedOnClick(object? sender, RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }

    protected void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e) {
        Vector2 currentPoint = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X, (float)e.GetCurrentPoint(ThisCanv).Position.Y);
        for (int i = 0; i < figureArray.Count; i++) {
            _pointflag = figureArray[i].IsPointNearVerticle(currentPoint);
            if (_pointflag != -1) {
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
                scaleFlagArray[i] = true;
                break;
            } else if (figureArray[i].IsPointInFigure(currentPoint)) {
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
                moveFlagArray[i] = true;
                break;
            }
            // moveSavePoint = e.GetCurrentPoint(ThisCanv);
        }
    }

    protected void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e) {
        for (int i = 0; i < figureArray.Count; i++) {
            if (moveFlagArray[i]) moveFlagArray[i] = false;
            if (scaleFlagArray[i]) scaleFlagArray[i] = false;
            figureArray[i].SortPoints();
        }
    }

    protected void OnCanvasPointerMoved(object? sender, PointerEventArgs e) {
        ThisCanv.Children.Clear();
        DrawAll();

        for (int i = 0; i < figureArray.Count; i++) {
            // Перетаскивание
            if (moveFlagArray[i]) {
                Vector2 p1 = new Vector2((float)moveSavePoint.Position.X, (float)moveSavePoint.Position.Y);
                Vector2 p2 = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X, (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Move(p1, p2);
                // figureArray[i + 1].Move(p1, p2);
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 }); // todo: fix tmp args
                // DrawBounds(boundsArray[i]);
            }

            // Масштабирование
            if (scaleFlagArray[i]) {
                Vector2 point = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X, (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Scale(point, _pointflag);
                // boundsArray[i].Scale(point);
                // moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 }); // todo: fix tmp args
                // DrawBounds(boundsArray[i]);
            }
        }
    }

    public void CreateRectangle(Vector2 point1, Vector2 point2, List<byte> argb_fill, List<byte> arbg_stroke) {
        Geometry.IFigure rectangle = new Geometry.Rectangle(point1, point2);
        //rectangle.Rotate(300.0f);
        // rectangle_bounds.Rotate(45.0f);
        DrawFigure(rectangle, argb_fill, arbg_stroke);
        figureArray.Add(rectangle);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
    }
}
