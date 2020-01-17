using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MathStudioWpf
{
    class CoordinatesConverter
    {
        public int ZoomLevel { get; set; }
        public double OffsetX { get; set; } = 0;
        public double OffsetY { get; set; } = 0;

        private double zoomValue
        {
            get
            {
                return Math.Pow(1.05, ZoomLevel) * 10;
            }
        }

        private double WXMax { get; set; }
        private double WXMin { get; set; }
        private double WYMax { get; set; }
        private double WYMin { get; set; }

        private double pixelWidth { get; set; }

        private Matrix WtoDMatrix, DtoWMatrix;
        public void PrepareMatrix(double dxmax, double dymax)
        {
            double difference = dxmax / dymax;
            // Make WtoD.
            WtoDMatrix = Matrix.Identity;

            double wxValue = zoomValue * difference;
            double wyValue = zoomValue;

            WtoDMatrix.Translate(wxValue, wyValue);


            double xscale = (dxmax) / (2 * wxValue);
            double yscale = (-dymax) / (2 * wyValue);

            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(0, dymax);

            WtoDMatrix.Translate(OffsetX, OffsetY);


            // Make DtoW.
            DtoWMatrix = WtoDMatrix;
            DtoWMatrix.Invert();

            Point maxPoint = DtoW(new Point(dxmax, dymax));
            Point minPoint = DtoW(new Point(0, 0));

            (WXMax, WYMin, WXMin, WYMax) = (maxPoint.X, maxPoint.Y, minPoint.X, minPoint.Y);

            Point oneone = DtoW(new Point(1, 1));

            pixelWidth = Math.Abs(oneone.X - minPoint.X);
        }

        public Point DtoW(Point point) => DtoWMatrix.Transform(point);
        public Point WtoD(Point point) => WtoDMatrix.Transform(point);
        public IEnumerable<Point> WtoDRange(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                yield return WtoD(point);
            }
        }
        public IEnumerable<Point> DtoWRange(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                yield return DtoW(point);
            }
        }

        public double GetWXMax() => WXMax * 1.1;
        public double GetWXMin() => WXMin * 1.1;
        public double GetWYMax() => WYMax * 1.1;
        public double GetWYMin() => WYMin * 1.1;

        public double GetPixelWidth() => pixelWidth / 2;
    }
}
