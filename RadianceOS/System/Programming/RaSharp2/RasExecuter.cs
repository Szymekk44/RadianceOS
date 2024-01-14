using Cosmos.Core;
using Cosmos.System.Graphics.Fonts;
using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp;
using RadianceOS.System.Programming.RaSharp2.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;



namespace RadianceOS.System.Programming.RaSharp2
{
	public static class RasExecuter
	{
		public static List<RasVariablesMemory> Data = new List<RasVariablesMemory>();
		public static int AllApps;
		public static void StartScript(string path)
		{
			try
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
				Process.Processes[Process.Processes.Count - 1].RasData.StopRenderConsole = false;
				Process.Processes[Process.Processes.Count - 1].DataID = AllApps;
				Data.Add(new RasVariablesMemory());
				for (int i = 0; i < Process.Processes[Process.Processes.Count - 1].RasData.code.Length; i++)
				{
					Process.Processes[Process.Processes.Count - 1].RasData.code[i] = Process.Processes[Process.Processes.Count - 1].RasData.code[i].Trim();
					if (Process.Processes[Process.Processes.Count - 1].RasData.code[i].StartsWith("void"))
					{
					Data[AllApps].voids.Add(Process.Processes[Process.Processes.Count - 1].RasData.code[i].Substring(5, Process.Processes[Process.Processes.Count - 1].RasData.code[i].Length - 6).Trim(), i);
					//	MessageBoxCreator.CreateMessageBox("Found!", "name: " + Process.Processes[Process.Processes.Count - 1].RasData.code[i].Substring(5, Process.Processes[Process.Processes.Count - 1].RasData.code[i].Length - 6).Trim() + "\nLine: " + i, MessageBoxCreator.MessageBoxIcon.info, 600);
					}
				}
				AllApps++;
			}
			catch (Exception ex)
			{
				MessageBoxCreator.CreateMessageBox("Ra# Executer Error", ex.Message, MessageBoxCreator.MessageBoxIcon.STOP, 600);
			}

		}

		public static void Render(int X, int Y, int SizeX, int SizeY, int i)
		{
			try
			{
				if (!Process.Processes[i].RasData.waitForUserInput && Process.Processes[i].RasData.CurrentLine < Process.Processes[i].RasData.code.Length)
				{
					RasInterpreter.RunCommand(Process.Processes[i].RasData.code[Process.Processes[i].RasData.CurrentLine], i);
				}
				if (Process.Processes[i].RasData.waitForUserInput)
				{
					if (Process.Processes[i].RasData.syncInput)
					{


						GetString.ChangeSrtingInput(i, Process.Processes[i].RasData.toVariable, InputSystem.CurrentString);
						Process.Processes[i].RasData.syncInput = false;
						Process.Processes[i].RasData.waitForUserInput = false;


					}
				}

				int tempX = SizeX;
				if (SizeX + X > Explorer.screenSizeX)
				{
					tempX -= SizeX + X - (int)Explorer.screenSizeX;
				}
				Window.DrawTop(i, X, Y, SizeX, Process.Processes[i].Name, Process.Processes[i].sizeAble, Process.Processes[i].closeAble, true, Process.Processes[i].hideAble, tempX);
				if (tempX == SizeX)
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, tempX, SizeY - 25);
				else
				{
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 28, tempX, SizeY - 25);
				}
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, tempX, SizeY - 25);
				if (!Process.Processes[i].RasData.StopRenderConsole)
				{
					Explorer.CanvasMain.DrawFilledRectangle(Color.Black, X + 2, Y + 27, tempX - 4, SizeY - 29);

					List<TextColor> texts = Process.Processes[i].RasData.lines;



					int start = 0;
					if (texts.Count >= SizeY / 18)
					{
						start = texts.Count + 1 - SizeY / 18;
					}
					for (int j = start; j < texts.Count; j++)
					{
						string tempText = texts[j].text;
						if (tempText.Length > SizeX / 8)
						{
							tempText = tempText.Substring(0, SizeX / 8 - 1);
						}
						if (j + 1 < texts.Count)
							Explorer.CanvasMain.DrawString(tempText, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
						else if (Process.Processes[i].selected && Process.Processes[i].RasData.waitForUserInput)
						{
							string result;
							if (tempText.Length > 0)
								result = tempText.Substring(0, Process.Processes[i].CurrChar) + "_" + tempText.Substring(Process.Processes[i].CurrChar);
							else
								result = "_";
							Explorer.CanvasMain.DrawString(result, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
						}
						else
							Explorer.CanvasMain.DrawString(tempText, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
					}
					if (Process.Processes[i].selected && Process.Processes[i].RasData.waitForUserInput)
					{

						InputSystem.Monitore(6, Process.Processes[i].CurrChar, i);
						InputSystem.SpecialCharracters = true;
						InputSystem.AllowArrows = true;
						InputSystem.AllowUpDown = false;
						Process.Processes[i].RasData.lines[Process.Processes[i].RasData.lines.Count - 1].text = InputSystem.CurrentString;
						if (Process.Processes[i].lines.Count < 1)
							Process.Processes[i].lines.Add(new TextColor());
						Process.Processes[i].lines[0].text = InputSystem.CurrentString;
					}
				}
				else
				{
					for (int j = 0; j < Process.Processes[i].RasData.text.Count; j++)
					{
						int XPos = 0 ;
						if (Process.Processes[i].RasData.text[j].center)
							XPos = (Process.Processes[i].SizeX - Process.Processes[i].RasData.text[j].Text.Length * Process.Processes[i].RasData.text[j].FontWidth) / 2;
						else
							XPos = Process.Processes[i].RasData.text[j].posX;
						Explorer.CanvasMain.DrawString(Process.Processes[i].RasData.text[j].Text, Process.Processes[i].RasData.text[j].Font, Color.White, XPos + X, Process.Processes[i].RasData.text[j].posY + Y);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBoxCreator.CreateMessageBox("Ra# Error", ex.Message + "\nLine: " + Process.Processes[i].RasData.CurrentLine, MessageBoxCreator.MessageBoxIcon.error, 600);
				Process.Processes[i].RasData.CurrentLine++;
			}
		}

	}
}
