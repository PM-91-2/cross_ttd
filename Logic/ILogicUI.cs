using System.Drawing;
using Logic.Common;

namespace Logic;

public interface ILogicUI
{
    public void FigureButtonTouched(FigureButtonType buttonType, Point origin, Size size);
    public void FigureTransformationButtonTouched(FigureTransformationButtonType buttonType, TransformationValue value);
    public void SystemButtonTouched(SystemButtonType buttonType);
}