using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Geometry;
using Rectangle = Geometry.Rectangle;

namespace CrossTTD;

enum EnumState
{
    Square,
    Line,
    Ellipse,
    Free,
    Curve
}

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public List<IFigure> figureArray = new List<IFigure>();
    // public ObservableCollection<Geometry.IFigure> Figures = new ObservableCollection<IFigure>();

    // public List<Geometry.IFigure> boundsArray = new List<Geometry.IFigure>();
    private PointerPoint moveSavePoint;
    private List<bool> moveFlagArray = new List<bool>();
    private List<bool> scaleFlagArray = new List<bool>();
    private List<bool> rotateFlagArray = new List<bool>();

    private List<bool> IsActive = new List<bool>();
    private int _pointflag = -1;

    private Vector2 firstPoint;
    private Vector2 secondPoint;
    private EnumState State = EnumState.Free;

    private Point initialRotatingPoint = new Point();

    private bool IsPressed = false;

    public MainWindow()
    {
        InitializeComponent();
        firstPoint = new Vector2(0, 0);
        secondPoint = new Vector2(0, 0);
    }

    private void ButtonProfilesOnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonSettingsOnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonFilesOnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonToolsOnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonLineOnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private List<Path> DrawFigure(IFigure figure, List<byte> argb_fill, List<byte> arbg_stroke)
    {
        CultureInfo customCulture =
            (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();
        var tmp = figure.PathData;

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
        var tmpbounds = figure.BoundsData;
        pathBounds.Data = Avalonia.Media.Geometry.Parse(figure.BoundsData);
        SolidColorBrush mySolidColorBrushBounds = new SolidColorBrush();
        mySolidColorBrushBounds.Color = Color.FromArgb(255, 0, 0, 0);
        pathBounds.Stroke = mySolidColorBrushBounds;
        pathBounds.StrokeThickness = 2;

        return new List<Path>() { pathFigure, pathBounds };
    }


    private Path DrawBounds(IFigure figure)
    {
        CultureInfo customCulture =
            (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();
        var tmp = figure.BoundsData;
        pathFigure.Data = Avalonia.Media.Geometry.Parse(figure.BoundsData);
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 0);
        pathFigure.Stroke = mySolidColorBrush;
        pathFigure.StrokeThickness = 2;

        return pathFigure;
    }

    private void DrawAll()
    {
        for (int i = 0; i < figureArray.Count; i++)
        {
            List<Path> pathFigure = DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                new List<byte>() { 255, 90, 255, 0 }); //todo: fix tmp args
            // Path pathFigureBounds = DrawBounds(figureArray[i]);
            Grid grid = new Grid();
            grid.Children.Add(pathFigure[0]);
            grid.Children.Add(pathFigure[1]);
            ThisCanv.Children.Add(grid);
        }
    }

    private void ButtonSquareOnClick(object? sender, RoutedEventArgs e)
    {
        // CreateRectangle(new Vector2(1f, 1f), new Vector2(300f, 400f), new List<byte>() {255, 255, 255, 0},
        //     new List<byte>() {255, 90, 255, 0});
        State = EnumState.Square;
    }

    private void ButtonCircleOnClick(object? sender, RoutedEventArgs e)
    {
        Ellipse ellipse = new Ellipse();
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
        ellipse.Fill = mySolidColorBrush;
        ellipse.StrokeThickness = 2;
        ellipse.Stroke = Brushes.Black;

        ellipse.Width = 100;
        ellipse.Height = 50;

        ThisCanv.Children.Add(ellipse);
    }

    private void ButtonCurvedOnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        IsPressed = true;

        Vector2 currentPoint = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X,
            (float)e.GetCurrentPoint(ThisCanv).Position.Y);

        firstPoint = currentPoint;
        if (State == EnumState.Free)
        {
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
                // moveSavePoint = e.GetCurrentPoint(ThisCanv);
            }
        }
    }

    protected void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (IsPressed == true)
        {
            secondPoint.X = (float)e.GetCurrentPoint(ThisCanv).Position.X;
            secondPoint.Y = (float)e.GetCurrentPoint(ThisCanv).Position.Y;

            IsPressed = false;
            if (State == EnumState.Square)
            {
                CreateRectangle(firstPoint, secondPoint, new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 });
                State = EnumState.Free;
            }
        }

        for (int i = 0; i < figureArray.Count; i++)
        {
            if (moveFlagArray[i]) moveFlagArray[i] = false;
            if (scaleFlagArray[i]) scaleFlagArray[i] = false;
            if (rotateFlagArray[i]) rotateFlagArray[i] = false;
            figureArray[i].SortPoints();
        }
    }

    protected void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
    {
        ThisCanv.Children.Clear();
        DrawAll();

        for (int i = 0; i < figureArray.Count; i++)
        {
            // Перетаскивание
            if (moveFlagArray[i])
            {
                Vector2 p1 = new Vector2((float)moveSavePoint.Position.X, (float)moveSavePoint.Position.Y);
                Vector2 p2 = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X,
                    (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Move(p1, p2);
                // figureArray[i + 1].Move(p1, p2);
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 }); // todo: fix tmp args
                // DrawBounds(boundsArray[i]);
            }

            // Масштабирование
            if (scaleFlagArray[i])
            {
                Vector2 point = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X,
                    (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Scale(point, _pointflag);
                // boundsArray[i].Scale(point);
                // moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 }); // todo: fix tmp args
                // DrawBounds(boundsArray[i]);
            }

            if (rotateFlagArray[i])
            {
                var somePoint = e.GetPosition(ThisCanv);
                var rotateAngle = 1;

                if (somePoint.Y - initialRotatingPoint.Y < 0) rotateAngle = -1;

                figureArray[i].Rotate(rotateAngle);
                figureArray[i].SortPoints();
                DrawFigure(figureArray[i], new List<byte>() { 255, 255, 255, 0 },
                    new List<byte>() { 255, 90, 255, 0 });
            }
        }
    }

    public void CreateRectangle(Vector2 point1, Vector2 point2, List<byte> argb_fill, List<byte> arbg_stroke)
    {
        IFigure rectangle = new Rectangle(point1, point2);
        //rectangle.Rotate(300.0f);
        // rectangle_bounds.Rotate(45.0f);
        DrawFigure(rectangle, argb_fill, arbg_stroke);
        figureArray.Add(rectangle);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
        rotateFlagArray.Add(false);
    }
}
