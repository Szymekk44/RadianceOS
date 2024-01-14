using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL2CPU.API.Attribs;
using SystemDrawing = System.Drawing;
namespace RadianceOS.System.Managment
{
    public static class Files
    {
		

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Fonts.zap-ext-light16.psf")]
        public static byte[] Font16;
        [ManifestResourceStream(ResourceName = "RadianceOS.Resources.Fonts.zap-ext-light18.psf")]
        public static byte[] Font18;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Fonts.lat9w-16.psf")]
		public static byte[] FontLat;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Fonts.ruscii_8x16.psf")]
		public static byte[] FontRuscii;


		[ManifestResourceStream(ResourceName = "RadianceOS.TTF.Fonts.UbuntuMono-Regular.ttf")]
		public static byte[] UbuntuMonoRegular;

		[ManifestResourceStream(ResourceName = "RadianceOS.TTF.Fonts.UbuntuMono-Bold.ttf")]
		public static byte[] UbuntuMonoBold;

		[ManifestResourceStream(ResourceName = "RadianceOS.TTF.Fonts.SometypeMono-Regular.ttf")]
		public static byte[] STRegualr;

		[ManifestResourceStream(ResourceName = "RadianceOS.TTF.Fonts.Cousine-Regular.ttf")]
		public static byte[] CousineRegular;

		[ManifestResourceStream(ResourceName = "RadianceOS.TTF.Fonts.Cousine-Bold.ttf")]
		public static byte[] CousineBold;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Wallpapers.wallpaper-tree.bmp")]
        public static byte[] wallpaper1;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Wallpapers.wallpaper-treeSmall.bmp")]
		public static byte[] wallpaper1S;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Wallpapers.wallpaper-tree2.bmp")]
		public static byte[] wallpaper2;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Wallpapers.wallpaper-tree2Small.bmp")]
		public static byte[] wallpaper2S;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.RadianceOS.RadianceOS icon.bmp")]
		public static byte[] RadianceOSIcon;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.RadianceOS.RadianceOS iconTransparent.bmp")]
		public static byte[] RadianceOSIconTransparent;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Start.buttonLight.bmp")]
		public static byte[] LButton;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Start.buttonDark.bmp")]
		public static byte[] DButton;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.error.bmp")]
        public static byte[] warning;
        [ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.stop.bmp")]
        public static byte[] stop;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.info.bmp")]
		public static byte[] info;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.criticalStop.bmp")]
		public static byte[] criticalStop;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.noDisk.bmp")]
		public static byte[] disk;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.cmd.bmp")]
		public static byte[] cmd;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.notepad.bmp")]
		public static byte[] notepad;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.setting icon.bmp")]
		public static byte[] setting;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.gamepad icon.bmp")]
		public static byte[] gamepad;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.sysinfo icon.bmp")]
		public static byte[] sysinfo;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.padlock.bmp")]
		public static byte[] padlock;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.RadiantWave.bmp")]
		public static byte[] RadiantWave;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.fileExplorer.bmp")]
		public static byte[] FileExplorer;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.user dark theme.bmp")]
		public static byte[] udt;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.user light theme.bmp")]
		public static byte[] ult;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.FileExplorer.text16.bmp")]
		public static byte[] text16;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.FileExplorer.document16.bmp")]
		public static byte[] document16;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.FileExplorer.folder16.bmp")]
		public static byte[] folder16;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.FileExplorer.data16.bmp")]
		public static byte[] data16;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.FileExplorer.sysData16.bmp")]
		public static byte[] sysData16;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.FilesIcons.txt.bmp")]
		public static byte[] txt;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.FilesIcons.ras.bmp")]
		public static byte[] ras;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.FilesIcons.unknown.bmp")]
		public static byte[] unknown;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.Xicon.bmp")]
		public static byte[] Xicon;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Icons.crash.bmp")]
		public static byte[] crash;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Installer.RadianceInstaller.bmp")]
        public static byte[] BlurWindow;
        [ManifestResourceStream(ResourceName = "RadianceOS.Resources.Installer.ButtonInstaller.bmp")]
        public static byte[] BlurButton;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Installer.ButtonInstaller2.bmp")]
		public static byte[] BlurButton2;
	

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Cursors.cursor.bmp")]
        public static byte[] cursor1;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Audio.startup.wav")]
		public static byte[] startupAduio;
	}
}
