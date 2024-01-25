using Cosmos.System.Graphics.Fonts;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp2.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp2.Commands.Draw
{
	public static class DrawMain
	{
		public static void RunCommand(string com, int ProcessID)
		{
			string arg1 = com.Substring(com.IndexOf('.') + 1, com.IndexOf('(') - (com.IndexOf('.') + 1));
			string temp = com.Substring(com.IndexOf('(') + 1, com.LastIndexOf(')') - com.IndexOf('(') + 1);
			if (arg1 == "text")
			{

				string[] paramets = temp.Split(',');
				for (int i = 0; i < paramets.Length; i++)
				{
					paramets[i] = paramets[i].Trim();
					if (paramets[i].EndsWith(");"))
					{
						paramets[i] = paramets[i].Remove(paramets[i].Length - 2);
					}
				}
				string[] paramets2;
				if (paramets[0].Contains('+'))
					paramets2 = paramets[0].Split('+');
				else
				{
					paramets2 = new string[1];
					paramets2[0] = paramets[0];
				}
				string[] paramets3;


				if (paramets[1].Contains('+'))
					paramets3 = paramets[1].Split('+');
				else
				{
					paramets3 = new string[1];
					paramets3[0] = paramets[1];
				}
				string text = GetString.ReturnString(paramets2, ProcessID, com);
				string font = GetString.ReturnString(paramets3, ProcessID, com);
				string finale2 = "=" + paramets[2];
				string finale3 = "=" + paramets[3];
				int x = GetInt.ReturnInt(ProcessID, finale2, Process.Processes[ProcessID].DataID);
				int y = GetInt.ReturnInt(ProcessID, finale3, Process.Processes[ProcessID].DataID);
				if (x < 0)
					x = 0;
				if (y < 0)
					y = 0;
				Font newFont = Kernel.font18;
				if (font == "18")
				{
					newFont = Kernel.font18;
				}
				else if (font == "16")
				{
					newFont = Kernel.font16;
				}
				else if (font == "lt")
				{
					newFont = Kernel.fontLat;
				}
				else if (font == "rs")
				{
					newFont = Kernel.fontRuscii;
				}
				Element TextEl = new Element
				{
					Text = text,
					posX = x,
					Font = newFont,
					posY = y + 25,
				};
				Process.Processes[ProcessID].RasData.text.Add(TextEl);
			}
			else if (arg1 == "textC")
			{

				string[] paramets = temp.Split(',');
				for (int i = 0; i < paramets.Length; i++)
				{
					paramets[i] = paramets[i].Trim();
					if (paramets[i].EndsWith(");"))
					{
						paramets[i] = paramets[i].Remove(paramets[i].Length - 2);
					}
				}
				string[] paramets2;
				if (paramets[0].Contains('+'))
					paramets2 = paramets[0].Split('+');
				else
				{
					paramets2 = new string[1];
					paramets2[0] = paramets[0];
				}
				string[] paramets3;


				if (paramets[1].Contains('+'))
					paramets3 = paramets[1].Split('+');
				else
				{
					paramets3 = new string[1];
					paramets3[0] = paramets[1];
				}
				string text = GetString.ReturnString(paramets2, ProcessID, com);
				string font = GetString.ReturnString(paramets3, ProcessID, com);
				string finale2 = "=" + paramets[2];
				int y = GetInt.ReturnInt(ProcessID, finale2, Process.Processes[ProcessID].DataID);
				if (y < 0)
					y = 0;
				Font newFont = Kernel.font18;
				int width = 8;
				if (font == "18")
				{
					newFont = Kernel.font18;

				}
				else if (font == "16")
				{
					newFont = Kernel.font16;
					width = 6;
				}
				else if (font == "lt")
				{
					newFont = Kernel.fontLat;
				}
				else if (font == "rs")
				{
					newFont = Kernel.fontRuscii;
				}

				Element TextEl = new Element
				{
					Text = text,
					posX = 0,
					center = true,
					FontWidth = width,
					Font = newFont,
					posY = y + 25,
				};
				Process.Processes[ProcessID].RasData.text.Add(TextEl);
			}
		}
	}
}
