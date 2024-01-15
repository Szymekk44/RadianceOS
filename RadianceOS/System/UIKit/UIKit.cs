using Cosmos.System.Graphics;
using RadianceOS.System.NewApps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.UIKit
{
    public class UIKit
    {

        private List<UIelement> elements = new();
        private Bitmap render;
        public UIKit(Size size)
        {
            render = new((uint)size.Width,(uint)size.Height,ColorDepth.ColorDepth32);
        }

        public void Update(App app)
        {

            foreach (var element in elements)
            {
                element.Update(this,app.apprect);
            }

        }

        public Bitmap GetRender()
        {
            return render;
        }

        #region UIKIT
        public void Button(Rectangle rect,Color color,Action action)
        {
            DrawFilledRect(rect,color);
            elements.Add(new UIButton(rect,color,action));
        }

        #endregion

        #region BasicDrawFunctions
        void DrawPixel(int x, int y, Color col)
        {
            
            if (x <= render.Width && x <= render.Height)
            {
                render.RawData[(y * render.Width) - (render.Width - x)] = col.ToArgb();
            }

        }
        public void DrawFilledRect(Rectangle rect,Color color)
        {
            for (int i = rect.X; i < rect.X + rect.Width; i++)
            {
                for (int j = rect.Y; j < rect.Y + rect.Height; j++)
                {
                    DrawPixel(i,j,color);
                }
            }
        }
        #endregion
    }
}
