using Cosmos.System.Graphics.Fonts;
using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp2
{
	public class RasProcessData
	{
		public string[] code;
		public List<TextColor> lines = new List<TextColor>();
		public int CurrentLine;
		public string CurrentVoid;
		public bool waitForUserInput;
		public string toVariable;
		public bool syncInput = false, syncFIX;
		public string Input;
		public bool StopRenderConsole;
		public Color TextColor = Color.White;
		public Color BackgroundColor = Color.Black;
		public List<Element> text = new List<Element>();
	}

	public class Element
	{
		public string Text = "null";
		public Font Font;
		public int FontWidth;
		public int posX, posY, size;
		public bool center;
	}
}
