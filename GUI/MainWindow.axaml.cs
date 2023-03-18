﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Geometry;


namespace CrossTTD;

public partial class MainWindow : Window
{
    public List<Geometry.IFigure> figureArray = new List<Geometry.IFigure>();
    public List<Geometry.IFigure> boundsArray = new List<Geometry.IFigure>();
    private PointerPoint moveSavePoint;
    private List<bool> moveFlagArray = new List<bool>();
    private List<bool> scaleFlagArray = new List<bool>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonProfilesOnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ButtonSettingsOnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ButtonFilesOnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ButtonToolsOnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ButtonLineOnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private Path DrawFigure(IFigure figure)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();
        var tmp = figure.PathData;
        pathFigure.Data = Avalonia.Media.Geometry.Parse(figure.PathData);
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Avalonia.Media.Color.FromArgb(255, 255, 255, 0);
        pathFigure.Fill = mySolidColorBrush;

        return pathFigure;
    }


    private Path DrawBounds(IFigure figure)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        var pathFigure = new Path();
        var tmp = figure.PathData;
        pathFigure.Data = Avalonia.Media.Geometry.Parse(figure.PathData);
        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Avalonia.Media.Color.FromArgb(255, 0, 0, 0);
        pathFigure.Stroke = mySolidColorBrush;
        pathFigure.StrokeThickness = 2;

        return pathFigure;
    }


    private void DrawAll()
    {
        for (int i = 0; i < figureArray.Count; i++)
        {
            Path pathFigure = DrawFigure(figureArray[i]);
            Path pathFigureBounds = DrawBounds(figureArray[i]);
            Grid grid = new Grid();
            grid.Children.Add(pathFigure);
            grid.Children.Add(pathFigureBounds);
            ThisCanv.Children.Add(grid);
        }
    }

    private void ButtonSquareOnClick(object? sender, RoutedEventArgs e)
    {
        createRectangle();
    }

    private void ButtonCircleOnClick(object? sender, RoutedEventArgs e)
    {
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
    private void ButtonCurvedOnClick(object? sender, RoutedEventArgs e)
    {
	    throw new System.NotImplementedException();
    }

    protected void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Vector2 currentPoint = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X, (float)e.GetCurrentPoint(ThisCanv).Position.Y);
        for (int i = 0; i < figureArray.Count; i++)
        {
            if (boundsArray[i].IsPointInFigure(currentPoint))
            {
                if (boundsArray[i].isPointNearVerticle(currentPoint))
                {
                    scaleFlagArray[i] = true;
                }
                else
                {
                    moveFlagArray[i] = true;
                }
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
            }
        }
    }

    protected void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        for (int i = 0; i < figureArray.Count; i++)
        {
            if (moveFlagArray[i]) moveFlagArray[i] = false;
            if (scaleFlagArray[i]) scaleFlagArray[i] = false;
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
                Vector2 p2 = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X, (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Move(p1, p2);
                boundsArray[i].Move(p1, p2);
                moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i]);
                DrawBounds(boundsArray[i]);
            }

            // Масштабирование
            if (scaleFlagArray[i])
            {
                Vector2 point = new Vector2((float)e.GetCurrentPoint(ThisCanv).Position.X, (float)e.GetCurrentPoint(ThisCanv).Position.Y);
                figureArray[i].Scale(point);
                boundsArray[i].Scale(point);
                // moveSavePoint = e.GetCurrentPoint(ThisCanv);
                DrawFigure(figureArray[i]);
                DrawBounds(boundsArray[i]);
            }
        }
    }

    private void createRectangle()
    {
        Geometry.IFigure rectangle = new Geometry.Rectangle(new Vector2(1.5f, 2), new Vector2(200, 400));
        Geometry.IFigure rectangle_bounds = new Geometry.Rectangle(new Vector2(1.5f, 2), new Vector2(200, 400));
        // rectangle.Rotate(45.0f);
        // rectangle_bounds.Rotate(45.0f);
        DrawFigure(rectangle);
        DrawBounds(rectangle_bounds);
        figureArray.Add(rectangle);
        boundsArray.Add(rectangle_bounds);
        moveFlagArray.Add(false);
        scaleFlagArray.Add(false);
    }
}
