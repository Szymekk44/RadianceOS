using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.UIKit
{
    public class UIButton :UIelement
    {

        MouseState Pmstate = MouseState.None;
        Action action;
        Color col;
        public UIButton(Rectangle rect,Color col,Action action) :base(rect)
        {
            this.action = action;
            this.col = col;
        }
        public override void Update(UIKit ui, Rectangle apprect)
        {
            base.Update(ui,apprect);

            if (this.rect.IntersectsWith(new Rectangle((int)MouseManager.X - apprect.X, (int)MouseManager.Y - apprect.Y, (int)Kernel.Cursor1.Width,(int)Kernel.Cursor1.Height)))
            {

                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (Pmstate == MouseState.None)
                    {
                        Pmstate = MouseState.Left;
                        int R= col.R - 10,G= col.G - 10, B = col.B - 10;
                        if (R < 0)
                            R = 0;
                        if (G < 0)
                            G = 0;
                        if (B < 0)
                            B = 0;

                        ui.DrawFilledRect(this.rect,Color.FromArgb(R,G,B));

                    }
                }
                else if (MouseManager.MouseState == MouseState.None)
                {
                    if (Pmstate == MouseState.Left)
                    {
                        Pmstate = MouseState.None;
                        ui.DrawFilledRect(this.rect, col);
                        action.Invoke();
                    }
                }

            }

        }

    }
}
