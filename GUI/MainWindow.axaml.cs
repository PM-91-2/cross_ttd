using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Geometry;
using IO;

using Rectangle = Geometry.Rectangle;
using Ellipse = Geometry.Ellipse;
using Line = Geometry.Line;

namespace CrossTTD;

enum EnumState
{
    Square,
    Line,
    Ellipse,
    Free,
    Curve,
    Select
}

public partial class MainWindow : Window
{
    public List<IFigure> figureArray = new List<IFigure>();
    // public ObservableCollection<Geometry.IFigure> Figures = new ObservableCollection<IFigure>();

    // public List<Geometry.IFigure> boundsArray = new List<Geometry.IFigure>();
    private PointerPoint moveSavePoint;
    private List<bool> moveFlagArray = new List<bool>();
    private List<bool> scaleFlagArray = new List<bool>();
    private List<bool> rotateFlagArray = new List<bool>();
    private List<bool> selectedFlagArray = new List<bool>();
    private List<bool> BezierFlagsArray = new List<bool>(){false, false, false,false};
    private List<Vector2> BezierPoints = new List<Vector2>();

    private int _pointflag = -1;

    private Vector2 firstPoint = new Vector2(0, 0);
    private Vector2 secondPoint = new Vector2(0, 0);
    private EnumState State = EnumState.Free;

    private Point initialRotatingPoint = new Point();
    private double previousRotationDegree = 0;

    private bool IsPressed = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonLoadOnClick(object? sender, RoutedEventArgs e)
    {
        IO.Svg svgObj = new IO.Svg();
        List<ListFigureSvg> attrs = svgObj.LoadFromSVG();
        foreach (ListFigureSvg attr in attrs)
        {
            switch (attr.name)
            {
                case "rectangle":
                    CreateRectangleFromImport(attr.points[0], attr.points[1], attr.points[2], attr.points[3],
                    attr.fill, attr.stroke, true); break;
                case "ellipse":
                    IFigure rectangle = new Ellipse(attr.points, attr.fill, attr.stroke, attr.x_radius,attr.y_radius,attr.angle);
                    DrawFigure(rectangle, attr.fill, attr.stroke, true);
                    figureArray.Add(rectangle);
                    moveFlagArray.Add(false);
                    scaleFlagArray.Add(false);
                    rotateFlagArray.Add(false);
                    selectedFlagArray.Add(false);
                    break;
                case "line":
                    CreateLine(attr.points[0], attr.points[1], attr.fill, attr.stroke, true);
                    break;
                case  "bezie":
                    CreateBezierCurveFromTool(attr.points[0], attr.points[1],attr.points[2],attr.points[3], attr.fill, attr.stroke, true);
                    break;
            }
        }
    }

    private void ButtonSaveOnClick(object? sender, RoutedEventArgs e)
    {
        List<ListFigureSvg> exportArray = new List<ListFigureSvg>();
        foreach (IFigure figure in figureArray) {
            exportArray.Add(figure.ExportData);
        }
        IO.Svg svgObj = new IO.Svg();
        svgObj.SaveToSVG(exportArray);
    }

