using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.UIKit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.NewApps
{
    public class App
    {

        public Rectangle apprect;
        public UIKit.UIKit ui;
        //Processes process;
        int pid = 0;
        string appname;
        public App(Rectangle apprect,string appname = "App")
        {

            this.apprect = apprect;
            ui = new(apprect.Size);
            //process = new();
            //Process.Processes.Add(process);
            pid = Process.Processes.Count - 1;
            this.appname = appname;
            Start();

        }

        public virtual void Start()
        {



        }
        public virtual void Update() 
        {

            ui.Update(this);
            //Window.DrawTop(pid, apprect.X, apprect.Y, apprect.Width, appname, true, true, false, true);
            Explorer.CanvasMain.DrawImage(ui.GetRender(),apprect.X,apprect.Y);

        }
        public virtual void Stop() 
        { }
    }
}
