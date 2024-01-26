using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4.UDP.DNS;
using Cosmos.System.Network.IPv4;
using RadianceOS.System.Graphic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using webkerneltest.HTMLRENDERV2;
using System.Diagnostics;
using Cosmos.Core.Memory;
using System.IO;
using Cosmos.System.Graphics;
using RadianceOS.System.Managment;
using Cosmos.System;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using HtmlAgilityPack;

namespace RadianceOS.System.Apps.RadianceOSwebBrowser
{
	public static class RadiantWave
	{

		static HtmlDocument document = new();

		public static void LoadWebsite(int ProcessID)
		{
			try
			{
				sysStatus.DrawBusy("Requesting data");
				using (TcpClient client = new TcpClient())
				{
					var dnsClient = new DnsClient();

					// DNS
					dnsClient.Connect(DNSConfig.DNSNameservers[0]);
					dnsClient.SendAsk("szymekk.pl");

					// Address from ip
					Address address = dnsClient.Receive();
					dnsClient.Close();
					string serverIp = address.ToString();
					int serverPort = 80;

					client.Connect(serverIp, serverPort);
					NetworkStream stream = client.GetStream();

					string url = "szymekk.pl/RadianceOS/index.html";
					Apps.Process.Processes[ProcessID].texts[0] = url;
					Apps.Process.Processes[ProcessID].CurrChar = Apps.Process.Processes[ProcessID].texts[0].Length;
					sysStatus.DrawBusy("Requesting data");
					string[] urlAddress = url.Split('/');
					string webAddress = "";
					for (int i = 1; i < urlAddress.Length; i++)
					{
						webAddress += "/";
						webAddress += urlAddress[i];
					}

					string httpget = "GET "  +"/RadianceOS.html" + " HTTP/1.1\r\n" +
								 "User-Agent: RadianceOS\r\n" +
								 "Accept: */*\r\n" +
								 "Accept-Encoding: identity\r\n" +
								 "Host: szymekk.pl\r\n" +
								 "Connection: Keep-Alive\r\n\r\n";
					string messageToSend = httpget;
					byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
					stream.Write(dataToSend, 0, dataToSend.Length);

					/** Receive data **/
					byte[] receivedData = new byte[client.ReceiveBufferSize];
					int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
					string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);


					string[] responseParts = receivedMessage.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

					if (responseParts.Length == 2)
					{
						string headers = responseParts[0];
						string content = responseParts[1];
						document = new();
						document.LoadHtml(content);
					}

					/** Close data stream **/
					stream.Close();
				}

				HtmlRender2.resources.Clear();

				HtmlRender2.AddResource(@"skk.png", Kernel.skk);
				HtmlRender2.AddResource(@"ok.png", Kernel.ok);
				HtmlRender2.AddResource(@"Radiance.png", Files.RadianceOSIconTransparent);
				HtmlRender2.AddResource(@"RadianceShadow.png", Files.RadianceOSIconShadow);
				HtmlRender2.AddResource(@"https://i.creativecommons.org/l/by-nd/4.0/80x15.png", Kernel.cc);
				HtmlRender2.PagePos = 10;
			}
			catch(Exception ex)
			{
				Kernel.Crash("RadiantWave Error: " + ex.Message, 10);
			}
		}
		public static void Render(int X, int Y, int SizeX, int SizeY, int ProcessID)
		{
			Window.DrawTop(ProcessID,X, Y, SizeX, "RadiantWave - RadianceOS Web Browser", false, true, true);


			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			if(Apps.Process.Processes[ProcessID].bitmap == null)
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
				sysStatus.DrawBusy("Rendering page");
				Heap.Collect();
				Explorer.CanvasMain.RenderHTML(document, X, Y + 25, SizeX, SizeY -35, ProcessID);
				Heap.Collect();

				int width = SizeX;
				int height = SizeY - 60;

				Color[] colors = new Color[width * height];
				Heap.Collect();

				for (int y = Y + 25, destY = 0; y < Y + SizeY - 35; y++, destY++)
				{
					for (int x = X, destX = 0; x < X + SizeX; x++, destX++)
					{
						colors[(height - 1 - destY) * width + destX] = Explorer.CanvasMain.GetPointColor(x, y);
					}
				}

				Heap.Collect();
				byte[] pixelData = new byte[width * height * 4]; 
				Heap.Collect();
				int index = 0;
				foreach (Color pixelColor in colors)
				{
					pixelData[index++] = pixelColor.B;
					pixelData[index++] = pixelColor.G;
					pixelData[index++] = pixelColor.R;
					pixelData[index++] = pixelColor.A;
				}
			
				Heap.Collect();

				// Utwórz nagłówek pliku BMP
				int fileSize = 54 + pixelData.Length; // 54 bajty to rozmiar nagłówka BMP
				byte[] fileHeader = new byte[54];
					Heap.Collect();
				fileHeader[0] = 66; // B
				fileHeader[1] = 77; // M
				BitConverter.GetBytes(fileSize).CopyTo(fileHeader, 2);
				BitConverter.GetBytes(54).CopyTo(fileHeader, 10);
				BitConverter.GetBytes(40).CopyTo(fileHeader, 14);
				BitConverter.GetBytes(width).CopyTo(fileHeader, 18);
				BitConverter.GetBytes(height).CopyTo(fileHeader, 22); // Uwaga: zmieniono wysokość
				BitConverter.GetBytes(1).CopyTo(fileHeader, 26);
				BitConverter.GetBytes(32).CopyTo(fileHeader, 28); // Zmieniono format na 32-bitowy

				Heap.Collect();

				// Utwórz obiekt Bitmap
				try
				{
					byte[] byteArray = fileHeader.Concat(pixelData).ToArray();
					Apps.Process.Processes[ProcessID].bitmap = new Bitmap(byteArray);
				}
				catch
				{
					Kernel.Crash("RadiantWave bitmap creation failed!", 11);
				}

				Heap.Collect();


			}
			else
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, 35);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 60, Y + 27, SizeX - 120, 29);
				if(!Apps.Process.Processes[ProcessID].selected)
				{
					Explorer.CanvasMain.DrawString("http://" + Apps.Process.Processes[ProcessID].texts[0], Kernel.font18, Kernel.fontColor, X+65, Y+33);
				}
				else
				{
					InputSystem.Monitore(5, Apps.Process.Processes[ProcessID].CurrChar, ProcessID);
					InputSystem.AllowArrows = true;
					InputSystem.AllowUpDown = false;
					InputSystem.SpecialCharracters = true;
					Apps.Process.Processes[ProcessID].CurrLine = 0;
					Apps.Process.Processes[ProcessID].texts[0] = InputSystem.CurrentString;
					string result = Apps.Process.Processes[ProcessID].texts[0].Substring(0, Apps.Process.Processes[ProcessID].CurrChar) + "|" + Apps.Process.Processes[ProcessID].texts[0].Substring(Apps.Process.Processes[ProcessID].CurrChar);
					Explorer.CanvasMain.DrawString("http://" + result, Kernel.font18, Kernel.fontColor, X + 65, Y + 33);
				}
				Explorer.CanvasMain.DrawImage(Apps.Process.Processes[ProcessID].bitmap, X, Y+60);
				for (int i = 0; i < Apps.Process.Processes[ProcessID].webData.elements.Count; i++)
				{
					if(MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
					{
						if (Explorer.MX > Apps.Process.Processes[ProcessID].webData.elements[i].x + X && Explorer.MX < Apps.Process.Processes[ProcessID].webData.elements[i].x + Apps.Process.Processes[ProcessID].webData.elements[i].SizeX + X)
						{
							if (Explorer.MY > Apps.Process.Processes[ProcessID].webData.elements[i].y + Y && Explorer.MY < Apps.Process.Processes[ProcessID].webData.elements[i].y + Apps.Process.Processes[ProcessID].webData.elements[i].SizeY + Y)
							{
								if (Apps.Process.Processes[ProcessID].webData.elements[i].url != Apps.Process.Processes[ProcessID].texts[0])
								{
									if(!Apps.Process.Processes[ProcessID].webData.elements[i].download)
									{
										string newUrl = Apps.Process.Processes[ProcessID].webData.elements[i].url;
										if (newUrl.StartsWith("https"))
										{
											MessageBoxCreator.CreateMessageBox("RadiantWave", "RadiantWave does not support HTTPS connections\nTrying to connect via http", MessageBoxCreator.MessageBoxIcon.warning, 550);
											newUrl = newUrl.Substring(8);
										}
										else if (newUrl.StartsWith("http"))
											newUrl = newUrl.Substring(7);
										else
										{
											MessageBoxCreator.CreateMessageBox("RadiantWave Error", "Unknow URL.\nRadiantWave does not yet support JavaScript!\n" + newUrl, MessageBoxCreator.MessageBoxIcon.error, 550);
											return;
										}

										ChangeWebsite(ProcessID, newUrl);
									}
									else
									{

										string newUrl = Apps.Process.Processes[ProcessID].webData.elements[i].url;
										if (newUrl.StartsWith("https"))
										{
											MessageBoxCreator.CreateMessageBox("RadiantWave", "RadiantWave does not support HTTPS connections\nTrying to connect via http", MessageBoxCreator.MessageBoxIcon.warning, 550);
											newUrl = newUrl.Substring(8);
										}
										else if (newUrl.StartsWith("http"))
											newUrl = newUrl.Substring(7);
										else
										{
											MessageBoxCreator.CreateMessageBox("RadiantWave Error", "Unknow URL.\nRadiantWave does not yet support JavaScript!\n" + newUrl, MessageBoxCreator.MessageBoxIcon.error, 550);
											return;
										}
										sysStatus.DrawBusy("Requesting data");
										string[] urlAddress = newUrl.Split('/');
										string webAddress = "";
										for (int j = 1; j < urlAddress.Length; j++)
										{
											webAddress += "/";
											webAddress += urlAddress[j];
										}

										using (TcpClient client = new TcpClient())
										{
											var dnsClient = new DnsClient();

											// DNS
											dnsClient.Connect(DNSConfig.DNSNameservers[0]);
											dnsClient.SendAsk(urlAddress[0]);

											// Address from ip
											Address address = dnsClient.Receive();
											dnsClient.Close();
											string serverIp = address.ToString();
											int serverPort = 80;

											client.Connect(serverIp, serverPort);
											NetworkStream stream = client.GetStream();
											string httpget = "GET " + webAddress + " HTTP/1.1\r\n" +
														 "User-Agent: RadianceOS\r\n" +
														 "Accept: */*\r\n" +
														 "Accept-Encoding: identity\r\n" +
														 "Host: " + urlAddress[0] + "\r\n" +
														 "Connection: Keep-Alive\r\n\r\n";
											string messageToSend = httpget;
											byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
											stream.Write(dataToSend, 0, dataToSend.Length);

											/** Receive data **/
											byte[] receivedData = new byte[client.ReceiveBufferSize];
											int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
											string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);


											string[] responseParts = receivedMessage.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

											if (responseParts.Length == 2)
											{
												string headers = responseParts[0];
												string content = responseParts[1];
												Apps.Process.Processes[ProcessID].temp = content;
											}
								

											/** Close data stream **/
											stream.Close();
											File.Create(@"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + urlAddress[urlAddress.Length - 1]);
											if (!responseParts[1].Contains('\n'))
											File.WriteAllText(@"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + urlAddress[urlAddress.Length - 1], responseParts[1]);
											else
											{
												string[] lines = responseParts[1].Split('\n');
						

												File.WriteAllLines(@"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + urlAddress[urlAddress.Length - 1], lines);
												MessageBoxCreator.CreateMessageBox("Yes", "yes");
											}
											DrawDesktopApps.Render();

										}


										
					
										
									}
								}
							}
						}
					}
				}
			}

		}
		static Bitmap Base64ToImage(string base64String)
		{
			byte[] imageBytes = Convert.FromBase64String(base64String);

				return new Bitmap(imageBytes);
			
		}
		static int tries = 0;
		public static void ChangeWebsite(int ProcessID, string url)
		{
			try
			{
				Apps.Process.Processes[ProcessID].bitmap = null;
				InputSystem.CurrentString = url;
				Apps.Process.Processes[ProcessID].texts[0] = url;
				Apps.Process.Processes[ProcessID].CurrChar = Apps.Process.Processes[ProcessID].texts[0].Length;
				sysStatus.DrawBusy("Requesting http data");
				string[] urlAddress = url.Split('/');
				string webAddress = "";
				for (int i = 1; i < urlAddress.Length; i++)
				{
					webAddress += "/";
					webAddress += urlAddress[i];
				}
				using (TcpClient client = new TcpClient())
				{
					var dnsClient = new DnsClient();

					// DNS
					dnsClient.Connect(DNSConfig.DNSNameservers[0]);
					dnsClient.SendAsk(urlAddress[0]);

					// Address from ip
					Address address = dnsClient.Receive();
					dnsClient.Close();
					string serverIp = address.ToString();
					int serverPort = 80;

					client.Connect(serverIp, serverPort);
					NetworkStream stream = client.GetStream();
					string httpget = "GET " + webAddress+ " HTTP/1.1\r\n" +
								 "User-Agent: RadianceOS\r\n" +
								 "Accept: */*\r\n" +
								 "Accept-Encoding: identity\r\n" +
								 "Host: " + urlAddress[0] +"\r\n" +
								 "Connection: Keep-Alive\r\n\r\n";
					string messageToSend = httpget;
					byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
					stream.Write(dataToSend, 0, dataToSend.Length);

					/** Receive data **/
					byte[] receivedData = new byte[client.ReceiveBufferSize];
					int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
					string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);


					string[] responseParts = receivedMessage.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

					if (responseParts.Length >= 2)
					{
						string headers = responseParts[0];
						string content = responseParts[1];

						document = new();
						document.LoadHtml(content);

						Apps.Process.Processes[ProcessID].temp = content;
			
					}
					else
					{
						//MessageBoxCreator.CreateMessageBox("RadiantWave Error", "Error generating html", MessageBoxCreator.MessageBoxIcon.error);
						ChangeWebsite(ProcessID, webAddress);
						return;
					}
					if(responseParts[0].Length > 8)
					{
						string info ="";
						info += responseParts[0][9];
						info += responseParts[0][10];
						info += responseParts[0][11];
						if (info == "400" && tries == 0)
						{
							tries++;
							ChangeWebsite(ProcessID, url + "/");
							return;
						}
						else
							tries = 0;
					}
					
					/** Close data stream **/
					stream.Close();
				}
				
				Apps.Process.Processes[ProcessID].selected = false;
				HtmlRender2.resources.Clear();

				HtmlRender2.AddResource(@"skk.png", Kernel.skk);
				HtmlRender2.AddResource(@"ok.png", Kernel.ok);
				HtmlRender2.AddResource(@"Radiance.png", Files.RadianceOSIconTransparent);
				HtmlRender2.AddResource(@"RadianceShadow.png", Files.RadianceOSIconShadow);

				HtmlRender2.AddResource(@"https://i.creativecommons.org/l/by-nd/4.0/80x15.png", Kernel.cc);
				HtmlRender2.PagePos = 5;
			}
			catch (Exception ex)
			{
				Kernel.Crash("RadiantWave Error: " + ex.Message, 10);
			}
		}

	}
}
