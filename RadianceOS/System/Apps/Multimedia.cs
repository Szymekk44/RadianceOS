using Cosmos.System.Graphics;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
	public static class Multimedia
	{
	
		public static void Start()
		{
			Processes MessageBox = new Processes
			{
				ID = 0,
				Name = "Error",
				Description = "Image was not converted!",
				metaData = "error",
				X = 500,
				Y = 200,
				SizeX = 300,
				SizeY = 200,
				moveAble = true
			};
			Apps.Process.Processes.Add(MessageBox);
			Apps.Process.UpdateProcess(Apps.Process.Processes.Count - 1);



		}

		public static void Render()
		{
			//Explorer.CanvasMain.DrawImage(ikona, 100, 100);
		}
	}
}
