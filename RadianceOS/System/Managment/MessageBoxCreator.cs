using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace RadianceOS.System.Managment
{
	public static class MessageBoxCreator
	{
		public static void CreateMessageBox(string title, string message,MessageBoxIcon Icon = MessageBoxIcon.info, int SizeX = 300, int SizeY = 175, string Button1 = "OK", string Button2 = null)
		{
			string MetaData = "";
			switch(Icon)
			{
				case MessageBoxIcon.info:
					MetaData = "info";
					break;
				case MessageBoxIcon.warning:
					MetaData = "warning";
					break;
				case MessageBoxIcon.error:
					MetaData = "error";
					break;
				case MessageBoxIcon.diskError:
					MetaData = "diskError";
					break;
				case MessageBoxIcon.STOP:
					MetaData = "criticalStop";
					break;
			}

			Processes MessageBox = new Processes
			{
				ID = 0,
				Name = title,
				Description = message,
				metaData = MetaData,
				X = 150,
				Y = 150,
				SizeX = SizeX,
				SizeY = SizeY,
				saved = true
		
			};
			Apps.Process.Processes.Add(MessageBox);
			Apps.Process.UpdateProcess(Apps.Process.Processes.Count - 1);
			RadianceOS.System.Apps.MessageBox.closedWith.Add(0);
			Apps.Process.Processes[Apps.Process.Processes.Count - 1].DataID = RadianceOS.System.Apps.MessageBox.closedWith.Count - 1;
			Apps.Process.Processes[Apps.Process.Processes.Count - 1].tempBool = true;
			Apps.Process.Processes[Apps.Process.Processes.Count - 1].defaultLines.Add(Button1);
			Apps.Process.Processes[Apps.Process.Processes.Count - 1].defaultLines.Add(Button2);
			if (Button2 != null)
				Apps.Process.Processes[Apps.Process.Processes.Count - 1].closeAble = false;
		}
		public enum MessageBoxIcon
		{
			info,
			warning,
			error,
			diskError,
			STOP
		}
	}
}
