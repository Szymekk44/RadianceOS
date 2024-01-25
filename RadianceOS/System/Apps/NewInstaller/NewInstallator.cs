using Cosmos.System;
using Cosmos.System.Graphics;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace RadianceOS.System.Apps.NewInstaller
{
    public static class NewInstallator
    {
        public static void Render(int ProcessID, int state, int X, int Y, int SizeX, int SizeY, bool newRender)
        {
            newRender = Check(state, ProcessID);

            if (newRender) //RENDER WINDOW AS IMAGE
            {
                switch (state)
                {
                    case 0:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer", false, false, true, false);


                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImageAlpha(new Bitmap(RadianceOS.System.Managment.Files.RadianceOSIconTransparent), X + (SizeX - 456) / 2, Y + 30);
                            StringsAcitons.DrawCenteredTTFString("Welcome to RadianceOS installer", SizeX, X, Y + 110, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("To start the installation process, please click Install", SizeX, X, Y + 130, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                            InputSystem.CurrentString = "";
                        }
                        break;
                    case 1:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - About RadianceOS", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImageAlpha(new Bitmap(RadianceOS.System.Managment.Files.RadianceOSIconTransparent), X + (SizeX - 456) / 2, Y + 30);
                            StringsAcitons.DrawCenteredTTFString("About", SizeX, X, Y + 110, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("RadianceOS " + Kernel.version + " - " + Kernel.subversion + "\nRa# version: " + Kernel.RasVersion + "\nRam: " + Cosmos.Core.GCImplementation.GetAvailableRAM() + "MB (Using " + (Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576) + "MB)" + "\n\nRadianceOS is an open source operating system created by Szymekk using Cosmos\nC# Open Source Managed Operating System\n\nSzymekk.pl\nYouTube.com/Szymekk\ngocosmos.org", SizeX, X, Y + 130, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer", 1);
                        }
                        break;
                    case 2:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Test RadianceOS", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImageAlpha(new Bitmap(RadianceOS.System.Managment.Files.RadianceOSIconTransparent), X + (SizeX - 456) / 2, Y + 30);
                            StringsAcitons.DrawCenteredTTFString("Test RadianceOS", SizeX, X, Y + 110, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Warning!\nSystem will not have access to the disk!\nSome apps may not work!\n\nYou can always install RadianceOS later.", SizeX, X, Y + 135, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer", 2);
                        }
                        break;
                    case 3:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Loading...", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImageAlpha(new Bitmap(RadianceOS.System.Managment.Files.RadianceOSIconTransparent), X + (SizeX - 456) / 2, Y + 30);
                            StringsAcitons.DrawCenteredTTFString("Please Wait", SizeX, X, Y + 110, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Loading RadianceOS", SizeX, X, Y + 130, 14, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 4:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX / 4, 25);

                            StringsAcitons.DrawCenteredTTFString("Disk Formatting", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Before installing RadianceOS you have to format your drive.\nWARNING!\nDon't do this on a real pc! This may damage your entire drive!\n\nFS type: FAT32\nDisk size: " + Kernel.fs.Disks[0].Size / (1024 * 1024) + " MB", SizeX, X, Y + 100, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 5:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX / 4, 25);

                            StringsAcitons.DrawCenteredTTFString("Disk Formatting", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Before installing RadianceOS you have to format your drive.\nWARNING!\nDon't do this on a real pc! This may damage your entire drive!\n\nFS type: FAT32\n\nPlease enter new disk size\nMinimum 32 MB!", SizeX, X, Y + 100, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer", 1);
                        }
                        break;
                    case 6:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX / 4, 25);
                            int diskSize = 0;
                            if (InputSystem.CurrentString != "")
                                diskSize = int.Parse(InputSystem.CurrentString);
                            else
                                diskSize = Kernel.fs.Disks[0].Size / (1024 * 1024);
                            StringsAcitons.DrawCenteredTTFString("Formatting Your Disk", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Please wait\n\nFS type: Fat32\nDisk size: " + diskSize + " MB", SizeX, X, Y + 100, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 7:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting Done!", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX / 2, 25);

                            StringsAcitons.DrawCenteredTTFString("Disk Formatting Done!", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Restart RadianceOS to continue.", SizeX, X, Y + 100, 18, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 8:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - User Account", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX / 2, 25);

                            StringsAcitons.DrawCenteredTTFString("Create your RadianceOS user account", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Please enter main account name", SizeX, X, Y + 100, 14, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 9:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - User Account", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX / 2, 25);

                            StringsAcitons.DrawCenteredTTFString("Create your RadianceOS user account", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Please enter password for " + Process.Processes[ProcessID].texts[0], SizeX, X, Y + 100, 14, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 10:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "Installing RadianceOS...", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX - SizeX / 4, 25);

                            StringsAcitons.DrawCenteredTTFString("Installing RadianceOS", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Please wait for the installation to complete.", SizeX, X, Y + 100, 14, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                    case 11:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Done!", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y + 25, SizeX, 25);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, SizeX, 25);

                            StringsAcitons.DrawCenteredTTFString("RadianceOS is ready to use!", SizeX, X, Y + 70, 20, Kernel.fontColor, "UMB", 24);
                            StringsAcitons.DrawCenteredTTFString("Restart your PC to continue.", SizeX, X, Y + 100, 14, Kernel.fontColor, "UMR", 18);
                            Window.GetImage(X, Y, SizeX, SizeY, ProcessID, "Installer");
                        }
                        break;
                }
                newRender = false;
                Process.Processes[ProcessID].tempBool = false;
            }
            else //RENDER WINDOW EVERY FRAME (WITH CREATED IMAGE)
            {
                switch (state)
                {
                    case 0:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            DrawButton(0, "Install", 0, X, Y, ProcessID);
                            DrawButton(1, "Test RadianceOS", 1, X, Y, ProcessID);
                            DrawButton(2, "System Information", 2, X, Y, ProcessID);
                        }
                        break;
                    case 1:
                        {
                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - About RadianceOS", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap2, X, Y + 25);
                            DrawButton(0, "Return", 3, X, Y, ProcessID);
                        }
                        break;
                    case 2:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - About RadianceOS", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap3, X, Y + 25);
                            DrawButton(1, "Return", 3, X, Y, ProcessID);
                            DrawButton(0, "Continue", 4, X, Y, ProcessID);

                        }
                        break;
                    case 3:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - About RadianceOS", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            Explorer.CanvasMain.Display();
                            Kernel.diskReady = false;
                            Kernel.DiskError = new Bitmap(Files.disk);
                            Processes start = new Processes
                            {
                                ID = -1,
                            };
                            Process.Processes.Add(start);
                            Processes MessageBox = new Processes
                            {
                                ID = 0,
                                Name = "Drive error",
                                Description = "The drive has not been configured!\nRadianceOS cannot use VFS\n\nMost applications may not work.",
                                metaData = "diskError",
                                X = 100,
                                Y = 100,
                                SizeX = 400,
                                SizeY = 200,
                                moveAble = true
                            };
                            Process.Processes.Add(MessageBox);
                            Process.UpdateProcess(Process.Processes.Count - 1);
                            Explorer.DrawTaskbar = true;
                            Explorer.drawIcons = true;
                            Explorer.UpdateIcons();
                            Process.Processes.RemoveAt(ProcessID);
                            return;
                        }
                        break;
                    case 4:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            DrawButton(2, "Return", 3, X, Y, ProcessID);
                            DrawButton(1, "Set Custom Disk Size", 5, X, Y, ProcessID);
                            DrawButton(0, "Continue", 6, X, Y, ProcessID);

                        }
                        break;
                    case 5:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap2, X, Y + 25);
                            InputSystem.onlyNumbers = true;
                            InputSystem.AllowArrows = true;
                            InputSystem.AllowUpDown = false;
                            Process.Processes[ProcessID].CurrLine = 0;
                            InputSystem.Monitore(6, Process.Processes[ProcessID].CurrChar, ProcessID);
                            StringsAcitons.DrawCenteredTTFString(InputSystem.CurrentString.Substring(0, Process.Processes[ProcessID].CurrChar) + "|" + InputSystem.CurrentString.Substring(Process.Processes[ProcessID].CurrChar) + " MB", SizeX, X, Y + 220, 20, Kernel.fontColor, "UMR", 14);
                            DrawButton(1, "Return", 3, X, Y, ProcessID);
                            if (int.Parse(InputSystem.CurrentString) < 32)
                            {
                                DrawButton(0, "Enter valid disk size to continue", 9999, X, Y, ProcessID);
                            }
                            else
                            {
                                DrawButton(0, "Continue", 6, X, Y, ProcessID);
                            }

                        }
                        break;
                    case 6:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            Explorer.CanvasMain.Display();
                            try
                            {
                                if (Kernel.fs.Disks[0].Partitions.Count > 0) //FORMAT
                                    Kernel.fs.Disks[0].DeletePartition(0);
                                Kernel.fs.Disks[0].Clear();
                                int diskSize = 0;
                                if (InputSystem.CurrentString == "")
								{
									diskSize = Kernel.fs.Disks[0].Size / 1048576;
									//MessageBoxCreator.CreateMessageBox("Info", "Default size: " + Kernel.fs.Disks[0].Size / 1048576 + " MB", MessageBoxCreator.MessageBoxIcon.info, 400, 175);

								}
								else
                                {
                                    diskSize = int.Parse(InputSystem.CurrentString);
                                }

                                if (Kernel.fs.Disks[0].Size > 1)
                                {
                                    Kernel.fs.Disks[0].CreatePartition(diskSize);
                                }
                                else
                                    Kernel.fs.Disks[0].CreatePartition(diskSize);

                                Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
                            }
                            catch (Exception ex)
                            {
                                string newMessage = "Error: " + ex.Message;
                                if (ex.Message == "size")
                                {
                                    newMessage = "Invalid partition size! (code: 0)\n";
                                }

                                MessageBoxCreator.CreateMessageBox("Fatal", "Disk formatting failed!\n" + newMessage, MessageBoxCreator.MessageBoxIcon.error, 540, 175);
                                Process.Processes[ProcessID].bitmap = null;
                                Process.Processes[ProcessID].bitmap2 = null;
                                Process.Processes[ProcessID].bitmap3 = null;
                                Process.Processes[ProcessID].tempInt = 0;
                                return;
                            }
                            finally
                            {
                                Process.Processes[ProcessID].bitmap = null;
                                Process.Processes[ProcessID].tempInt = 7;
                            }
                        }
                        break;
                    case 7:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Disk Formatting", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            DrawButton(0, "Reboot", 7, X, Y, ProcessID);

                        }
                        break;
                    case 8:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - User Account", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            InputSystem.onlyNumbers = false;
                            InputSystem.SpecialCharracters = false;
                            InputSystem.AllowArrows = true;
                            InputSystem.AllowUpDown = false;
                            Process.Processes[ProcessID].selected = true;
                            Process.Processes[ProcessID].CurrLine = 0;
                            InputSystem.Monitore(6, Process.Processes[ProcessID].CurrChar, ProcessID, 16);
                            StringsAcitons.DrawCenteredTTFString(InputSystem.CurrentString.Substring(0, Process.Processes[ProcessID].CurrChar) + "|" + InputSystem.CurrentString.Substring(Process.Processes[ProcessID].CurrChar), SizeX, X, Y + 120, 20, Kernel.fontColor, "UMR", 18);
                            if (InputSystem.CurrentString.Length < 3)
                            {
                                DrawButton(0, "Enter account name to continue", 9999, X, Y, ProcessID);
                            }
                            else
                            {
                                DrawButton(0, "Continue", 8, X, Y, ProcessID);
                            }

                        }
                        break;
                    case 9:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - User Account (" + Process.Processes[ProcessID].texts[0] + ")", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            InputSystem.SpecialCharracters = true;
                            InputSystem.AllowArrows = true;
                            InputSystem.AllowUpDown = false;
                            Process.Processes[ProcessID].selected = true;
                            Process.Processes[ProcessID].CurrLine = 0;
                            InputSystem.Monitore(6, Process.Processes[ProcessID].CurrChar, ProcessID, 24);
                            StringsAcitons.DrawCenteredTTFString(InputSystem.CurrentString.Substring(0, Process.Processes[ProcessID].CurrChar) + "|" + InputSystem.CurrentString.Substring(Process.Processes[ProcessID].CurrChar), SizeX, X, Y + 120, 20, Kernel.fontColor, "UMR", 18);
                            DrawButton(1, "Change Account Name", 9, X, Y, ProcessID);
                            if (InputSystem.CurrentString.Length < 3)
                            {
                                DrawButton(0, "Enter password to continue", 9999, X, Y, ProcessID);
                            }
                            else
                            {
                                DrawButton(0, "Continue", 10, X, Y, ProcessID);
                            }

                        }
                        break;

                    case 10:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "Installing RadianceOS...", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            Explorer.CanvasMain.Display();

                            try
                            {
                                InstallFiles(ProcessID);
                            }
                            catch (Exception ex)
                            {
                                Explorer.CanvasMain.Disable();

                                Kernel.WriteLineERROR(ex.ToString());
                            }
                            Process.Processes[ProcessID].bitmapTop = null;
                            Process.Processes[ProcessID].bitmap = null;
                            Process.Processes[ProcessID].tempInt = 11;
                        }
                        break;
                    case 11:
                        {

                            Window.DrawTop(ProcessID, X, Y, SizeX, "RadianceOS " + Kernel.subversion + " Installer - Done!", false, false, true, false);
                            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
                            Explorer.CanvasMain.DrawImage(Process.Processes[ProcessID].bitmap, X, Y + 25);
                            DrawButton(0, "Reboot", 7, X, Y, ProcessID);

                        }
                        break;
                }


            }



        }


        public static void InstallFiles(int ProcessID)
        {
            Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
            DrawStatus("Please Wait", 0);
            Thread.Sleep(2500);
            DrawStatus("Creating System Directories", 1);
            if(!Directory.Exists(@"0:\RadianceOS"))
            {
				Directory.CreateDirectory(@"0:\RadianceOS");
				Directory.CreateDirectory(@"0:\RadianceOS\System");
				Directory.CreateDirectory(@"0:\RadianceOS\System\Files");
				Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Wallpapers");
				Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Icons");
				Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Audio");
				Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Apps");
				Directory.CreateDirectory(@"0:\RadianceOS\Settings");
			
			}
			Directory.CreateDirectory(@"0:\Users");

			DrawStatus("Creating User", 2);
            string AccountName = Process.Processes[ProcessID].texts[0];
            Directory.CreateDirectory(@"0:\Users\" + AccountName);
            string myUser = @"0:\Users\" + AccountName + @"\";
            Directory.CreateDirectory(myUser + "AccountInfo");
            File.Create(myUser + @"AccountInfo\Password.SysData");
            File.WriteAllText(myUser + @"AccountInfo\Password.SysData", Process.Processes[ProcessID].texts[1]);//PASSWORD

            DrawStatus("Creating Directories", 3);
            Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Documents");
            Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Settings");
            Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Saved");
            Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Desktop");
			Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Downloads");

			DrawStatus("Creating Data Files", 4);
            File.Create(@"0:\Users\" + AccountName + @"\Desktop\desktop.SysData");
            File.Create(@"0:\Users\" + AccountName + @"\Settings\Theme.dat");
            File.WriteAllText(@"0:\Users\" + AccountName + @"\Settings\Theme.dat", "0");
            File.Create(@"0:\Users\" + AccountName + @"\Settings\Wallpaper.dat");
            File.WriteAllText(@"0:\Users\" + AccountName + @"\Settings\Wallpaper.dat", "0");

            DrawStatus("Done!", 5);
        }


        public static void DrawStatus(string text, int progress)
        {
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, (int)(Explorer.screenSizeX - 400) / 2 + 3, (int)(Explorer.screenSizeY - 150) / 2 + 3, 400, 150);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, (int)(Explorer.screenSizeX - 400) / 2, (int)(Explorer.screenSizeY - 150) / 2, 400, 150);
            StringsAcitons.DrawCenteredTTFString(text, 400, (int)(Explorer.screenSizeX - 400) / 2, (int)(Explorer.screenSizeY - 150) / 2 + 20, 20, Kernel.fontColor, "UMR", 18);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, (int)(Explorer.screenSizeX - 400) / 2 + 25, (int)(Explorer.screenSizeY - 150) / 2 + 100, 350, 25);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, (int)(Explorer.screenSizeX - 400) / 2 + 25, (int)(Explorer.screenSizeY - 150) / 2 + 100, 70 * progress, 25);
            Explorer.CanvasMain.Display();
        }

        public static void DrawButton(int Number, string Text, int Action, int X, int Y, int ProcessID)
        {

            if (Explorer.MX > X + 250 && Explorer.MX < X + 550)
            {
                if (Explorer.MY > Y + 450 - (Number * 45) && Explorer.MY < Y + 480 - (Number * 45))
                {
                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + 250, Y + 450 - (Number * 45), 300, 40);
                    StringsAcitons.DrawCenteredString(Text, 300, X + 250, Y + 461 - (Number * 45), 20, Color.FromArgb(199, 191, 255), Kernel.font18);
                    if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
                    {
                        switch (Action)
                        {
                            case 0:
                                {
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    Process.Processes[ProcessID].tempInt = 4;
                                    Process.Processes[ProcessID].bitmap = null;
                                }
                                break;
                            case 1:
                                {
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    Process.Processes[ProcessID].tempInt = 2;
                                }
                                break;
                            case 2:
                                {
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    Process.Processes[ProcessID].tempInt = 1;
                                }
                                break;
                            case 3:
                                {
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    if (Process.Processes[ProcessID].tempInt == 4 || Process.Processes[ProcessID].tempInt == 5)
                                        Process.Processes[ProcessID].bitmap = null;
                                    Process.Processes[ProcessID].tempInt = 0;

                                }
                                break;
                            case 4:
                                {
                                    Process.Processes[ProcessID].tempInt = 3;
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    Process.Processes[ProcessID].bitmap = null;
                                    Process.Processes[ProcessID].bitmap2 = null;
                                    Process.Processes[ProcessID].bitmap3 = null;
                                }
                                break;
                            case 5:
                                {
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    Process.Processes[ProcessID].tempInt = 5;
                                    InputSystem.CurrentString = "";
                                    Process.Processes[ProcessID].selected = true;
                                    Process.Processes[ProcessID].CurrChar = 0;
                                }
                                break;
                            case 6: //Default
                                {
                                    Process.Processes[ProcessID].tempInt = 6;
                                    Process.Processes[ProcessID].bitmapTop = null;
                                    Process.Processes[ProcessID].bitmap = null;

                                }
                                break;
                            case 7:
                                {
                                    Cosmos.System.Power.Reboot();

                                }
                                break;
                            case 8:
                                {
                                    Process.Processes[ProcessID].texts = new string[2];
                                    Process.Processes[ProcessID].texts[0] = InputSystem.CurrentString;
                                    InputSystem.CurrentString = "";
                                    Process.Processes[ProcessID].CurrChar = 0;
                                    Process.Processes[ProcessID].tempInt = 9;
                                    Process.Processes[ProcessID].bitmap = null;
                                }
                                break;
                            case 9:
                                {
                                    InputSystem.CurrentString = Process.Processes[ProcessID].texts[0];
                                    Process.Processes[ProcessID].CurrChar = Process.Processes[ProcessID].texts[0].Length;
                                    Process.Processes[ProcessID].tempInt = 8;
                                    Process.Processes[ProcessID].bitmap = null;
                                }
                                break;
                            case 10:
                                {
                                    Process.Processes[ProcessID].tempInt = 10;
                                    Process.Processes[ProcessID].texts[1] = InputSystem.CurrentString;
                                    Process.Processes[ProcessID].bitmap = null;
                                }
                                break;

                        }
                    }
                }
                else
                {
                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 250, Y + 450 - (Number * 45), 300, 40);
                    StringsAcitons.DrawCenteredString(Text, 300, X + 250, Y + 461 - (Number * 45), 20, Color.White, Kernel.font18);
                }
            }
            else
            {
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 250, Y + 450 - (Number * 45), 300, 40);
                StringsAcitons.DrawCenteredString(Text, 300, X + 250, Y + 461 - (Number * 45), 20, Color.White, Kernel.font18);
            }


        }

        public static bool Check(int state, int ProcessID)
        {
            if (Process.Processes[ProcessID].bitmap == null)
                return true;
            switch (state) //RENDER CHECK
            {
                case 1:
                    {
                        if (Process.Processes[ProcessID].bitmap2 == null)
                            return true;
                    }
                    break;
                case 2:
                    {
                        if (Process.Processes[ProcessID].bitmap3 == null)
                            return true;
                    }
                    break;
                case 5:
                    {
                        if (Process.Processes[ProcessID].bitmap2 == null)
                            return true;
                    }
                    break;
            }

            return false;

        }
    }
}
