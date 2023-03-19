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
            ListFigureSvg figure = new ListFigureSvg(new Vector2(x1,y1),new Vector2(x2,y2),"line", line_stroke, line_fill, true);
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
            
            ListFigureSvg figure = new ListFigureSvg(new Vector2(x_rect,y_rect),new Vector2(x_rect+Width, y_rect+Height),"rectangle", rect_stroke, rect_fill); // To do
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
            //string fill ="";
            List<Vector2> points = new List<Vector2>();
            SvgCubicCurveSegment a = (SvgCubicCurveSegment)svgPath.PathData[1];
            if (svgPath.PathData.Count == 3)
            {
                points.Add(new Vector2(svgPath.PathData[0].End.X, svgPath.PathData[0].End.Y));
                points.Add(new Vector2(a.FirstControlPoint.X, a.FirstControlPoint.Y));
                points.Add(new Vector2(a.SecondControlPoint.X, a.SecondControlPoint.Y));
                points.Add(new Vector2(a.End.X, a.End.Y));
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
            ListFigureSvg figure = new ListFigureSvg(points,"bezie", path_stroke, path_fill);
            this.figure_attributes.Add(figure);
        }

        public void SaveToSVG(List<ListFigureSvg> figure_attributesToExport)
        {
            var svgDoc = new SvgDocument();
            for (int i = 0; i < figure_attributesToExport.Count; i++)
            {
                switch (figure_attributesToExport[i].name)
                {
                    case "rectangle" :
                       var rectangle =  SaveRectangle(figure_attributesToExport[i]);
                       svgDoc.Children.Add(rectangle);
                        break;
                    case "line" :
                        var line =  SaveLine(figure_attributesToExport[i]);
                        svgDoc.Children.Add(line);
                        break;
                    case "ellipse" :
                        var ellipse =  SaveEllipse(figure_attributesToExport[i]);
                        svgDoc.Children.Add(ellipse);
                        break;
                    case "path" :
                        var path =  SavePath(figure_attributesToExport[i]);
                        svgDoc.Children.Add(path);
                        break;
                }
            }
        }
 
        private SvgRectangle SaveRectangle(ListFigureSvg rect)
        {
            var a = new SvgRectangle
            {
                X = rect.P1_rect.X,
                Y = rect.P1_rect.Y,
                Width = rect.P2_rect.X - rect.P1_rect.X,
                Height = rect.P2_rect.Y - rect.P1_rect.Y,
                Fill = new SvgColourServer(Color.FromArgb(rect.fill[0],rect.fill[1],rect.fill[2],rect.fill[3])) ,
                Stroke = new SvgColourServer(Color.FromArgb(rect.stroke[0],rect.stroke[1],rect.stroke[2],rect.stroke[3]))
            };
            return a;
        }
        private SvgLine SaveLine(ListFigureSvg line)
        {
            var a = new SvgLine
            {
                StartX = line.P1_line.X,
                StartY = line.P1_line.Y,
                EndX = line.P2_line.X,
                EndY = line.P2_line.Y,
                Fill = new SvgColourServer(Color.FromArgb(line.fill[0],line.fill[1],line.fill[2],line.fill[3])) ,
                Stroke = new SvgColourServer(Color.FromArgb(line.stroke[0],line.stroke[1],line.stroke[2],line.stroke[3]))
            };
            return a;
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
        private SvgPath SavePath(ListFigureSvg bezie)
        {
            SvgPathSegmentList Data = new SvgPathSegmentList();
            Data.Add(new SvgMoveToSegment(false, new PointF(bezie.points_bezie[0].X,bezie.points_bezie[0].Y))); 
            Data.Add(new SvgCubicCurveSegment(
                false,
                new PointF(bezie.points_bezie[1].X, bezie.points_bezie[1].Y),
                new PointF(bezie.points_bezie[2].X, bezie.points_bezie[2].Y),
                new PointF(bezie.points_bezie[3].X,bezie.points_bezie[3].Y))
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
