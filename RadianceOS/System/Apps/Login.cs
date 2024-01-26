using CosmosTTF;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using RadianceOS.System.Radiance;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
    public static class Login
	{
		public static bool Clicked;
		public static void Render(int X, int Y, int SizeX, int SizeY, int i)
		{
			try
			{
                InputSystem.Monitore(0, Apps.Process.Processes[i].CurrChar, i);
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                Window.DrawTop(i, X, Y, SizeX, "User authentication", false, false, true, false);
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
                //Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X, Y, SizeX, 25);
                Explorer.CanvasMain.DrawImage(Kernel.padlockIcon, X + 20, Y + 50);
                if (Clicked)
                {
                    if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.None)
                    {
                        Clicked = false;
                    }
                }
                switch (Apps.Process.Processes[i].tempInt)
                {
                    case 0:
                        {
                            InputSystem.AllowUpDown = false;
                            InputSystem.SpecialCharracters = true;
                            InputSystem.AllowArrows = true;
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X + 160, Y + 75, 250, 25);
                            Explorer.CanvasMain.DrawStringTTF("Username", "CR", Color.White, 17, X + 163, Y + 56 + 15);
                            Explorer.CanvasMain.DrawStringTTF(Kernel.loggedUser, "CR", Color.White, 17, X + 163, Y + 78 + 15);
                            string result;
                            if (InputSystem.CurrentString.Length > 0)
                                result = InputSystem.CurrentString;
                            else
                                result = "";
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X + 160, Y + 125, 250, 25);
                            Explorer.CanvasMain.DrawStringTTF("Password", "CR", Color.White, 17, X + 163, Y + 106 + 15);
                            Explorer.CanvasMain.DrawStringTTF(new string('•', result.Length) + "|", "CR", Color.White, 17, X + 163, Y + 128 + 15);




                            if (Explorer.MY > Y + SizeY - 60 && Explorer.MY < Y + SizeY - 30)
                            {
                                if (Explorer.MX > X + 310 && Explorer.MX < X + 310 + 100)
                                {
                                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + 310, Y + SizeY - 60, 100, 30);
                                    if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left && !Clicked)
                                    {
                                        string myUser = @"0:\Users\" + Kernel.loggedUser + @"\";
                                        if (InputSystem.CurrentString == File.ReadAllText(myUser + @"AccountInfo\Password.SysData"))
                                        {
                                            if (File.Exists(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat"))
                                            {
                                                try
                                                {
                                                    int theme = int.Parse(File.ReadAllText(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat"));
                                                    Design.ChangeTheme(theme);
                                                    Kernel.Theme = theme;
                                                }
                                                catch(Exception e)
                                                {
                                                    MessageBoxCreator.CreateMessageBox("Config Erorr", "Theme config was corrupted!\nRadianceOS has restored default settings." + e.Message, MessageBoxCreator.MessageBoxIcon.warning, 500, 175);
                                                    File.Delete(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat");
                                                    File.Create(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat");
                                                    File.WriteAllText(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat", "0");
                                                }
                                               
                                            }
                                            Radiance.Security.LogIn(); // Patched

                                        }
                                        else
                                        {
                                            Apps.Process.Processes[i].tempInt = 1;
                                            Clicked = true;
                                        }
                                    }
                                }
                                else
                                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 310, Y + SizeY - 60, 100, 30);
                            }
                            else
                                Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 310, Y + SizeY - 60, 100, 30);
                            StringsAcitons.DrawCenteredTTFString("Login", 100, X + 310, Y + SizeY - 40, 15, Color.White, "UMR", 17);
                        }
                        break;
                    case 1:
                        {
                            InputSystem.AllowUpDown = false;
                            InputSystem.SpecialCharracters = false;
                            InputSystem.AllowArrows = false;
                            InputSystem.CurrentString = "";
                            Apps.Process.Processes[i].CurrChar = 0;
                            Explorer.CanvasMain.DrawStringTTF("Wrong password!", "UMR", Color.White, 17, X + 163, Y + 46 + 15);
                            Explorer.CanvasMain.DrawStringTTF("Try again.", "UMR", Color.White, 17, X + 163, Y + 65 + 15);

                            if (Explorer.MY > Y + SizeY - 40 && Explorer.MY < Y + SizeY - 10)
                            {
                                if (Explorer.MX > X + 222 - 50 && Explorer.MX < X + 222 + 50)
                                {
                                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + 222 - 50, Y + SizeY - 40, 100, 30);
                                    if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left && !Clicked)
                                    {
                                        Apps.Process.Processes[i].tempInt = 0;
                                        Clicked = true;
                                    }
                                }
                                else
                                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 222 - 50, Y + SizeY - 40, 100, 30);
                            }
                            else
                                Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 222 - 50, Y + SizeY - 40, 100, 30);
                            StringsAcitons.DrawCenteredTTFString("OK", 100, X + 222 - 50, Y + SizeY - 20, 15, Color.White, "UMR", 17);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBoxCreator.CreateMessageBox("Fatal Error", "Please open RadianceOS console mode!\n" + e.Message, MessageBoxCreator.MessageBoxIcon.error, 600, 175);
            }
           
		}




	}
}
