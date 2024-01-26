using Cosmos.System;
using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.UIKit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.NewApps
{
    public class App
    {

        public Rectangle apprect;
        public Size MinSize = new (100,100),MaxSize = new(1280,720);
        public UIKit.UIKit ui;
        //Processes process;
        string appname;
        Point PreviousPos = Point.Empty;
        MouseState Pmstate = MouseState.None;
        int previousMousePosX = 0, previousMousePosY = 0;
        public App(Rectangle apprect,string appname = "App")
        {

            this.apprect = apprect;
            MinSize.Width = apprect.Width / 2;
            MinSize.Height = apprect.Height / 2;
            ui = new(apprect.Size);
            //process = new();
            //Apps.Process.Processes.Add(process);
            this.appname = appname;
            Start();

        }
        
        public virtual void Start()
        {



        }
        public virtual void Update() 
        {

            ui.Update(this);
            var bar = Window.DrawTop(apprect.X, apprect.Y, apprect.Width, appname, true, true, false, true);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, apprect.X + 3, apprect.Y + 28, apprect.Width, apprect.Height);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, apprect.X, apprect.Y + 25, apprect.Width, apprect.Height);
            Explorer.CanvasMain.DrawImage(ui.GetRender(), apprect.X, apprect.Y + 25);
            #region HitProcessings
            var mouse = new Rectangle((int)MouseManager.X, (int)MouseManager.Y, (int)Kernel.Cursor1.Width, (int)Kernel.Cursor1.Height);
            if (bar.IntersectsWith(mouse))
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (Pmstate == MouseState.None)
                    {
                        PreviousPos.X = (int)MouseManager.X -apprect.X;
                        PreviousPos.Y = (int)MouseManager.Y -apprect.Y;
                        Pmstate = MouseState.Left;
                    }
                    else if (Pmstate == MouseState.Left)
                    {

                        apprect.X = (int)MouseManager.X - PreviousPos.X;
                        apprect.Y = (int)MouseManager.Y - PreviousPos.Y;

                    }
                }
                else
                {
                    Pmstate = MouseState.None;
                }
            }
            if (new Rectangle(apprect.X + apprect.Width + 3, apprect.Y + 28,15,apprect.Height).IntersectsWith(mouse) && MouseManager.MouseState == MouseState.Left && previousMousePosX != mouse.X)
            {
                apprect.Width = Math.Max(MinSize.Width, Math.Min(MaxSize.Width, mouse.X - apprect.X));
                ui.Resize(this,apprect.Size);
            }
            if (new Rectangle(apprect.X + 3, apprect.Y +apprect.Height + 28, apprect.Width, 15).IntersectsWith(mouse) && MouseManager.MouseState == MouseState.Left && previousMousePosY != mouse.Y)
            {
                apprect.Height = Math.Max(MinSize.Height,Math.Min(MaxSize.Height,(mouse.Y) - (apprect.Y + 25)));
                ui.Resize(this, apprect.Size);
            }
            #endregion

        }
        public virtual void Stop() 
        { }
    }
}
