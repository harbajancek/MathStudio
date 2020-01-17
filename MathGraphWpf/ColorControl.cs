using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MathStudioWpf
{
    static class ColorControl
    {
        static private List<Brush> brushes = new List<Brush>()
        {
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Purple,
            Brushes.Orange,
            Brushes.Pink,
            Brushes.Brown
        };
        static private int index = 0;

        static public Brush GetBrush()
        {
            index++;
            if (index == brushes.Count)
            {
                index = 0;
            }
            return brushes[index];
        }

        static public void Reset()
        {
            index = 0;
        }
    }
}
