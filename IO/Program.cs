using System.Drawing;
using System.Numerics;
using Svg;
using Svg.Pathing;

namespace IO {
    public class Svg
    {
        private string fileName = "../../../template.svg";
        private List<ListFigureSvg> figure_attributes = new List<ListFigureSvg>();
        public List<ListFigureSvg> LoadFromSVG()
        {
            
            var svg = SvgDocument.Open(fileName);
            foreach (SvgElement svgElem in svg.Children)
            {
                
                switch (svgElem.GetType().Name)
                {
                    case "SvgPath":
                        Path(svgElem);
                        break;
                    case "SvgEllipse":
                        Ellipse(svgElem);
                        break;
                    case "SvgRectangle":
                        Rectangle(svgElem);
                        break;
                    case "SvgLine":
                        Line(svgElem);
                        break;
                }
            }

            return figure_attributes;
        }

        private void Line(SvgElement svgElem)
        {
            SvgLine? svgLine = svgElem as SvgLine;
            var x1 = svgLine.StartX.Value;
            var y1 = svgLine.StartY.Value;
            var x2 = svgLine.EndX.Value;
            var y2 = svgLine.EndY.Value;
            List<byte> line_fill;
            List<byte> line_stroke;
            if (svgLine.Stroke != null)
            {
                var svgColourStroke = ((SvgColourServer)svgLine.Stroke).Colour;
                line_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };

            }
            else
            {
                line_stroke = new List<byte>() { 255, 0, 0, 0 };
            }
            if (svgLine.Fill != null)
            {
                var svgColourFill = ((SvgColourServer)svgLine.Fill).Colour;
                line_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };

            }
            else
            {
                line_fill = new List<byte>() { 255, 255, 255, 255 };
            }

