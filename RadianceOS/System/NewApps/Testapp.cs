using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.NewApps
{
    public class Testapp:App
    {

        public Testapp(Rectangle apprect) :base (apprect,"testapp")
        {

        }
        public override void Start()
        {
            base.Start();

            ui.Button(new Rectangle(10, 10, 100, 40), Color.DarkBlue,() => { },"Button1");

        }

    }
}
