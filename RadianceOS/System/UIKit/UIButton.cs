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
        public Action action;
        public Color col,textcol = Color.White;
        public string text;
        public int rounding = 0;
        public UIButton(Rectangle rect, Color col, Action action, string text,int rounding = 0) :base(rect)
        {
            this.action = action;
            this.col = col;
            this.text = text;
            this.rounding = rounding;
        }
        public override void Update(UIKit ui, Rectangle apprect)
        {
            base.Update(ui,apprect);

            if (this.rect.IntersectsWith(new Rectangle(((int)MouseManager.X) - apprect.X, ((int)MouseManager.Y) - (apprect.Y+25), (int)Kernel.Cursor1.Width,(int)Kernel.Cursor1.Height)))
            {

                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (Pmstate == MouseState.None)
                    {
                        Pmstate = MouseState.Left;
                        int R= col.R - 10,G= col.G - 10, B = col.B - 10;
                        R = Math.Max(0,R);
                        G = Math.Max(0,G);
                        B = Math.Max(0,B);

                        if (rounding > 0)
                            ui.DrawRoundedRect(rect, rounding, Color.FromArgb(R, G, B));
                        else
                            ui.DrawFilledRect(rect, Color.FromArgb(R, G, B));
                        ui.DrawACSIIString(text,this.rect.X + (rect.Width/2 - (text.Length*8)/2),this.rect.Y + (rect.Height / 2 - 16 / 2), textcol);

                    }
                }
                else if (MouseManager.MouseState == MouseState.None)
                {
                    if (Pmstate == MouseState.Left)
                    {
                        Pmstate = MouseState.None;
                        if (rounding > 0)
                            ui.DrawRoundedRect(rect, rounding, col);
                        else
                            ui.DrawFilledRect(rect, col);
                        ui.DrawACSIIString(text, this.rect.X + (rect.Width / 2 - (text.Length * 8) / 2), this.rect.Y + (rect.Height / 2 - 16 / 2), textcol);
                        action.Invoke();
                    }
                }

            }

        }
        public override void Redraw(UIKit ui, Rectangle apprect)
        {
            if (rounding > 0)
                ui.DrawRoundedRect(rect, rounding, col);
            else
                ui.DrawFilledRect(rect, col);
            ui.DrawACSIIString(text, this.rect.X + (rect.Width / 2 - (text.Length * 8) / 2), this.rect.Y + (rect.Height / 2 - 16 / 2), textcol);
        }
    }
}