    private List<Path> DrawFigure(IFigure figure, List<byte> argb_fill, List<byte> arbg_stroke, Boolean needBoundingBox)
    {
        CultureInfo customCulture =
            (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();

        pathFigure.Data = Avalonia.Media.Geometry.Parse(figure.PathData);
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Color.FromArgb(argb_fill[0], argb_fill[1], argb_fill[2], argb_fill[3]);
        pathFigure.Fill = mySolidColorBrush;
        SolidColorBrush mySolidColorBrush2 = new SolidColorBrush();
        mySolidColorBrush2.Color =
            Color.FromArgb(arbg_stroke[0], arbg_stroke[1], arbg_stroke[2], arbg_stroke[3]);
        pathFigure.Stroke = mySolidColorBrush2;
        pathFigure.StrokeThickness = 4;

        var pathBounds = new Path();
        if (needBoundingBox)
        {
            pathBounds.Data = Avalonia.Media.Geometry.Parse(figure.BoundsData);
            SolidColorBrush mySolidColorBrushBounds = new SolidColorBrush();
            mySolidColorBrushBounds.Color = Color.FromArgb(255, 0, 0, 0);
            pathBounds.Stroke = mySolidColorBrushBounds;
            pathBounds.StrokeDashArray = new AvaloniaList<double>(4, 2, 4);
            pathBounds.StrokeThickness = 2;
        }

        return new List<Path>() { pathFigure, pathBounds };
    }

    private void DrawAll()
    {
        for (int i = 0; i < figureArray.Count; i++)
        {
            List<Path> pathFigure = DrawFigure(figureArray[i], figureArray[i].ArgbFill, figureArray[i].ArgbStroke, selectedFlagArray[i]); //todo: fix tmp args
            // Path pathFigureBounds = DrawBounds(figureArray[i]);
            Grid grid = new Grid();
            grid.Children.Add(pathFigure[0]);
            grid.Children.Add(pathFigure[1]);
            ThisCanv.Children.Add(grid);
        }
    }

    private void ButtonLineOnClick(object? sender, RoutedEventArgs e)
    {
        State = EnumState.Line;
    }
    private void ButtonSquareOnClick(object? sender, RoutedEventArgs e)
    {
        State = EnumState.Square;
    }

    private void ButtonCircleOnClick(object? sender, RoutedEventArgs e)
    {
        State = EnumState.Ellipse;
    }

    private void ButtonCurvedOnClick(object? sender, RoutedEventArgs e)
    {
        State = EnumState.Curve;
    }

    protected void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        IsPressed = true;

        Vector2 currentPoint = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X,
            (float)e.GetCurrentPoint(ThisCanv).Position.Y);

