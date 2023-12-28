using Cosmos.System.Graphics;
using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp
{
	public class RasData
	{
		public string[] Commands;
		public List<TextColor> output;
		public bool inGraphic;
		public int CurrLine;
		public Color CurrColor = Color.White;
		public bool GetInput;
		public string tempStringName;
		public string currVoid;
		public Dictionary<string, object> strings = new Dictionary<string, object>();
		public Dictionary<string, int> ints = new Dictionary<string, int>();
		public Dictionary<string, int> methods;

	}
}
