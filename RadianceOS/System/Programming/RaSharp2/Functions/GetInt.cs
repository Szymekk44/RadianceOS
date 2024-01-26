using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RadianceOS.System.Programming.RaSharp2.Functions
{
	public static class GetInt
	{
		public static void MakeInt(string[] paramets, int ProcessID, string com, int i)
		{
			if (paramets.Count() > 2)
			{
				string temp = com.Trim();
				int finaleInt = ReturnInt(ProcessID,com,Apps.Process.Processes[ProcessID].DataID);


				string VarName = temp.Substring(3, temp.IndexOf('=')-3);
				VarName = VarName.Trim();
				RasExecuter.Data[Apps.Process.Processes[ProcessID].DataID].variables.Add(VarName, finaleInt);
			}
			else
			{
				RasExecuter.Data[Apps.Process.Processes[ProcessID].DataID].variables.Add(paramets[1], 0);
			}
		}

		public static int ReturnInt(int ProcessID, string com, int RasID)
		{
			string input = com.Substring(com.IndexOf('=') + 1); //everything after =
			string expression = input.Trim();
			if(expression.Contains(';')) 
			{
				expression = expression.Replace(';', ' ');
				expression = expression.Trim();
			}
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


				int result = UpdateInt(operacje.ToArray(), ProcessID);
				return result;
			}
			else
			{
				if (RasExecuter.Data[RasID].variables.ContainsKey(expression))
					return Convert.ToInt32(RasExecuter.Data[RasID].variables[expression]);
				return Convert.ToInt32(expression);
			}
				


		}

		public static int UpdateInt(string[] operations, int ProcessID)
		{
			if (operations == null || operations.Length % 2 != 1)
			{
				TextColor toReturn2 = new TextColor
				{
					text = "Incorrect operations. There is no operation or the number of operations/operands is not even.",
					color = Color.Red
				};
				Apps.Process.Processes[ProcessID].lines.Add(toReturn2);
				return -1;
			}

			List<string> tempOperands = new List<string>();
			List<string> tempOperators = new List<string>();

			for (int i = 0; i < operations.Length; i++)
			{
				if (i % 2 == 0) //numbers
				{
					int numvalue;
					if (int.TryParse(operations[i], out numvalue))
						tempOperands.Add(operations[i]);
					else
					{
						operations[i] = operations[i].Trim();
						if (RasExecuter.Data[Apps.Process.Processes[ProcessID].DataID].variables.ContainsKey(operations[i]))
						{
							tempOperands.Add(RasExecuter.Data[Apps.Process.Processes[ProcessID].DataID].variables[operations[i]].ToString());
						}
					}
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
	}

	
}
