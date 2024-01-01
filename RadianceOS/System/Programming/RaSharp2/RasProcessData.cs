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
		public bool waitForUserInput;
		public bool syncInput;
		public string Input;
		public Color TextColor = Color.White;
		public Color BackgroundColor = Color.Black;
		public Dictionary<string, string> Variables = new Dictionary<string, string>();
	}
}
