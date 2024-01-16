using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.UIKit
{
    public class UIelement
    {
        public Rectangle rect;
        public UIelement(Rectangle rect) 
        {
            this.rect = rect;
        }

        public virtual void Update(UIKit ui,Rectangle apprect)
        {



        }

        public virtual void Redraw(UIKit ui, Rectangle apprect)
        {
            Update(ui,apprect);
        }

    }
}
