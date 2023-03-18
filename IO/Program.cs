using Svg;

namespace IO {
    public class Svg
    {
        private string fileName = "template.svg";
        private List<ListFigureSvg> asda;
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

            return asda;
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
            ListFigureSvg Array = new ListFigureSvg(1,1,1);// To do
            asda.Add(Array);
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
            ListFigureSvg Array = new ListFigureSvg(1,1,1); // To do
            asda.Add(Array);
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
            ListFigureSvg Array = new ListFigureSvg(1, 1, 1);// To do
            asda.Add(Array);
        }

        private void Path(SvgElement svgElem)
        {
            SvgPath? svgPath = svgElem as SvgPath;
            string a = "";
            //string fill ="";
            for (int i = 0; i < svgPath.PathData.Count; i++)
            {
                a += svgPath.PathData[i] + " ";

            }

            var svgColourFill = ((SvgColourServer)svgPath.Fill).Colour;
            var svgColourStroke = ((SvgColourServer)svgPath.Stroke).Colour;
            ListFigureSvg Array = new ListFigureSvg(1, 1,1);// To do
            asda.Add(Array);
        }

        public void SaveToSVG()
        {

        }
    }
}
