using Cosmos.System.Graphics;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp2.Commands.Console;
using RadianceOS.System.Programming.RaSharp2.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp2.Commands.Window
{
	public static class RasWindow
	{
		public static void RunCommand(string[] paramets, string[] dots, string com, int ProcessID)
		{
			string arg2 = com.Substring(com.IndexOf('=') + 1, com.Length - com.IndexOf('=') - 2);
			arg2 = arg2.Trim();
			string arg1 = dots[1].Substring(0, dots[1].IndexOf('='));
			arg1 = arg1.Trim();
			if (arg1 == "mode")
			{
				int mode = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				switch(mode)
				{
					case 0:
						Apps.Process.Processes[ProcessID].RasData.StopRenderConsole = false;
						break;
					case 1:
						Apps.Process.Processes[ProcessID].RasData.StopRenderConsole = true;
						break;
					default:
						MessageBoxCreator.CreateMessageBox("Ra# Error", "Mode: " + mode + " not implemented!", MessageBoxCreator.MessageBoxIcon.error,400);
						break;
				}
			}
			else if(arg1 == "sizeAble")
			{
				int mode = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				switch (mode)
				{
					case 0:
						Apps.Process.Processes[ProcessID].sizeAble = false;
						break;
					case 1:
						Apps.Process.Processes[ProcessID].sizeAble = true;
						break;
					default:
						MessageBoxCreator.CreateMessageBox("Ra# Error", "SizeAble: " + mode + " not implemented!", MessageBoxCreator.MessageBoxIcon.error,400);
						break;
				}
			}
			else if (arg1 == "height" || arg1 == "sizeY")
			{
				int size = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				if(size < 25)
				{
					MessageBoxCreator.CreateMessageBox("Ra# Error", "You cannot set the window\nheight to less than 25.", MessageBoxCreator.MessageBoxIcon.error, 500);
					return;
				}
				Apps.Process.Processes[ProcessID].SizeY = size;
			}
			else if (arg1 == "width" || arg1 == "sizeX")
			{
				int size = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				if (size < 40)
				{
					MessageBoxCreator.CreateMessageBox("Ra# Error", "You cannot set the window\n width to less than 40.", MessageBoxCreator.MessageBoxIcon.error, 500);
					return;
				}
				Apps.Process.Processes[ProcessID].SizeX = size;
			}
			else if (arg1 == "minHeight" || arg1 == "minSizeY")
			{
				int size = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				if (size < 25)
				{
					MessageBoxCreator.CreateMessageBox("Ra# Error", "You cannot set the window\nmin height to less than 25.", MessageBoxCreator.MessageBoxIcon.error, 500);
					return;
				}
				Apps.Process.Processes[ProcessID].MinY = size;
			}
			else if (arg1 == "minWidth" || arg1 == "minSizeX")
			{
				int size = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				if (size < 40)
				{
					MessageBoxCreator.CreateMessageBox("Ra# Error", "You cannot set the window\nmin width to less than 40.", MessageBoxCreator.MessageBoxIcon.error, 500);
					return;
				}
				Apps.Process.Processes[ProcessID].MinX = size;
			}
			else if (arg1 == "moveAble")
			{
				int mode = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				switch (mode)
				{
					case 0:
						Apps.Process.Processes[ProcessID].moveAble = false;
						break;
					case 1:
						Apps.Process.Processes[ProcessID].moveAble = true;
						break;
					default:
						MessageBoxCreator.CreateMessageBox("Ra# Error", "MoveAble: " + mode + " not implemented!", MessageBoxCreator.MessageBoxIcon.error, 400);
						break;
				}
			}
			else if (arg1 == "posX")
			{
				int size = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				if (size < 0 || Explorer.screenSizeX < Apps.Process.Processes[ProcessID].X + size)
				{
					MessageBoxCreator.CreateMessageBox("Ra# Error", "Cannot change window Window X Position", MessageBoxCreator.MessageBoxIcon.error, 500);
					return;
				}
				Apps.Process.Processes[ProcessID].X = size;
			}
			else if (arg1 == "posY")
			{
				int size = GetInt.ReturnInt(ProcessID, com, Apps.Process.Processes[ProcessID].DataID);
				if (size < 0 || Explorer.screenSizeY < Apps.Process.Processes[ProcessID].Y + size)
				{
					MessageBoxCreator.CreateMessageBox("Ra# Error", "Cannot change window Window Y Position", MessageBoxCreator.MessageBoxIcon.error, 500);
					return;
				}
				Apps.Process.Processes[ProcessID].Y = size;
			}
		}
	}
}
