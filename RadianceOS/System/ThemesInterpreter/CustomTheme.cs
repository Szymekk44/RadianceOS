using RadianceOS.System.Graphic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.ThemesInterpreter
{
    public static class CustomTheme
    {

        public static void ParseTheme(string themeCode)
        {

            var lines = themeCode.Split('\n');
            foreach (var line in lines)
            {

                if (line.StartsWith("#"))
                {

                    var keys = line.TrimStart('#').Split(" ");

                    switch (keys[0])
                    {
                        //i know there are better ways to do it tho i am lazy, i will improve it later -Samma
                        case "main":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[]{'"','\''});

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.main = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.main = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "lightMain":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.lightMain = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.lightMain = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "lightlightMain":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.lightlightMain = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.lightlightMain = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "shadow":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.shadow = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.shadow = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "middark":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.middark = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.middark = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "dark":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.dark = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.dark = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "fontColor":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.fontColor = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.fontColor = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "terminalColor":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.terminalColor = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.terminalColor = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "startDefault":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.startDefault = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.startDefault = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "startLight":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.startLight = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.startLight = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "startDefaultSelected":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.startDefaultSelected = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.startDefaultSelected = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                        case "startLightSelected":
                            {

                                if (keys[1].StartsWith("\"") | keys[1].StartsWith("'"))
                                {

                                    string ts = keys[1].Trim(new char[] { '"', '\'' });

                                    for (int i = 2; i < keys.Length; i++)
                                    {

                                        ts += keys[i].Trim(new char[] { '"', '\'' });
                                        if (keys[i].Contains("\"") | keys[i].Contains("'"))
                                        {
                                            break;
                                        }

                                    }

                                    Kernel.startLightSelected = Color.FromName(ts);

                                }
                                else
                                {
                                    Kernel.startLightSelected = Color.FromArgb(int.Parse(keys[1]), int.Parse(keys[2]), int.Parse(keys[3]));
                                }

                            }
                            break;
                    }

                }

            }

        }

    }
}
