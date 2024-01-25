using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security.FileManagment
{
	public static class FileAction
	{
		public static void DelteFile(string path)
		{
			if (path.EndsWith(".SysData") && !Kernel.Root)
			{
				MessageBoxCreator.CreateMessageBox("Permission denied", path + "\nThis system file cannot be deleted!", MessageBoxCreator.MessageBoxIcon.STOP);
				return;
			}
			if (path.StartsWith(@"1:\") && !Kernel.Root)
			{
				MessageBoxCreator.CreateMessageBox("Permission denied", path + "\nThis file is located on a read-only drive", MessageBoxCreator.MessageBoxIcon.STOP);
				return;
			}

			File.Delete(path);
		}
	}
}
