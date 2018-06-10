using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HyberShift_CSharp.Model
{
    public class EllipseModel
    {
        private Ellipse ellipse;
        private Point point;

        public EllipseModel()
        {
            ellipse = new Ellipse();
            point = new Point();
        }

        public EllipseModel(Ellipse elp, Point p)
        {
            ellipse = elp;
            point = p;
        }

        //getter and setter
        public double X { get => point.X; set => point.X = value; }
        public double Y { get => point.Y; set => point.Y = value; }
        public double Width { get => ellipse.Width; set => ellipse.Width = value; }
        public double Height { get => ellipse.Height; set => ellipse.Height = value; }
        public Brush Stroke { get => ellipse.Stroke; set => ellipse.Stroke = value; }
        public Brush Fill { get => ellipse.Fill; set => ellipse.Fill = value; }
        public double StrokeThickness { get => ellipse.StrokeThickness; set => ellipse.StrokeThickness = value; }
    }
}
