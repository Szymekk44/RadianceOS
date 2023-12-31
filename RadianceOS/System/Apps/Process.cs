using Cosmos.System.Graphics;
using RadianceOS.System.Apps.Games;
using RadianceOS.System.Programming.RaSharp;
using RadianceOS.System.Programming.RaSharp2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RadianceOS.System.Apps
{
	public static class Process
	{
		public static List<Processes> Processes = new List<Processes>();
		public static void UpdateProcess(int index)
		{
			Processes[index].texts = Processes[index].Description.Split(new string[] { "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToArray();
		}
	}



	public class Processes
	{
		//MAIN
		public int ID;
		public string Name;
		public string Description;
		public int X, Y;
		public int SizeX, SizeY;
		public string metaData;
		
		//INPUT
		public int CurrLine;
		public int StartLine = 0;
		public int CurrChar;
		public string[] texts;
		public List<TextColor> lines = new List<TextColor>();
		public List<String> defaultLines = new List<String>();

		//SETTINGS
		public bool moveAble = true;
		public bool closeAble = true;
		public bool sizeAble = false;
		public bool hideAble = true;
		public bool hidden = false;
		public int MinX = 200, MinY = 100;
		public int notMaxX, notMaxY;
		public bool maximized;
		public bool selected;
		public bool saved;
		public bool saving;

		//TEMP
		public string temp;
		public bool tempBool;
		public int tempInt;
		public int tempInt2;
		public int tempInt3;
		public List<int> tempList;

		//OTHER
		public List<fragment> fragments;
		public List<fragment> fragments2;

		//SHADOWS (later)
		public List<Color> shadowDown;

		//OHNO
		public Bitmap bitmap;
        public Bitmap bitmap2;
        public Bitmap bitmap3;
        public Bitmap bitmapTop;
        public FileExplorerData FileExplorerDat;
		public WebData webData;

		public RasProcessData RasData;

	}
	public class WebData
	{
		public List<ElementData> elements;
	}
	public class ElementData
	{
		public int x, y, SizeX, SizeY;
		public int type = 0;
		public string url;
	}
	public class FileExplorerData
	{
		public List<FilePath> Files;
		public List<FilePath> MainDirectories;
		public List<FilePath> Directories;
	}
	public class FilePath
	{
		public string Name;
		public string Path;
		public string Extension;
		public bool menuOpen;
		public int menuX;
	}
}
