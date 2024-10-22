using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace makets.helper
{
    public static class ColorHelper
    {

        public static Color GetFromHex(string color)
        {
            
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString(color);
            return ((SolidColorBrush)brush).Color;
        }
       
    }
}
