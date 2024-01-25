using Cosmos.System.Graphics;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp
{
	public static class RasInvoker
	{
		public static void RunCommand(string com, int RasID, int ProcessID)
		{
			
			string[] commands = com.Split(' ');
			string[] commandsDots = com.Split('.');
			commands[0] = commands[0].Replace("\n", "");
			commands[0] = commands[0].Trim();
			if (commands[0] == "void")
			{
				if (commands[1].Contains(RasPerformer.Data[RasID].currVoid))
				{
					int closingBraceIndex = com.IndexOf("{");


					if (closingBraceIndex != -1 && closingBraceIndex < com.Length - 1)
					{
						
						RunCommand(com.Substring(closingBraceIndex + 1).Trim(), RasID, ProcessID);
					}
			
				}
				else
				{
					RasPerformer.Data[RasID].CurrLine = int.MaxValue;
					return;

				}
			}
			else if (commandsDots[0] == "Console")
			{
				int startIndex = com.IndexOf("(");
				int endIndex = com.LastIndexOf(")");

				int startIndex2 = com.IndexOf(".");

				string text = "";
				string command = "";
				if (startIndex2 != -1 && startIndex != -1 && startIndex > startIndex2)
				{
					int length = startIndex - startIndex2 - 1;
					command = com.Substring(startIndex2 + 1, length);
				}

				if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
				{
					int length = endIndex - startIndex - 1;
					text = com.Substring(startIndex + 1, length);
				}




				if (command == "WriteLine")
				{
					if (text.Contains("+"))
					{
						string[] argss = text.Split('+');
						string readyString = "";
						readyString = ReturnString(argss, RasID, ProcessID);
						if (readyString != "")
						{
							TextColor toReturn2 = new TextColor
							{
								text = readyString,
								color = RasPerformer.Data[RasID].CurrColor
							};
							RasPerformer.Data[RasID].output.Add(toReturn2);
						}
						else
						{
							TextColor toReturn2 = new TextColor
							{
								text = "String is empty!",
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn2);
						}
					}
					else
					{
						string[] argss = new string[1];
						argss[0] = text;
						string readyString = ReturnString(argss, RasID, ProcessID);
						TextColor toReturn = new TextColor
						{
							text = readyString,
							color = RasPerformer.Data[RasID].CurrColor
						};
						RasPerformer.Data[RasID].output.Add(toReturn);
					}
					string[] args = text.Split('+');

				}
				else
				{
					TextColor toReturn = new TextColor
					{
						text = "Unknown Command - " + command,
						color = Color.Red
					};
					RasPerformer.Data[RasID].output.Add(toReturn);
					TextColor toReturn2 = new TextColor
					{
						text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
						color = Color.Red
					};
					RasPerformer.Data[RasID].output.Add(toReturn2);
				}
			}
			else if (commands[0] == "string")
			{
				if (commands.Length > 1)
				{
					if (commands.Length > 3)
					{
						if (commands[2] == "=")
						{
							List<string> rawValue = new List<string>();
							rawValue.Add(commands[3] + " ");
							for (int i = 4; i < commands.Length; i++)
							{
								rawValue[0] += commands[i] + " ";
							}
							string finaleValue = ReturnString(rawValue.ToArray(), RasID, ProcessID, commands[1]);
							if (finaleValue != "waitForInput....!@#$Ra#%")
								RasPerformer.Data[RasID].strings.Add(commands[1], finaleValue);
						}
						else
						{
							TextColor toReturn = new TextColor
							{
								text = "Unknown parametr - " + commands[2] + " - " + commands,
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn);
							TextColor toReturn2 = new TextColor
							{
								text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn2);
						}

					}
					else
					{
						RasPerformer.Data[RasID].strings.Add(commands[1], "");
					}
				}
				

			}
			else if (commands[0] == "int")
			{
				if (commands.Length > 1)
				{
					if (commands.Length > 3)
					{
						if (commands[2] == "=")
						{
							RasPerformer.Data[RasID].ints.Add(commands[1], int.Parse(commands[3]));
						}
						else
						{
							TextColor toReturn = new TextColor
							{
								text = "Unknown parametr - " + commands[2] + " - " + commands,
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn);
							TextColor toReturn2 = new TextColor
							{
								text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn2);
						}

					}
					else
					{
						RasPerformer.Data[RasID].ints.Add(commands[1], 0);
					}
				}

			}
			else if (commands[0] == "Color")
			{
				if (commands[1] == "=")
				{
					RasPerformer.Data[RasID].CurrColor = Color.FromName(commands[2]);
				}
				else
				{
					TextColor toReturn = new TextColor
					{
						text = "Unknown parametr - " + commands[2] + " - " + commands,
						color = Color.Red
					};
					RasPerformer.Data[RasID].output.Add(toReturn);
					TextColor toReturn2 = new TextColor
					{
						text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
						color = Color.Red
					};
					RasPerformer.Data[RasID].output.Add(toReturn2);
				}
			}
			else if (commands.Length > 1)
			{
				int firstSpaceIndex = com.IndexOf(' ');

				int secondSpaceIndex = com.IndexOf(' ', firstSpaceIndex + 1);
				string resultString = com.Substring(secondSpaceIndex + 1);
				if (commands[1] == "=")
				{

					if (RasPerformer.Data[RasID].strings.ContainsKey(commands[0]))
					{

						if (com.Contains("+"))
						{

							if (firstSpaceIndex != -1 && secondSpaceIndex != -1)
							{
								resultString = com.Substring(secondSpaceIndex + 1);
								string[] argss = resultString.Split('+');
								stringOperations(RasID, 0, commands[0], ReturnString(argss, RasID, ProcessID));
							}
						}
						else
						{
							List<string> strings = new List<string>();
							strings.Add(resultString);
							stringOperations(RasID, 0, commands[0], ReturnString(strings.ToArray(), RasID, ProcessID));
						}
					}
					else if (RasPerformer.Data[RasID].ints.ContainsKey(commands[0]))
					{

						string input = com.Substring(com.IndexOf('=') + 1); //everything after =
						string expression = input.Substring(input.IndexOf('=') + 1).Trim();

						if (expression.Contains("+") || expression.Contains("-") || expression.Contains("*") || expression.Contains("/"))
						{
							List<string> operacje = new List<string>();

							int start = 0;
							for (int i = 0; i < expression.Length; i++)
							{
								if (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/')
								{
									
									operacje.Add(expression.Substring(start, i - start).Trim());
									
									operacje.Add(expression[i].ToString());
								
									start = i + 1;
								}
							}

				
							operacje.Add(expression.Substring(start).Trim());

							
							int result = UpdateInt(operacje.ToArray(), RasID);
							RasPerformer.Data[RasID].ints[commands[0]] = result;

						}
					}
					else if (commands[1] == "+=")
					{
						com.Remove(com.IndexOf("+"));
						if (RasPerformer.Data[RasID].strings.ContainsKey(commands[0]))
						{

							if (com.Contains("+"))
							{

								if (firstSpaceIndex != -1 && secondSpaceIndex != -1)
								{
									resultString = com.Substring(secondSpaceIndex + 1);
									string[] argss = resultString.Split('+');
									stringOperations(RasID, 1, commands[0], ReturnString(argss, RasID, ProcessID));
								}
							}
							else
							{
								List<string> strings = new List<string>();
								strings.Add(resultString);
								stringOperations(RasID, 1, commands[0], ReturnString(strings.ToArray(), RasID, ProcessID));
							}
						}
					}
				}
				else if (commands[0] == "}" || commands[0] == "{")
				{
				
					int closingBraceIndex = com.IndexOf("{");
				

					if (closingBraceIndex != -1 && closingBraceIndex < com.Length - 1)
					{
						RunCommand(com.Substring(closingBraceIndex + 1).Trim(), RasID, ProcessID);
					}

				}
				else if (commands[0] != ""  && commands[0] != "(" && commands[0] != ")" )
				{
					TextColor toReturn = new TextColor
					{
						text = "Unknown Command - " + commands[0],
						color = Color.Red
					};
					RasPerformer.Data[RasID].output.Add(toReturn);
					TextColor toReturn2 = new TextColor
					{
						text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
						color = Color.Red
					};
					RasPerformer.Data[RasID].output.Add(toReturn2);
				}
			
				

			}
		}

		public static void stringOperations(int RasID, int operation, string stringName, string value)
		{
			switch (operation)
			{
				case 0: // =
					{
						RasPerformer.Data[RasID].strings[stringName] = value;
					}
					break;
				case 1: // +=
					{
						RasPerformer.Data[RasID].strings[stringName] += value;
					}
					break;
			}
		}

		public static bool ReturnBool(String[] argss)
		{

			return false;
		}

		public static string ReturnString(String[] argss, int RasID, int ProcessID, string sname = default)
		{
			if (sname == default)
				sname = "";
			string readyString = "";
			for (int i = 0; i < argss.Length; i++)
			{
				if (argss[i].Contains('"'))
				{
					argss[i] = argss[i].Trim();
					if (argss[i].Length >= 2 && argss[i][0] == '"' && argss[i][argss[i].Length - 1] == '"')
					{
						argss[i] = argss[i].Substring(1, argss[i].Length - 2);
						readyString += argss[i];
					}
				}
				else
				{
					argss[i] = argss[i].Replace(" ", "");
					if (argss[i] == "Console.ReadLine()")
					{

						RasPerformer.Data[RasID].GetInput = true;
						TextColor toReturn = new TextColor
						{
							text = "",
							color = RasPerformer.Data[RasID].CurrColor
						};
						RasPerformer.Data[RasID].output.Add(toReturn);
						InputSystem.CurrentString = "";
						Process.Processes[ProcessID].CurrChar = 0;
						RasPerformer.Data[RasID].tempStringName = sname;
						return "waitForInput....!@#$Ra#%";
					}
					else
					{
						if (RasPerformer.Data[RasID].strings.ContainsKey(argss[i]))
						{
							readyString += RasPerformer.Data[RasID].strings[argss[i]];
						}
						else if(argss[i].Contains(".ToString()"))
						{
							argss[i] = argss[i].Substring(0, argss[i].Length - ".ToString()".Length);
							if (RasPerformer.Data[RasID].ints.ContainsKey(argss[i]))
							{
								readyString += RasPerformer.Data[RasID].ints[argss[i]].ToString();
							}
							else
							{
								TextColor toReturn = new TextColor
								{
									text = "Int " + argss[i] + " does not exist!",
									color = Color.Red
								};
								RasPerformer.Data[RasID].output.Add(toReturn);
								TextColor toReturn2 = new TextColor
								{
									text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
									color = Color.Red
								};
								RasPerformer.Data[RasID].output.Add(toReturn2);
								return "";
							}
						}
						else
						{
							TextColor toReturn = new TextColor
							{
								text = "String " + argss[i] + " does not exist!",
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn);
							TextColor toReturn2 = new TextColor
							{
								text = "Line: " + (RasPerformer.Data[RasID].CurrLine + 1),
								color = Color.Red
							};
							RasPerformer.Data[RasID].output.Add(toReturn2);
							return "";
						}
					}
					
				}
			}
			return readyString;
		}


		public static int UpdateInt(string[] operations, int RasID)
		{
			if (operations == null || operations.Length % 2 != 1)
			{
				TextColor toReturn2 = new TextColor
				{
					text = "Incorrect operations. There is no operation or the number of operations/operands is not even.",
					color = Color.Red
				};
				RasPerformer.Data[RasID].output.Add(toReturn2);
				return -1;
			}

			List<string> tempOperands = new List<string>();
			List<string> tempOperators = new List<string>();

			for (int i = 0; i < operations.Length; i++)
			{
				if (i % 2 == 0) //numbers
				{
					tempOperands.Add(operations[i]);
				}
				else //operators
				{
					tempOperators.Add(operations[i]);
				}
			}

			//multiplication and division
			for (int i = 0; i < tempOperators.Count; i++)
			{
				if (tempOperators[i] == "*" || tempOperators[i] == "/")
				{
					int leftOperand = int.Parse(tempOperands[i]);
					int rightOperand = int.Parse(tempOperands[i + 1]);

					int result = tempOperators[i] == "*" ? leftOperand * rightOperand : leftOperand / rightOperand;

					
					tempOperands[i] = result.ToString();
					tempOperands.RemoveAt(i + 1);
					tempOperators.RemoveAt(i);

					
					i--;
				}
			}

			//Addition and subtraction
			int finalResult = int.Parse(tempOperands[0]);
			for (int i = 0; i < tempOperators.Count; i++)
			{
				int nextOperand = int.Parse(tempOperands[i + 1]);

				if (tempOperators[i] == "+")
				{
					finalResult += nextOperand;
				}
				else if (tempOperators[i] == "-")
				{
					finalResult -= nextOperand;
				}
			}

			return finalResult;
		}


		public static void enter() //Input
		{
			for (int i = 0; i < Process.Processes.Count; i++)
			{
				if (Process.Processes[i].ID == 3)
				{
					if (Process.Processes[i].selected)
					{
						if (RasPerformer.Data[Process.Processes[i].tempInt].tempStringName != null)
						{
							if (RasPerformer.Data[Process.Processes[i].tempInt].tempStringName != "")
							{
								RasPerformer.Data[Process.Processes[i].tempInt].strings.Add(RasPerformer.Data[Process.Processes[i].tempInt].tempStringName, InputSystem.CurrentString);
								RasPerformer.Data[Process.Processes[i].tempInt].GetInput = false;
							}
						}
					}
				}

			}
		}
	}

}
