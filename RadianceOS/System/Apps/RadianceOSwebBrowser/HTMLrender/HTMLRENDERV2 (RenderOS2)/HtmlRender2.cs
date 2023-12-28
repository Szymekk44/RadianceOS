using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using CosmosTTF;
using HtmlAgilityPack;
using IL2CPU.API.Attribs;
using RadianceOS.Render;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Canvas = Cosmos.System.Graphics.Canvas;

namespace webkerneltest.HTMLRENDERV2
{
    public static class HtmlRender2
    {

        public static Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>();

        public static int PagePos = 10;

        static Canvas canv;

        static Point Pos = Point.Empty;

        static CSSRules PageStyle = new CSSRules();

        static Rectangle renderpoint = Rectangle.Empty;

        private static void RenderNode(HtmlNode node,Color col,int ProcessID, int TextSize = 14,bool centered = false)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Document:
                    RenderDocument(node, ProcessID);
                    break;

                case HtmlNodeType.Element:
                    RenderElement(node,col, ProcessID, centered);
                    break;

                case HtmlNodeType.Text:
                    RenderText(node,col,TextSize,centered);
                    break;

                case HtmlNodeType.Comment:
                    RenderComment(node);
                    break;
            }
        }

        private static void RenderDocument(HtmlNode node, int ProcessID)
        {

            // Render all child nodes.
            foreach (var childNode in node.ChildNodes)
            {
                RenderNode(childNode,Color.Black, ProcessID);
            }

        }

        private static void RenderElement(HtmlNode node,Color col, int ProcessID, bool centered = false)
        {

            Dictionary<string, string> attribs = new Dictionary<string, string>();

            // Render all attributes.
            foreach (var attribute in node.Attributes)
            {
                attribs.Add(attribute.Name,attribute.Value);
            }

            int tsize = 14;
            
            switch (node.Name)
            {

                case "h1":
                    tsize = 40;
                    break;
                case "h2":
                    tsize = 30;
                    break;
                case "h3":
                    tsize = 20;
                    break;
                case "h4":
                    tsize = 18;
                    break;
                case "a":
                    if (attribs.ContainsKey("href"))
                    {
                        col = Color.Blue;
                    }
                    break;
                case "button":
                    canv.DrawFilledRectangle(Color.LightGray,Pos.X+10, Pos.Y + PagePos,node.InnerText.Length * 8,18);
                    ElementData ed = new ElementData
                    {
                        x = Pos.X + 10, y = Pos.Y + PagePos, SizeX = node.InnerText.Length * 8, SizeY = 18, type = 0,
                        url = node.InnerHtml
                    };
                    Process.Processes[ProcessID].webData.elements.Add(ed);



                    if (node.ChildNodes.Count == 0)
                    {
                        PagePos += 23;
                    }

                    break;
                case "input":
                    canv.DrawFilledRectangle(Color.White, Pos.X + 10, Pos.Y + PagePos, 200, 18);
                    canv.DrawRectangle(Color.LightGray, Pos.X + 10, Pos.Y + PagePos, 200, 18);
                    PagePos += 23;
                    break;
                case "img":

                    if (attribs.ContainsKey("src") && resources.ContainsKey(attribs["src"]))
                    {

                        Bitmap img = new Bitmap(resources[attribs["src"]]);
                        RadianceOS.Render.Canvas.DrawImageAlpha(img, Pos.X + 10, Pos.Y + PagePos);
                        PagePos += (int)img.Height + 5;

                    }
                    else
                    {
                        if (attribs.ContainsKey("alt"))
                        {
                            if (!string.IsNullOrWhiteSpace(attribs["alt"]))
                            {
                                canv.DrawStringTTF(attribs["alt"], "UMR", Color.Black, tsize, Pos.X + 10, Pos.Y + PagePos + tsize);
                                PagePos += tsize + 5;
                            }
                        }
                    }

                    //canv.DrawFilledRectangle(Color.Blue, Pos.X + 10, Pos.Y + PagePos, 50, 50);
                    //PagePos += 50;
                    
                    break;
                case "style":
                    PageStyle = CssParser.Update(node.InnerText,PageStyle);
                    break;
                case "body":

                    if (PageStyle.BackgroundColor("body") != Color.White)
                        canv.DrawFilledRectangle(PageStyle.BackgroundColor("body"), renderpoint.X, renderpoint.Y, renderpoint.Width, renderpoint.Height);

                    break;
                case "meta":
                case "script":
                case "title":
                    return;
                default:
                    break;
            }

            if (attribs.ContainsKey("class"))
            {

                if (!string.IsNullOrWhiteSpace(attribs["class"]))
                {

                    var classnames = attribs["class"].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in classnames)
                    {

                        col = PageStyle.TextColor($".{item}");
                        centered = PageStyle.TextAlign($".{item}");

                    }

                }

            }

            // Render all child nodes.
            foreach (var childNode in node.ChildNodes)
            {
                RenderNode(childNode,col,ProcessID,tsize,centered);
            }

        }
        
        private static void RenderText(HtmlNode node,Color col,int fontsize = 14,bool centered = false)
        {
            // Render a text node.
            if (!string.IsNullOrWhiteSpace(node.InnerHtml))
            {

                if (centered)
                {
                    StringsAcitons.DrawCenteredTTFString(node.InnerHtml,renderpoint.Width - 10,Pos.X + 10, Pos.Y + PagePos + fontsize,1,col,"UMR",fontsize);
                }
                else
                {
                    canv.DrawStringTTF(node.InnerHtml, "UMR", col, fontsize, Pos.X + 10, Pos.Y + PagePos + fontsize);
                }
                PagePos += fontsize + 5;
            }
            
        }

        private static void RenderComment(HtmlNode node)
        {
        }

        public static void RenderHTML(this Canvas Canv,string HTMLCODE,int X=0,int Y=0,int Widht=1280,int Height=720, int ProcessID = 0)
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTMLCODE);
            PageStyle = CssParser.Empty();

            canv = Canv;

            Pos = new Point(X,Y);

            renderpoint.X = X;
            renderpoint.Y = Y;
            renderpoint.Width = Widht;
            renderpoint.Height = Height;
            canv.DrawFilledRectangle(Color.White,X, Y, Widht, Height);
            RenderNode(htmlDoc.DocumentNode,Color.Black, ProcessID);
        }


        public static void AddResource(string name, byte[] resourcedata)
        {

            resources.Add(name,resourcedata);

        }

        public static void EditResource(string name, byte[] resourcedata)
        {

            resources[name] = resourcedata;

        }

        public static void RemoveResource(string name)
        {

            resources.Remove(name);

        }

    }
}
