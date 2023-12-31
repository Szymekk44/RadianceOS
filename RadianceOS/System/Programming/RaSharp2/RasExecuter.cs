using Cosmos.Core;
using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp2
{
	public static class RasExecuter
	{
		public static void StartScript(string path)
		{
			string[] fragments = path.Split(@"\");
			Processes RasApp = new Processes
			{
				ID = 12,
				Name = fragments[fragments.Length - 1],
				X = 100,
				Y = 100,
				SizeX = 800,
				SizeY = 500,
				tempInt = 0,
				sizeAble = true,
				hideAble = true,
				moveAble = true
				
			

			};
			Process.Processes.Add(RasApp);
			Process.Processes[Process.Processes.Count - 1].RasData = new RasProcessData();
			Process.Processes[Process.Processes.Count - 1].RasData.lines = new List<TextColor>();
			Process.Processes[Process.Processes.Count - 1].RasData.CurrentLine = 0;
			Process.Processes[Process.Processes.Count - 1].RasData.code = File.ReadAllLines(path);
		}

		public static void Render(int X, int Y, int SizeX, int SizeY, int i)
		{
			try
			{
				if (!Process.Processes[i].RasData.waitForUserInput && Process.Processes[i].RasData.CurrentLine < Process.Processes[i].RasData.code.Length)
				{
					RasInterpreter.RunCommand(Process.Processes[i].RasData.code[Process.Processes[i].RasData.CurrentLine], i);
				}

			}
			catch(Exception ex)
			{
				Kernel.Crash("Ra# Error: " + ex.Message, 0);
			}
			int tempX = SizeX;
			if (SizeX + X > Explorer.screenSizeX)
			{
				tempX -= SizeX + X - (int)Explorer.screenSizeX;
			}
			Window.DrawTop(i, X, Y, SizeX, Process.Processes[i].Name , true, true, true, true, tempX);
			if (tempX == SizeX)
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, tempX, SizeY - 25);
			else
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 28, tempX, SizeY - 25);
			}
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, tempX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Color.Black, X + 2, Y + 27, tempX - 4, SizeY - 29);

			List<TextColor> texts = Process.Processes[i].RasData.lines;


			int start = 0;
			if (texts.Count >= SizeY / 18)
			{
				start = texts.Count + 1 - SizeY / 18;
			}
			for (int j = start; j < texts.Count; j++)
			{
				if (j + 1 < texts.Count)
					Explorer.CanvasMain.DrawString(texts[j].text, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
				else if (Process.Processes[i].selected && Process.Processes[i].RasData.waitForUserInput)
				{
					string result = texts[j].text.Substring(0, Process.Processes[i].CurrChar) + "_" + texts[j].text.Substring(Process.Processes[i].CurrChar);
					Explorer.CanvasMain.DrawString( result, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
				}
				else
					Explorer.CanvasMain.DrawString( texts[j].text, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
			}
			if (Process.Processes[i].selected && Process.Processes[i].RasData.waitForUserInput)
			{

				InputSystem.Monitore(1, Process.Processes[i].CurrChar, i);
				InputSystem.SpecialCharracters = true;
				InputSystem.AllowArrows = true;
				InputSystem.AllowUpDown = false;
				Process.Processes[i].lines[Process.Processes[i].lines.Count - 1].text = InputSystem.CurrentString;

			}
		}
	}
}
