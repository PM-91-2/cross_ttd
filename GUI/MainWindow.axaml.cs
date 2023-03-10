using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace CrossTTD;

public partial class MainWindow : Window
{
	private PointerPoint firstPoint;
	private PointerPoint secondPoint;

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

    private void ButtonSquareOnClick(object? sender, RoutedEventArgs e)
    {
	    Rectangle rectangle = new Rectangle();
	    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
	    mySolidColorBrush.Color = Color.FromArgb(255, 0, 255, 0);
	    rectangle.Fill = mySolidColorBrush;
	    rectangle.StrokeThickness = 2;
	    rectangle.Stroke = Brushes.Black;

	    rectangle.Width = 50;
	    rectangle.Height = 25;

	    ThisCanv.Children.Add(rectangle);
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
	    throw new System.NotImplementedException();
    }

    protected void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
	    firstPoint = e.GetCurrentPoint(ThisCanv);
    }

    protected void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
	    secondPoint = e.GetCurrentPoint(ThisCanv);
	    Console.WriteLine(firstPoint);
	    Console.WriteLine(secondPoint);
    }
}