            List<Vector2> Points = new List<Vector2>();
            Points.Add(new Vector2(x1,y1));
            Points.Add(new Vector2(x2,y2));
            ListFigureSvg figure = new ListFigureSvg(Points,"line", line_stroke, line_fill, true);
            figure_attributes.Add(figure);
        }

        private void Rectangle(SvgElement svgElem)
        {
            SvgRectangle? svgRectangle = svgElem as SvgRectangle;
            var x_rect = svgRectangle.X.Value;
            var y_rect = svgRectangle.Y.Value;
            var Height = svgRectangle.Height.Value;
            var Width = svgRectangle.Width.Value;
            List<byte> rect_stroke;
            List<byte> rect_fill;
            if (svgRectangle.Stroke != null)
            {
                var svgColourStroke = ((SvgColourServer)svgRectangle.Stroke).Colour;
                rect_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };

            }
            else
            {
                rect_stroke = new List<byte>() { 255, 0, 0, 0 };
            }
            if (svgRectangle.Fill != null)
            {
                var svgColourFill = ((SvgColourServer)svgRectangle.Fill).Colour;
                rect_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };

            }
            else
            {
                rect_fill = new List<byte>() { 255, 255, 255, 255 };
            }

            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(x_rect,y_rect));
            points.Add(new Vector2(x_rect+Width,y_rect));
            points.Add(new Vector2(x_rect,y_rect+Height));
            points.Add(new Vector2(x_rect+Width, y_rect+Height));
            ListFigureSvg figure = new ListFigureSvg(points,"rectangle", rect_stroke, rect_fill); // To do
            figure_attributes.Add(figure);
        }

        private void Ellipse(SvgElement svgElem)
        {
            SvgEllipse? svgEllipse = svgElem as SvgEllipse;
            var rx = svgEllipse.RadiusX.Value;
            var ry = svgEllipse.RadiusY.Value;
            var x = svgEllipse.CenterX.Value;
            var y = svgEllipse.CenterY.Value;
            List<byte> ellipse_fill;
            List<byte> ellipse_stroke;
            if (svgEllipse.Stroke != null)
            {
                var svgColourStroke = ((SvgColourServer)svgEllipse.Stroke).Colour;
                ellipse_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };

            }
            else
            {
                ellipse_stroke = new List<byte>() { 255, 0, 0, 0 };
            }
            if (svgEllipse.Fill != null)
            {
                var svgColourFill = ((SvgColourServer)svgEllipse.Fill).Colour;
                ellipse_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };

            }
            else
            {
                ellipse_fill = new List<byte>() { 255, 255, 255, 255 };
            }
            ListFigureSvg figure = new ListFigureSvg(rx,ry,new Vector2(x,y),"ellipse", ellipse_stroke, ellipse_fill);
            figure_attributes.Add(figure);
        }
        //test bezie
        private void Path(SvgElement svgElem)
        {
            SvgPath? svgPath = svgElem as SvgPath;
            string name="";
            List<Vector2> points = new List<Vector2>();
            if (svgPath.PathData.Count == 3)
            {
                if ( svgPath.PathData[1] is SvgCubicCurveSegment)
                {
                    SvgCubicCurveSegment a = (SvgCubicCurveSegment)svgPath.PathData[1];
                    if (a.FirstControlPoint.X != null && a.SecondControlPoint.X != null)
                    {
                        name = "bezie";
                        points.Add(new Vector2(svgPath.PathData[0].End.X, svgPath.PathData[0].End.Y));
                        points.Add(new Vector2(a.FirstControlPoint.X, a.FirstControlPoint.Y));
                        points.Add(new Vector2(a.SecondControlPoint.X, a.SecondControlPoint.Y));
                        points.Add(new Vector2(a.End.X, a.End.Y));
                    }
                }
                if (svgPath.PathData[1] is SvgLineSegment)
                {
                    SvgLineSegment line = (SvgLineSegment)svgPath.PathData[1];

                        name = "line";
                        points.Add(new Vector2(svgPath.PathData[0].End.X, svgPath.PathData[0].End.Y));
                        points.Add(new Vector2(line.End.X, line.End.Y));
                    
                }
            }
            //rectangle
            bool flag = true;
            if (svgPath.PathData.Count == 5)
            {
                for (int i = 1; i < svgPath.PathData.Count-1; i++)
                {
                    if (!(svgPath.PathData[i] is SvgLineSegment))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    points.Add(new Vector2(svgPath.PathData[0].End.X, svgPath.PathData[0].End.Y));
                    for (int i = 1; i < svgPath.PathData.Count-1; i++)
                    {
                        SvgLineSegment rectangle = (SvgLineSegment)svgPath.PathData[i];
                        points.Add(new Vector2(rectangle.End.X, rectangle.End.Y));
                    }
                    name = "treangle";
                }
            }
            List<byte> path_stroke;
            List<byte> path_fill;
            if (svgPath.Stroke != null)
            {
                var svgColourStroke = ((SvgColourServer)svgPath.Stroke).Colour;
                path_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };
            }
            else
            {
                path_stroke = new List<byte>() { 255, 0, 0, 0 };
            }
            if (svgPath.Fill != null)
            {
                var svgColourFill = ((SvgColourServer)svgPath.Fill).Colour;
                path_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };

            }
            else
            {
                path_fill = new List<byte>() { 255, 255, 255, 255 };
            }

            if (points.Count != 0)
            {
                ListFigureSvg figure = new ListFigureSvg(points,name, path_stroke, path_fill);
                this.figure_attributes.Add(figure);
            }
            
        }

        public void SaveToSVG(List<ListFigureSvg> figure_attributesToExport)
        {
            var svgDoc = new SvgDocument();
            for (int i = 0; i < figure_attributesToExport.Count; i++)
            {
                switch (figure_attributesToExport[i].name)
                {
                    case "rectangle" :
                       var rectangle =  SavePathRectangle(figure_attributesToExport[i]);
                       svgDoc.Children.Add(rectangle);
                        break;
                    case "line" :
                        var line =  SavePathLine(figure_attributesToExport[i]);
                        svgDoc.Children.Add(line);
                        break;
                    case "ellipse" :
                        var ellipse =  SaveEllipse(figure_attributesToExport[i]);
                        svgDoc.Children.Add(ellipse);
                        break;
                    case "path" :
                        var path =  SavePathBizie(figure_attributesToExport[i]);
                        svgDoc.Children.Add(path);
                        break;
                }
            }
        }
        
        private SvgEllipse SaveEllipse(ListFigureSvg ellip)
        {
            var a = new SvgEllipse()
            {
                CenterX = ellip.P_ellipse.X,
                CenterY = ellip.P_ellipse.Y,
                RadiusX = ellip.r1_ellipse,
                RadiusY = ellip.r2_ellipse,
                Fill = new SvgColourServer(Color.FromArgb(ellip.fill[0],ellip.fill[1],ellip.fill[2],ellip.fill[3])) ,
                Stroke = new SvgColourServer(Color.FromArgb(ellip.stroke[0],ellip.stroke[1],ellip.stroke[2],ellip.stroke[3]))
            };
            return a;
        }
        private SvgPath SavePathLine(ListFigureSvg Line)
        {
            SvgPathSegmentList Data = new SvgPathSegmentList();
            Data.Add(new SvgMoveToSegment(false, new PointF(Line.points[0].X,Line.points[0].Y))); 
            Data.Add(new SvgLineSegment(false,new PointF(Line.points[1].X, Line.points[1].Y)));
            Data.Add(new SvgClosePathSegment(true));
            var bezie_path = new SvgPath()
            {
                PathData = Data,
                Fill = new SvgColourServer(Color.FromArgb(Line.fill[0], Line.fill[1],Line.fill[2],Line.fill[3])) ,
                Stroke = new SvgColourServer(Color.FromArgb(Line.stroke[0], Line.stroke[1],Line.stroke[2],Line.stroke[3]))
            };
            return bezie_path;
        }
        private SvgPath SavePathRectangle(ListFigureSvg rectangle)
        {
            SvgPathSegmentList Data = new SvgPathSegmentList();
            Data.Add(new SvgMoveToSegment(false, new PointF(rectangle.points[0].X,rectangle.points[0].Y))); 
            Data.Add(new SvgLineSegment(false,new PointF(rectangle.points[1].X, rectangle.points[1].Y)));
            Data.Add(new SvgLineSegment(false,new PointF(rectangle.points[2].X, rectangle.points[2].Y)));
            Data.Add(new SvgLineSegment(false,new PointF(rectangle.points[3].X, rectangle.points[3].Y)));
            Data.Add(new SvgClosePathSegment(true));
            var bezie_path = new SvgPath()
            {
                PathData = Data,
                Fill = new SvgColourServer(Color.FromArgb(rectangle.fill[0], rectangle.fill[1],rectangle.fill[2],rectangle.fill[3])) ,
                Stroke = new SvgColourServer(Color.FromArgb(rectangle.stroke[0], rectangle.stroke[1],rectangle.stroke[2],rectangle.stroke[3]))
            };
            return bezie_path;
        }
        private SvgPath SavePathBizie(ListFigureSvg bezie)
        {
            SvgPathSegmentList Data = new SvgPathSegmentList();
            Data.Add(new SvgMoveToSegment(false, new PointF(bezie.points[0].X,bezie.points[0].Y))); 
            Data.Add(new SvgCubicCurveSegment(
                false,
                new PointF(bezie.points[1].X, bezie.points[1].Y),
                new PointF(bezie.points[2].X, bezie.points[2].Y),
                new PointF(bezie.points[3].X,bezie.points[3].Y))
            );
            Data.Add(new SvgClosePathSegment(true));
            var bezie_path = new SvgPath()
            {
                PathData = Data,
                Fill = new SvgColourServer(Color.FromArgb(bezie.fill[0], bezie.fill[1],bezie.fill[2],bezie.fill[3])) ,
                Stroke = new SvgColourServer(Color.FromArgb(bezie.stroke[0], bezie.stroke[1],bezie.stroke[2],bezie.stroke[3]))
            };
            return bezie_path;
        }
    }
}
