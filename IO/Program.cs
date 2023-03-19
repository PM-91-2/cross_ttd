using System.Numerics;
using Svg;
using Svg.Pathing;

namespace IO {
    public class Svg
    {
        private string fileName = "template.svg";
        private List<ListFigureSvg> figure_attributes;
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
            var svgColourFill = ((SvgColourServer)svgLine.Fill).Colour;
            var svgColourStroke = ((SvgColourServer)svgLine.Stroke).Colour;
            List<byte> line_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };
            List<byte> line_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };
            ListFigureSvg Array = new ListFigureSvg(new Vector2(x1,y1),new Vector2(x2,y2),"line", line_stroke, line_fill, true);
            figure_attributes.Add(Array);
        }

        private void Rectangle(SvgElement svgElem)
        {
            SvgRectangle? svgRectangle = svgElem as SvgRectangle;
            var x_rect = svgRectangle.X.Value;
            var y_rect = svgRectangle.Y.Value;
            var Height = svgRectangle.Height.Value;
            var Width = svgRectangle.Width.Value;
            var svgColourFill = ((SvgColourServer)svgRectangle.Fill).Colour;
            var svgColourStroke = ((SvgColourServer)svgRectangle.Stroke).Colour;
            List<byte> rect_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };
            List<byte> rect_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };
            ListFigureSvg Array = new ListFigureSvg(new Vector2(x_rect,y_rect),new Vector2(x_rect+Width, y_rect+Height),"rectangle", rect_stroke, rect_fill); // To do
            figure_attributes.Add(Array);
        }

        private void Ellipse(SvgElement svgElem)
        {
            SvgEllipse? svgEllipse = svgElem as SvgEllipse;
            var rx = svgEllipse.RadiusX.Value;
            var ry = svgEllipse.RadiusY.Value;
            var x = svgEllipse.CenterX.Value;
            var y = svgEllipse.CenterY.Value;
            var svgColourFill = ((SvgColourServer)svgEllipse.Fill).Colour;
            var svgColourStroke = ((SvgColourServer)svgEllipse.Stroke).Colour;
            List<byte> ellipse_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };
            List<byte> ellipse_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };
            ListFigureSvg Array = new ListFigureSvg(rx,ry,new Vector2(x,y),"ellipse", ellipse_stroke, ellipse_fill);
            figure_attributes.Add(Array);
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
            var svgColourFill = ((SvgColourServer)svgPath.Fill).Colour;
            var svgColourStroke = ((SvgColourServer)svgPath.Stroke).Colour;
            List<byte> path_fill = new List<byte>() { svgColourFill.A, svgColourFill.R, svgColourFill.G, svgColourFill.B };
            List<byte> path_stroke = new List<byte>() { svgColourStroke.A, svgColourStroke.R, svgColourStroke.G, svgColourStroke.B };
            ListFigureSvg Array = new ListFigureSvg(points,"bezie", path_stroke, path_fill);
            figure_attributes.Add(Array);
        }

        public void SaveToSVG()
        {

        }
    }
}
