using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp
{
	public static class RasRender
	{
		public static void Render(int ProcessID)
		{
			if (!RasPerformer.Data[Process.Processes[ProcessID].tempInt].inGraphic)
			{
				if(!RasPerformer.Data[Process.Processes[ProcessID].tempInt].GetInput)
				{
					if (RasPerformer.Data[Process.Processes[ProcessID].tempInt].CurrLine < RasPerformer.Data[Process.Processes[ProcessID].tempInt].Commands.Length - 1)
					{
						RasInvoker.RunCommand(RasPerformer.Data[Process.Processes[ProcessID].tempInt].Commands[RasPerformer.Data[Process.Processes[ProcessID].tempInt].CurrLine], Process.Processes[ProcessID].tempInt, ProcessID);
						RasPerformer.Data[Process.Processes[ProcessID].tempInt].CurrLine++;
					}
				}
				
				RenderConsole(ProcessID, Process.Processes[ProcessID].tempInt);
				
			}
		}

		public static void RenderConsole(int i, int DataID)
		{
			List<TextColor> texts = RasPerformer.Data[DataID].output;
			int X = Process.Processes[i].X;
			int Y = Process.Processes[i].Y;
			int SizeX = Process.Processes[i].SizeX;
			int SizeY = Process.Processes[i].SizeY;
			Window.DrawTop(i,X, Y, SizeX, Process.Processes[i].Name);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Color.Black, X + 2, Y + 27, SizeX - 4, SizeY - 29);


			int start = 0;
			if (texts.Count >= SizeY / 18)
			{
				start = texts.Count + 1 - SizeY / 18;
			}
			for (int j = start; j < texts.Count; j++)
			{
				if (j + 1 < texts.Count)
					Explorer.CanvasMain.DrawString(texts[j].text, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
				else if (Process.Processes[i].selected && RasPerformer.Data[Process.Processes[i].tempInt].GetInput)
				{
					string result = texts[j].text.Substring(0, Process.Processes[i].CurrChar) + "_" + texts[j].text.Substring(Process.Processes[i].CurrChar);
					Explorer.CanvasMain.DrawString(result, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
				}
				else
					Explorer.CanvasMain.DrawString(texts[j].text, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
			}
			if (Process.Processes[i].selected && RasPerformer.Data[Process.Processes[i].tempInt].GetInput)
			{

				InputSystem.Monitore(4, Process.Processes[i].CurrChar, i);
				InputSystem.SpecialCharracters = true;
				InputSystem.AllowArrows = true;
				InputSystem.AllowUpDown = false;
				//Process.Processes[i].lines[Process.Processes[i].lines.Count - 1].text = InputSystem.CurrentString;
				 RasPerformer.Data[Process.Processes[i].tempInt].output[RasPerformer.Data[Process.Processes[i].tempInt].output.Count - 1].text = InputSystem.CurrentString;
			}
		}
	}
}
