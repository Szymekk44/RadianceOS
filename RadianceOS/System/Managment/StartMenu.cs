using Cosmos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cosmos.System;
using System.Linq.Expressions;
using RadianceOS.System.Apps.Games;
using RadianceOS.System.Apps.RadianceOSwebBrowser;
using Cosmos.HAL;
using RadianceOS.Render;
using RadianceOS.System.Graphic;
using static Cosmos.HAL.Drivers.Video.VGADriver;
using CosmosTTF;
using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System.Graphics;
using Canvas = RadianceOS.Render.Canvas;
using RadianceOS.System.Apps;

namespace RadianceOS.System.Managment
{
    public static class StartMenu
    {
        public static int state;
        public static int y;
        public static Bitmap bimapRadiance;
        public static Bitmap bimapName;
        public static void Render()
        {
            //		Explorer.CanvasMain.DrawImage(Kernel.StartMenu, 5, y);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, 5, y, 250, 700);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, 255, y, 250, 700);
            Window.DrawRoundedRectangle(5, y - 25, 500, 25, 10, Kernel.shadow, 20);

            //Window.DrawRoundedTopRightCorner(255, y, 200, 700, 15, Kernel.main);
            DrawStartButton(0);
            DrawStartButton(1);
            DrawStartButton(2);
            DrawStartButton(3);
            DrawStartButton(4);
            DrawStartButton(5);
            DrawStartButton(6);

            DrawStartSmallButton(0);
            Explorer.CanvasMain.DrawImage(Kernel.userIcon, 275, y + 5);
            if (bimapName != null)
            {
                Explorer.CanvasMain.DrawImage(bimapName, 330, y + 20);
                Explorer.CanvasMain.DrawImage(bimapRadiance, 10, y - 21);
            }
            else
            {

                if (y < 600)
                {
                    ReDraw();
                }
                else
                {
                    Explorer.CanvasMain.DrawStringTTF("RadianceOS", "UMB", Kernel.fontColor, 17, 10, y - 7, 18);
                    Explorer.CanvasMain.DrawStringTTF(Kernel.loggedUser, "UMR", Kernel.fontColor, 24, 330, y + 35);
                }


            }

            Explorer.CanvasMain.DrawLine(Kernel.lightMain, 326, y + 69, 429, y + 69);
            Explorer.CanvasMain.DrawLine(Kernel.lightMain, 325, y + 70, 430, y + 70);
            Explorer.CanvasMain.DrawLine(Kernel.lightMain, 326, y + 71, 429, y + 71);
        }
        public static void ReDraw()
        {
            Explorer.CanvasMain.DrawStringTTF(Kernel.loggedUser, "UMR", Kernel.fontColor, 24, 330, y + 35);
            Window.GetTempImage(330, y + 35 - 16, Kernel.loggedUser.GetTTFWidth("UMR", 30), 24, "PreRender user");
            bimapName = Window.tempBitmap;

            Explorer.CanvasMain.DrawStringTTF("RadianceOS", "UMB", Kernel.fontColor, 17, 10, y - 7, 18);
            Window.GetTempImage(10, y - 7 - 15, "RadianceOS".GetTTFWidth("UMB", 17), 20, "PreRender Radiance Text");
            bimapRadiance = Window.tempBitmap;
        }
        public static void DrawStartButton(int id)
        {

            bool selected = false;
            if (state == 0)
            {
                if (Explorer.MY >= y + id * 40 && Explorer.MY <= y + id * 40 + 40)
                {
                    if (Explorer.MX >= 5 && Explorer.MX <= 255)
                    {
                        selected = true;
                        if (MouseManager.MouseState == MouseState.Left)
                        {

                            state = 2;

                            switch (id)
                            {
                                case 0:
                                    {
                                        Processes MessageBox2 = new Processes
                                        {
                                            ID = 1,
                                            Name = "Terminal",
                                            Description = "CosmosVFS is working!",
                                            metaData = @"0:\",
                                            X = 100,
                                            Y = 100,
                                            SizeX = 800,
                                            SizeY = 500,
                                            sizeAble = true,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(MessageBox2);
                                        Process.UpdateProcess(Process.Processes.Count - 1);
                                    }
                                    break;

                                case 1:
                                    {
                                        Processes MessageBox2 = new Processes
                                        {
                                            ID = 2,
                                            Name = "Untitled",
                                            Description = "CosmosVFS is working!",
                                            metaData = @"0:\",
                                            X = 100,
                                            Y = 100,
                                            SizeX = 1000,
                                            SizeY = 700,
                                            saved = true,
                                            sizeAble = true,
                                            temp = @"0:\Users\" + Kernel.loggedUser + @"\Desktop\",
                                            moveAble = true
                                        };
                                        Process.Processes.Add(MessageBox2);
                                        Process.UpdateProcess(Process.Processes.Count - 1);
                                    }
                                    break;
                                case 2:
                                    {


                                        Processes MessageBox2 = new Processes
                                        {
                                            ID = 5,
                                            Name = "Settings",
                                            X = 100,
                                            Y = 100,
                                            SizeX = 1000,
                                            SizeY = 700,
                                            tempInt = 0,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(MessageBox2);
                                        Process.UpdateProcess(Process.Processes.Count - 1);

                                    }
                                    break;
                                case 3:
                                    {


                                        Processes MessageBox2 = new Processes
                                        {
                                            ID = 6,
                                            Name = "Snake",
                                            X = 100,
                                            Y = 70,
                                            SizeX = 900,
                                            SizeY = 950,
                                            tempInt = 0,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(MessageBox2);
                                        Snake.Start(Process.Processes.Count - 1);


                                    }
                                    break;
                                case 4:
                                    {


                                        Processes MessageBox2 = new Processes
                                        {
                                            ID = 7,
                                            Name = "Performance Info",
                                            X = 100,
                                            Y = 70,
                                            SizeX = 300,
                                            SizeY = 142,
                                            tempInt = 0,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(MessageBox2);
                                        Kernel.countFPS = true;

                                    }
                                    break;
                                case 5:
                                    {


                                        Processes MessageBox2 = new Processes
                                        {
                                            ID = 8,
                                            Name = "RadiantWave",
                                            X = 100,
                                            Y = 70,
                                            SizeX = 1200,
                                            SizeY = 700,
                                            tempInt = 0,
                                            texts = new string[2] { "szymekk.pl/RadianceOS/index.html", "" },
                                            CurrChar = "szymekk.pl/RadianceOS/index.html".Length,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(MessageBox2);
                                        RadiantWave.LoadWebsite(Process.Processes.Count - 1);

                                    }
                                    break;
                                case 6:
                                    {


                                        Processes SecurityManager = new Processes
                                        {
                                            ID = 13,
                                            Name = "Security Manager",
                                            Description = "The RadianceOS Security Manager",
                                            X = 100,
                                            Y = 70,
                                            SizeX = 1200,
                                            SizeY = 700,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(SecurityManager);
                                        Process.UpdateProcess(Process.Processes.Count - 1);

                                    }
                                    break;



                            }


                        }
                    }
                }
            }

            if (selected)
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, 5, y + id * 40, 250, 40);
            switch (id)
            {
                case 0:
                    Canvas.DrawImageAlpha(Kernel.cmd, 10, y + 4 + id * 32);
                    Explorer.CanvasMain.DrawString("Terminal", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 32);
                    //TTFManager.DrawStringTTF(Explorer.CanvasMain, "Terminal", "UMR", Kernel.fontColor, 15, 47, y + 27 + (id * 47));
                    break;
                case 1:
                    Canvas.DrawImageAlpha(Kernel.notepad, 10, y + 4 + id * 40);
                    Explorer.CanvasMain.DrawString("Notepad", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 40);
                    //TTFManager.DrawStringTTF(Explorer.CanvasMain, "Notepad", "UMR", Kernel.fontColor, 15, 47, y + 12 + (id * 47));
                    break;
                case 2:
                    Canvas.DrawImageAlpha(Kernel.settingIcon, 10, y + 4 + id * 40);
                    Explorer.CanvasMain.DrawString("Settings", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 40);
                    //TTFManager.DrawStringTTF(Explorer.CanvasMain, "Settings", "UMR", Kernel.fontColor, 15, 47, y + 12 + (id * 47));
                    break;
                case 3:
                    Canvas.DrawImageAlpha(Kernel.gamepadIcon, 10, y + 4 + id * 40);
                    Explorer.CanvasMain.DrawString("Snake", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 40);
                    //TTFManager.DrawStringTTF(Explorer.CanvasMain, "Snake", "UMR", Kernel.fontColor, 15, 47, y + 12 + (id * 47));
                    break;
                case 4:
                    Canvas.DrawImageAlpha(Kernel.sysinfoIcon, 10, y + 4 + id * 40);
                    Explorer.CanvasMain.DrawString("Performance", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 40);
                    //TTFManager.DrawStringTTF(Explorer.CanvasMain, "Performance", "UMR", Kernel.fontColor, 15, 47, y + 12 + (id * 47));
                    break;
                case 5:
                    Canvas.DrawImageAlpha(Kernel.RadiantWave, 10, y + 4 + id * 40);
                    Explorer.CanvasMain.DrawString("RadiantWave Web Browser", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 40);
                    //	TTFManager.DrawStringTTF(Explorer.CanvasMain, "RadiantWave Web Browser", "UMR", Kernel.fontColor, 15, 47, y + 12 + (id * 47));
                    break;
                case 6:
                    Canvas.DrawImageAlpha(Kernel.UACIcon, 10, y + 4 + id * 40);
                    Explorer.CanvasMain.DrawString("Security Manager", Kernel.font18, Kernel.fontColor, 47, y + 12 + id * 40);
                    break;
            }
        }


        public static void DrawStartSmallButton(int id)
        {

            bool selected = false;
            if (state == 0)
            {
                if (Explorer.MY >= y + id * 20 + 80 && Explorer.MY <= y + id * 20 + 100)
                {
                    if (Explorer.MX >= 270 && Explorer.MX <= 490)
                    {
                        selected = true;
                        if (MouseManager.MouseState == MouseState.Left)
                        {

                            state = 2;

                            switch (id)
                            {
                                case 0:
                                    {
                                        Processes FE = new Processes
                                        {
                                            ID = 10,
                                            Name = "File Explorer",
                                            X = 300,
                                            Y = 200,
                                            SizeX = 900,
                                            MinX = 500,
                                            SizeY = 550,
                                            sizeAble = true,
                                            moveAble = true
                                        };
                                        Process.Processes.Add(FE);
                                        FileExplorer.UpdateList(Process.Processes.Count - 1, @"0:\");
                                    }
                                    break;
                            }


                        }
                    }
                }
            }

            if (selected)
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, 265, y + 82 + id * 16, 225, 20);
            switch (id)
            {
                case 0:
                    Canvas.DrawImageAlpha(Kernel.SmallFE, 270, y + 84 + id * 16);
                    Explorer.CanvasMain.DrawString("File Explorer", Kernel.font18, Kernel.fontColor, 290, y + 84 + id * 16);
                    //TTFManager.DrawStringTTF(Explorer.CanvasMain, "Terminal", "UMR", Kernel.fontColor, 15, 47, y + 27 + (id * 47));
                    break;
            }
        }

        public static void Move()
        {
            switch (state)
            {
                case 1:
                    if (y > Explorer.screenSizeY - 600)
                    {
                        y -= 20;
                    }
                    else
                        state = 0;
                    break;
                case 2:
                    {
                        if (y < Explorer.screenSizeY - 40)
                        {
                            y += 20;
                        }
                        else
                        {
                            state = 0;
                            Explorer.DrawMenu = false;
                        }

                    }
                    break;
            }

        }
    }
}
