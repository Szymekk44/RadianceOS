using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Managment
{
    public class MouseHelpers
    {
        public static float clicked = 0;
        public static float clicktime = 0;
        public static float clickdelay = 1f;

        public static bool DoubleClick()
        {
            clicked++;
            if (clicked == 1) clicktime = float.Parse(DateTime.Now.ToString("ss"));

            if (clicked > 1 && float.Parse(DateTime.Now.ToString("ss")) - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                return true;
            }
            else if (clicked > 2 || float.Parse(DateTime.Now.ToString("ss")) - clicktime > 1) clicked = 0;

            return false;
        }
    }
}
