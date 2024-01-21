using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace RadianceOS.System.Security
{
    public static class SecurityInstaller
    {
        // A really basic GUI to tell the user what it's doing while it sets stuff up like Security Policies, users, etc...
        // Also allows the user to turn on/off some basic settings

        static int width = 450;
        static int height = 200;
        static int x = 0;
        static int y = 0;

        static bool DrawCenterText = false;
        static string CenterText = "";

        static int renderLoop = 0;

        public static void Render()
        {
            /*int width = 450;
            int height = 200;

            int x = (int)((Explorer.screenSizeX / 2) - (width / 2));
            int y = (int)((Explorer.screenSizeY / 2) - (height / 2));*/

            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, x, y, width, height);

            if (DrawCenterText) RenderCenterText(CenterText);

            renderLoop++;
            InstallerActions[renderLoop - 1]();

            if(renderLoop > InstallerActions.Count)
            {
                DrawCenterText = true;
                CenterText = "Finishing off...";
            }
        }

        public static void Rerender()
        {
            Resize();

            //Explorer.CanvasMain.Clear();

            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, x, y, width, height);
            StringsAcitons.DrawCenteredString("Security Installer", width, x, y + 5, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

            StringsAcitons.DrawCenteredString("Please wait...", width, x, y + height - 25, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

            if (DrawCenterText) RenderCenterText(CenterText);
        }

        public static void Initialise()
        {
            Resize();

            DrawCenterText = true;
            CenterText = "Initialising Security installer...";
            Render();
        }

        private static void Resize() => Resize(width, height);

        private static void Resize(int width, int height)
        {
            SecurityInstaller.width = width;
            SecurityInstaller.height = height;

            x = (int)((Explorer.screenSizeX / 2) - (width / 2));
            y = (int)((Explorer.screenSizeY / 2) - (height / 2));
        }

        public static void RenderCenterText(string text)
        {
            StringsAcitons.DrawCenteredString(text, width, x, y + (height / 2) - 5, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);
        }

        static List<Action> InstallerActions = new List<Action>
        {
            new Action(() =>
            {
                CenterText = "Setting up UAC...";
                Rerender();
                Thread.Sleep(5000);
            }),
            new Action(() =>
            {
                Rerender();
                CenterText = "Finished setting up Security...";
                Rerender();
                Thread.Sleep(500);
                CenterText = "Returning to setup...";
                Rerender();
                Thread.Sleep(5000);
                Apps.NewInstaller.NewInstallator.RenderSecurityInstaller = false;
            })
        };
    }
}