        firstPoint = currentPoint;
        switch (State)
        {
            case EnumState.Curve when !BezierFlagsArray[0]:
                BezierFlagsArray[0] = true;
                BezierPoints.Add(firstPoint);
                break;
            case EnumState.Curve:
            {
                if (BezierFlagsArray[1]) {
                    BezierFlagsArray[2] = true;
                    BezierPoints.Add(firstPoint);
                }

                break;
            }
            case EnumState.Free or EnumState.Select:
            {
                for (int i = 0; i < figureArray.Count; i++)
                {
                    var tmpSelectedFigure = selectedFlagArray[i];
                    selectedFlagArray[i] = figureArray[i].IsPointInFigure(currentPoint);
                    if (tmpSelectedFigure != selectedFlagArray[i])
                    {
                        UpdateCanvas();
                        State = EnumState.Select;
                    }
                }
            
                for (int i = 0; i < figureArray.Count; i++)
                {
                    _pointflag = figureArray[i].IsPointNearVerticle(currentPoint);

                    if (e.KeyModifiers == KeyModifiers.Control && _pointflag != -1)
                    {
                        rotateFlagArray[i] = true;
                        initialRotatingPoint = e.GetCurrentPoint(ThisCanv).Position;
                        break;
                    }

                    else if (_pointflag != -1)
                    {
                        moveSavePoint = e.GetCurrentPoint(ThisCanv);
                        scaleFlagArray[i] = true;
                        break;
                    }

                    else if (figureArray[i].IsPointInFigure(currentPoint))
                    {
                        moveSavePoint = e.GetCurrentPoint(ThisCanv);
                        moveFlagArray[i] = true;
                        break;
                    }
                }

                break;
            }
        }
    }

    protected void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (IsPressed)
        {
            secondPoint.X = (float)e.GetCurrentPoint(ThisCanv).Position.X;
            secondPoint.Y = (float)e.GetCurrentPoint(ThisCanv).Position.Y;

            IsPressed = false;
            switch (State)
            {
                case EnumState.Square:
                    CreateRectangleFromTool(firstPoint, secondPoint, new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 }, true);
                    State = EnumState.Free;
                    break;
                case EnumState.Ellipse:
                    CreateEllipse(firstPoint, secondPoint, new List<byte>() { 255, 255, 255, 0 },
                        new List<byte>() { 255, 90, 255, 0 }, true);
                         State = EnumState.Free;
                    break;
                case EnumState.Curve:
                    if (BezierFlagsArray[0] && !BezierFlagsArray[1]) {
                        BezierFlagsArray[1] = true;
                        BezierPoints.Add(secondPoint);
                    } else if (BezierFlagsArray[2] && !BezierFlagsArray[3]) {
                        BezierFlagsArray[3] = true;
                        BezierPoints.Add(secondPoint);
                    }

                    foreach (bool point in BezierFlagsArray) {
                        if (!point) {
                            return;
                        }
                    }

                    CreateBezierCurveFromTool(BezierPoints[0], BezierPoints[2], BezierPoints[3], BezierPoints[1], new List<byte>() { 0, 0, 0, 0 },
                        new List<byte>() { 255, 90, 255, 0 }, true);

                    State = EnumState.Free;

                    for (int i = 0; i < BezierFlagsArray.Count(); i++) {
                        BezierFlagsArray[i] = false;
                    }

                    BezierPoints.Clear();

                    break;
                case EnumState.Line:
                    CreateLine(firstPoint, secondPoint, new List<byte>() { 255, 255, 255, 0 },
                        new List<byte>() { 255, 90, 255, 0 }, true);
                    State = EnumState.Free;
                    break;
            }
            
        }

        for (int i = 0; i < figureArray.Count; i++)
        {
            if (moveFlagArray[i]) moveFlagArray[i] = false;
            if (scaleFlagArray[i]) scaleFlagArray[i] = false;
            if (rotateFlagArray[i]) rotateFlagArray[i] = false;
        }
    }
    

    protected void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
    {
        UpdateCanvas();

        for (int i = 0; i < figureArray.Count; i++)
        {
            // Перетаскивание
            if (moveFlagArray[i])
            {
                Vector2 p1 = new Vector2((float)moveSavePoint.Position.X, (float)moveSavePoint.Position.Y);
                Vector2 p2 = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X,
                (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Move(p1, p2);
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i], figureArray[i].ArgbFill, figureArray[i].ArgbStroke, selectedFlagArray[i]); // todo: fix tmp args
            }

            // Масштабирование
            if (scaleFlagArray[i])
            {
                Vector2 point = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X,
                (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Scale(point, _pointflag);
                DrawFigure(figureArray[i], figureArray[i].ArgbFill, figureArray[i].ArgbStroke, selectedFlagArray[i]); // todo: fix tmp args
            }

            if (rotateFlagArray[i])
            {
                var angle = 2;
                var dragPoint = e.GetPosition(ThisCanv);

                Vector2 center = new Vector2(
                    (secondPoint.X - firstPoint.X) / 2,
                    (secondPoint.Y - firstPoint.Y) / 2
                );

                var rotationAngle = GetRotationAngle(
                    GetPointWithNewCenter(center, initialRotatingPoint), 
                    GetPointWithNewCenter(center, dragPoint)
                    );

                if (rotationAngle < previousRotationDegree)
                {
                    angle = -angle;
                }
                
                figureArray[i].Rotate(angle);
                DrawFigure(figureArray[i], figureArray[i].ArgbFill, figureArray[i].ArgbStroke, selectedFlagArray[i]);
                
                previousRotationDegree = rotationAngle;
            }
        }
    }
    
    private static double GetRotationAngle(Point initialPoint, Point dragPoint)
    {
        double x1 = initialPoint.X, y1 = initialPoint.Y;
        double x2 = dragPoint.X, y2 = dragPoint.Y;
        double A = Math.Atan2(y1 - y2, x1 - x2) / Math.PI * 180;

        return (A < 0) ? A + 360 : A;
    }

    private static Point GetPointWithNewCenter(Vector2 center, Point point)
    {
        return new Point(
                center.X - point.X,
                center.Y - point.Y
            );
    }

    private void CreateLine(Vector2 point1, Vector2 point2, List<byte> argb_fill, List<byte> argb_stroke, Boolean needBoundingBox)
    {
        IFigure line = new Line(point1, point2, argb_fill, argb_stroke);
        DrawFigure(line, argb_fill, argb_stroke, needBoundingBox);
        figureArray.Add(line);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
        rotateFlagArray.Add(false);
        selectedFlagArray.Add(false);
    }
    public void CreateRectangleFromTool(Vector2 point1, Vector2 point2, List<byte> argb_fill, List<byte> argb_stroke, Boolean needBoundingBox)
    {
        IFigure rectangle = new Rectangle(point1, point2, argb_fill, argb_stroke);
        DrawFigure(rectangle, argb_fill, argb_stroke, needBoundingBox);
        figureArray.Add(rectangle);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
        rotateFlagArray.Add(false);
        selectedFlagArray.Add(false);
    }
    public void CreateRectangleFromImport(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, List<byte> argb_fill, List<byte> argb_stroke, Boolean needBoundingBox)
    {
        IFigure rectangle = new Rectangle(point1, point2, point3, point4, argb_fill, argb_stroke);
        DrawFigure(rectangle, argb_fill, argb_stroke, needBoundingBox);
        figureArray.Add(rectangle);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
        rotateFlagArray.Add(false);
        selectedFlagArray.Add(false);
    }

    public void CreateEllipse(Vector2 point1, Vector2 point2, List<byte> argb_fill, List<byte> argb_stroke, Boolean needBoundingBox)
    {
        IFigure rectangle = new Ellipse(point1, point2, argb_fill, argb_stroke);
        DrawFigure(rectangle, argb_fill, argb_stroke, needBoundingBox);
        figureArray.Add(rectangle);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
        rotateFlagArray.Add(false);
        selectedFlagArray.Add(false);
    }
    
    public void CreateBezierCurveFromTool(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, List<byte> argb_fill, List<byte> argb_stroke, Boolean needBoundingBox) {
        IFigure rectangle = new BezierCurve(point1, point2, point3, point4, argb_fill, argb_stroke);
        DrawFigure(rectangle, argb_fill, argb_stroke, needBoundingBox);
        figureArray.Add(rectangle);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
        rotateFlagArray.Add(false);
        selectedFlagArray.Add(false);
    }
    
    private void UpdateCanvas()
    {
        ThisCanv.Children.Clear();
        DrawAll();
    }

    private void UpdateFigure(int index)
    {
        ThisCanv.Children.RemoveAt(index);
        var pathFigure = DrawFigure(figureArray[index], new List<byte>() { 255, 255, 255, 0 },
            new List<byte>() { 255, 90, 255, 0 }, selectedFlagArray[index]);
        Grid grid = new Grid();
        grid.Children.Add(pathFigure[0]);
        grid.Children.Add(pathFigure[1]);
        ThisCanv.Children.Add(grid);
    }

    private void KeyEvents(object? sender, KeyEventArgs e)
    {
        switch (State)
        {
            case EnumState.Select:
                var selectedIndex = -1;
                for (int i = 0; i < figureArray.Count; i++)
                {
                    if (selectedFlagArray[i] is true)
                    {
                        selectedIndex = i;
                    }
                }
                if (e.Key is Key.Delete && selectedIndex != -1)
                {
                    DeleteFigure(selectedIndex);
                }
                break;
        }
    }

    private void DeleteFigure(int index)
    {
        ThisCanv.Children.RemoveAt(index);
        figureArray.RemoveAt(index);
        selectedFlagArray.RemoveAt(index);
        moveFlagArray.RemoveAt(index);
        scaleFlagArray.RemoveAt(index);
        rotateFlagArray.RemoveAt(index);
        State = EnumState.Free;
    }
}
