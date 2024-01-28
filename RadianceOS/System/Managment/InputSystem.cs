
using RadianceOS.System.Apps;
using RadianceOS.System.Apps.RadianceOSwebBrowser;
using RadianceOS.System.Programming.RaSharp;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RadianceOS.System.Managment
{
	public static class InputSystem
	{
		public static string CurrentString;
		public static bool SpecialCharracters = true, allowDots, AllowArrows = false, AllowUpDown, onlyNumbers;
		public static bool Shift;
		public static void Monitore(int action, int CurChar, int Index, int maxLenght = 1000)
		{
			bool enterChar = true;
			char charToEnter = ' ';
			bool isSpaceBar = false;
			while (Console.KeyAvailable)
			{
				Apps.Process.Processes[Index].saved = false;
				ConsoleKeyInfo key = Console.ReadKey(true);

                if(key.Modifiers == ConsoleModifiers.Shift) {
					Shift = true;
				} else
				{
					Shift = false;
				}

                switch (key.Key)
				{
				
					#region Main keys

					case ConsoleKey.A:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'a';
							break;
						}
						else
						{
							charToEnter = 'A';
							break;
						}
					case ConsoleKey.B:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'b';
							break;
						}
						else
						{
							charToEnter = 'B';
							break;
						}
					case ConsoleKey.C:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'c';
							break;
						}
						else
						{
							charToEnter = 'C';
							break;
						}
					case ConsoleKey.D:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift && key.Modifiers != ConsoleModifiers.Control)
						{
							charToEnter = 'd';
							break;
						}
						else if(key.Modifiers != ConsoleModifiers.Control)
						{
							charToEnter = 'D';
							break;
						}
						else
						{
							if(!Kernel.countFPS)
							{
								Processes MessageBox2 = new Processes
								{
									ID = 7,
									Name = "System Info",
									X = 100,
									Y = 70,
									SizeX = 300,
									SizeY = 142,
									tempInt = 0,
									moveAble = true
								};
								Apps.Process.Processes.Add(MessageBox2);
								Kernel.countFPS = true;
							}
					
							break;
						}
					case ConsoleKey.E:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'e';
							break;
						}
						else
						{
							charToEnter = 'E';
							break;
						}
					case ConsoleKey.F:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'f';
							break;
						}
						else
						{
							charToEnter = 'F';
							break;
						}
					case ConsoleKey.G:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'g';
							break;
						}
						else
						{
							charToEnter = 'G';
							break;
						}
					case ConsoleKey.H:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'h';
							break;
						}
						else
						{
							charToEnter = 'H';
							break;
						}
					case ConsoleKey.I:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'i';
							break;
						}
						else
						{
							charToEnter = 'I';
							break;
						}
					case ConsoleKey.J:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'j';
							break;
						}
						else
						{
							charToEnter = 'J';
							break;
						}
					case ConsoleKey.K:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'k';
							break;
						}
						else
						{
							charToEnter = 'K';
							break;
						}
					case ConsoleKey.L:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'l';
							break;
						}
						else
						{
							charToEnter = 'L';
							break;
						}
					case ConsoleKey.M:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'm';
							break;
						}
						else
						{
							charToEnter = 'M';
							break;
						}
					case ConsoleKey.N:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'n';
							break;
						}
						else
						{
							charToEnter = 'N';
							break;
						}
					case ConsoleKey.O:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'o';
							break;
						}
						else
						{
							charToEnter = 'O';
							break;
						}
					case ConsoleKey.P:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'p';
							break;
						}
						else
						{
							charToEnter = 'P';
							break;
						}
					case ConsoleKey.Q:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'q';
							break;
						}
						else
						{
							charToEnter = 'Q';
							break;
						}
					case ConsoleKey.R:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'r';
							break;
						}
						else
						{
							charToEnter = 'R';
							break;
						}
					case ConsoleKey.S:

						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift && key.Modifiers != ConsoleModifiers.Control)
						{
							charToEnter = 's';

						}
						else if (key.Modifiers != ConsoleModifiers.Control)
						{
							charToEnter = 'S';

						}
						else
						{
							enterChar = false;
							switch (action)
							{
								case 2:
									{

										Notepad.save();
										CurChar = 0;
									

										return;
									}
									break;
							}
						}

						break;
					case ConsoleKey.T:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 't';
							break;
						}
						else
						{
							charToEnter = 'T';
							break;
						}
					case ConsoleKey.U:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'u';
							break;
						}
						else
						{
							charToEnter = 'U';
							break;
						}
					case ConsoleKey.V:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'v';
							break;
						}
						else
						{
							charToEnter = 'V';
							break;
						}
					case ConsoleKey.W:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'w';
							break;
						}
						else
						{
							charToEnter = 'W';
							break;
						}
					case ConsoleKey.X:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'x';
							break;
						}
						else
						{
							charToEnter = 'X';
							break;
						}
					case ConsoleKey.Y:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'y';
							break;
						}
						else
						{
							charToEnter = 'Y';
							break;
						}
					case ConsoleKey.Z:
						if (!Console.CapsLock && key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = 'z';
							break;
						}
						else
						{
							charToEnter = 'Z';
							break;
						}

					case ConsoleKey.D0:

						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '0';
						}
						else if (SpecialCharracters)
						{
							charToEnter = ')';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D1:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '1';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '!';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D2:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '2';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '@';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D3:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '3';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '#';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D4:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '4';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '$';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D5:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '5';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '%';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D6:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '6';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '^';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D7:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '7';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '&';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D8:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '8';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '*';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.D9:
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '9';
						}
						else if (SpecialCharracters)
						{
							charToEnter = '(';
						}
						else
							enterChar = false;
						break;
					case ConsoleKey.Spacebar:
						charToEnter = ' ';
						isSpaceBar = true;
						break;
					case ConsoleKey.OemPeriod:

						if (allowDots || SpecialCharracters)
						{
							if (key.Modifiers != ConsoleModifiers.Shift)
							{
								if (allowDots || SpecialCharracters)
									charToEnter = '.';
								else
								{
									enterChar = false;
									break;
								}
							}
							else if (SpecialCharracters)
							{
								charToEnter = '>';
							}
							else
							{
								enterChar = false;
								break;
							}
						}




						break;
					case ConsoleKey.OemComma:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}

						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = ',';
						}
						else
						{
							charToEnter = '<';
						}
						break;
					case ConsoleKey.Oem1:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = ';';
						}
						else
						{
							charToEnter = ':';
						}
						break;
					case ConsoleKey.Oem2:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '/';
						}
						else
						{
							charToEnter = '?';
						}
						break;
					case ConsoleKey.Oem3:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '`';
						}
						else
						{
							charToEnter = '~';
						}
						break;
					case ConsoleKey.Oem4:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '[';
						}
						else
						{
							charToEnter = '{';
						}
						break;
					case ConsoleKey.Oem5:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '\\';
						}
						else
						{
							charToEnter = '|';
						}
						break;
					case ConsoleKey.Oem6:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = ']';
						}
						else
						{
							charToEnter = '}';
						}
						break;
					case ConsoleKey.Oem7:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '\'';
						}
						else
						{
							charToEnter = '"';
						}
						break;
					case ConsoleKey.OemPlus:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '=';
						}
						else
						{
							charToEnter = '+';
						}
						break;
					case ConsoleKey.OemMinus:
						if (!SpecialCharracters)
						{
							enterChar = false;
							break;
						}
						if (key.Modifiers != ConsoleModifiers.Shift)
						{
							charToEnter = '-';
						}
						else
						{
							charToEnter = '_';
						}
						break;
					#endregion
					case ConsoleKey.Backspace:
						enterChar = false;
						if (CurrentString.Length > 0 && CurChar > 0)
						{
							if (AllowArrows)
							{
								CurrentString = CurrentString.Remove(CurChar - 1, 1);
								for (int i = 0; i < Apps.Process.Processes.Count; i++)
								{
									if (Apps.Process.Processes[i].selected)
									{
										Apps.Process.Processes[i].CurrChar--;


									}
								}
							}
							else
								CurrentString = CurrentString.Remove(CurrentString.Length - 1, 1);

						}
						else if (AllowUpDown)
						{
							for (int i = 0; i < Apps.Process.Processes.Count; i++)
							{
								if (Apps.Process.Processes[i].selected)
								{
									if (Apps.Process.Processes[i].CurrLine > 0)
									{
										if (Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length == 0)
										{
											Apps.Process.Processes[i].defaultLines.RemoveAt(Apps.Process.Processes[i].CurrLine);
											Apps.Process.Processes[i].CurrLine--;
											CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine];

											Apps.Process.Processes[i].CurrChar = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length;
										}
										else
										{
											CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine - 1] + Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine];
											Apps.Process.Processes[i].defaultLines.RemoveAt(Apps.Process.Processes[i].CurrLine);
											Apps.Process.Processes[i].CurrLine--;

											Apps.Process.Processes[i].CurrChar = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length;
										}

									}



								}
							}
						}
						else if (CurrentString.Length > 0)
							CurrentString = CurrentString.Remove(CurrentString.Length - 1, 1);

						enterChar = false;
						break;
					case ConsoleKey.RightArrow:
						enterChar = false;
						if (AllowArrows)
						{
							for (int i = 0; i < Apps.Process.Processes.Count; i++)
							{
								if (Apps.Process.Processes[i].selected)
								{
									if (AllowUpDown)
									{
										if (Apps.Process.Processes[i].lines.Count != 0)
										{
											if (Apps.Process.Processes[i].lines[Apps.Process.Processes[i].CurrLine].text.Length > Apps.Process.Processes[i].CurrChar)
												Apps.Process.Processes[i].CurrChar++;
										}
										else if (Apps.Process.Processes[i].defaultLines.Count != 0)
										{
											if (Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length > Apps.Process.Processes[i].CurrChar)
											{
												Apps.Process.Processes[i].CurrChar++;
											}
										}
									}
									else
									{
										if (Apps.Process.Processes[i].lines.Count != 0)
										{
											if (Apps.Process.Processes[i].lines[Apps.Process.Processes[i].lines.Count - 1].text.Length > Apps.Process.Processes[i].CurrChar)
												Apps.Process.Processes[i].CurrChar++;
										}
										else if (Apps.Process.Processes[i].defaultLines.Count != 0)
										{
											if (Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].defaultLines.Count - 1].Length > Apps.Process.Processes[i].CurrChar)
											{
												Apps.Process.Processes[i].CurrChar++;
											}
										}
									}


								}
							}

						}
	
						break;
					case ConsoleKey.LeftArrow:
						enterChar = false;
						if (AllowArrows)
						{
							for (int i = 0; i < Apps.Process.Processes.Count; i++)
							{
								if (Apps.Process.Processes[i].selected)
								{
									if (Apps.Process.Processes[i].lines.Count != 0)
									{
										if (Apps.Process.Processes[i].CurrChar > 0)
											Apps.Process.Processes[i].CurrChar--;
									}
									else if (Apps.Process.Processes[i].defaultLines.Count != 0)
									{
										if (Apps.Process.Processes[i].CurrChar > 0)
										{
											Apps.Process.Processes[i].CurrChar--;
										}
									}

								}
							}

						}

						break;
					case ConsoleKey.UpArrow:
						enterChar = false;
						if (AllowUpDown)
						{
							int i = Index;
							if (Apps.Process.Processes[i].lines.Count != 0)
							{
								if (Apps.Process.Processes[i].CurrLine > 0)
								{
									Apps.Process.Processes[i].CurrLine--;
									if (Apps.Process.Processes[i].lines[Apps.Process.Processes[i].CurrLine].text.Length - 1 > Apps.Process.Processes[i].CurrChar)
									{
										Apps.Process.Processes[i].CurrChar = Apps.Process.Processes[i].lines[Apps.Process.Processes[i].CurrLine].text.Length;
									}
								}

							}
							else if (Apps.Process.Processes[i].defaultLines.Count != 0)
							{
								if (Apps.Process.Processes[i].CurrLine > 0)
								{

									Apps.Process.Processes[i].CurrLine--;
									if(Apps.Process.Processes[i].CurrLine < Apps.Process.Processes[i].StartLine)
									{
										Apps.Process.Processes[i].StartLine--;
									}

									if (Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length - 1 < Apps.Process.Processes[i].CurrChar)
									{
										Apps.Process.Processes[i].CurrChar = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length;

									}
									CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine];
								}
							}




						}

						break;
					case ConsoleKey.DownArrow:
						enterChar = false;
						if (AllowUpDown)
						{
							int i = Index;
							if (Apps.Process.Processes[i].lines.Count != 0)
							{
								if (Apps.Process.Processes[i].CurrLine < Apps.Process.Processes[i].lines.Count)
								{
									Apps.Process.Processes[i].CurrLine++;
									if (Apps.Process.Processes[i].lines[Apps.Process.Processes[i].CurrLine].text.Length - 1 > Apps.Process.Processes[i].CurrChar)
									{
										Apps.Process.Processes[i].CurrChar = Apps.Process.Processes[i].lines[Apps.Process.Processes[i].CurrLine].text.Length;
									}
								}

							}
							else if (Apps.Process.Processes[i].defaultLines.Count != 0)
							{
								if (Apps.Process.Processes[i].CurrLine < Apps.Process.Processes[i].defaultLines.Count-1)
								{
									Apps.Process.Processes[i].CurrLine++;

									if (Apps.Process.Processes[i].CurrLine >= Apps.Process.Processes[i].StartLine + (Apps.Process.Processes[i].SizeY - 50) / 18)
									{
										Apps.Process.Processes[i].StartLine++;
									}

									if (Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length - 1 < Apps.Process.Processes[i].CurrChar)
									{
										Apps.Process.Processes[i].CurrChar = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine].Length;

									}
									CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine];
								}
							}



						}
						break;

					case ConsoleKey.Enter:
						enterChar = false;
						switch (action)
						{
							case 1:
								{
									Terminal.enter();

								}
								break;
							case 2:
								{
									Notepad.enter();

								}
								break;
							case 3:
								{
									Notepad.save();
								}
								break;
							case 4:
								{
									RasInvoker.enter();
								}
								break;
							case 5:
								{
									RadiantWave.ChangeWebsite(Index, CurrentString);
								}
								break;
							case 6:
								{
									Apps.Process.Processes[Index].RasData.syncInput = true;
								}
								break;
							case 7:
								{
									DrawDesktopApps.FinishRenaming();
								}
								break;
						}
						break;
						enterChar = false;

				}
				if(charToEnter == ' ')
				{
					if (!isSpaceBar)
						return;
				}
				if (enterChar)
				{
					if (CurrentString.Length >= maxLenght)
						return;
					if(onlyNumbers)
					{
						if(charToEnter == '0' || charToEnter == '1' || charToEnter == '2' || charToEnter == '3'|| charToEnter == '4'|| charToEnter == '5'|| charToEnter == '6'|| charToEnter == '7'|| charToEnter == '8'|| charToEnter == '9')
						{
							if (CurrentString.Length < 1 && charToEnter == '0')
								return;
							if (long.Parse(InputSystem.CurrentString + charToEnter) > 2147483647)
								return;
						}
						else
						{
							return;
						}
					}
					for (int i = 0; i < Apps.Process.Processes.Count; i++)
					{
						if (Apps.Process.Processes[i].selected)
						{
							if (AllowArrows)
								Apps.Process.Processes[i].CurrChar++;

						}
					}

					if (AllowArrows && CurrentString.Length > 0)
					{

						string prefix = CurrentString.Substring(0, CurChar);
						string suffix = CurrentString.Substring(CurChar);
						CurrentString = prefix + charToEnter + suffix;

					}
					else
						CurrentString += charToEnter.ToString();


				}

			}
		}
	}
}
