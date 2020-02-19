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
        private double OffsetX { get; set; } = 0;
        private double OffsetY { get; set; } = 0;

        private double zoomValue
        {
            get
            {
                var returnValue = Math.Pow(Math.E, 1d / 20d * ZoomLevel) * 10;
                return returnValue;
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

            double xscale = (dxmax) / (2 * wxValue);
            double yscale = (-dymax) / (2 * wyValue);

            WtoDMatrix.Translate(wxValue, wyValue);

            WtoDMatrix.Translate(-OffsetX, -OffsetY);

            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(0, dymax);

            

            // Make DtoW
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

        public double GetWXMax() => WXMax;
        public double GetWXMin() => WXMin;
        public double GetWYMax() => WYMax;
        public double GetWYMin() => WYMin;

        public double GetPixelWidth() => pixelWidth / 2;

        public void Offset(double offsetX, double offsetY)
        {
            OffsetX += offsetX;
            OffsetY += offsetY;
        }

        public void Reset()
        {
            OffsetX = 0;
            OffsetY = 0;
            ZoomLevel = default;
        }
    }
}
