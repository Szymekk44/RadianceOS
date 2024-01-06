using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security.Auth
{
    public static class Session
    {
        public static bool IsAuthenticated { get; private set; }
        public static DateTime? AuthenticatedAt { get; private set; }
        public static string UserName { get; private set; }
        public static bool IsLocked { get; private set; }
        /// <summary>
        /// An upgradable authentication system
        /// </summary>
        /// <param name="username">Their username</param>
        /// <param name="password">Their password</param>
        /// <param name="force">If there's an active user, you can forcefully bypass that and log them out so you can log in.</param>
        /// <returns>0 = Success, 1 = Wrong password, 2 = User not found, 3 = User already logged in (Do you want to force log in and log the other user out?)</returns>
        public static int Authenticate(string username, string password, bool? force)
        {
            // This can be easily upgraded
            if(!string.IsNullOrEmpty(username))
            {
                if(force != true)
                {
                    return 3;
                }
            }

            if(File.Exists(@"0:\Users\" + username + @"\"))
            {
                if(password == File.ReadAllText(@"0:\Users\" + username + @"\AccountInfo\Password.SysData"))
                {
                    AuthenticatedAt = DateTime.Now;
                    StartSession(username);
                    return 0;
                } else
                {
                    return 1;
                }
            } else
            {
                return 2;
            }
        }
        /// <summary>
        /// Logs the user out and returns them to the login screen
        /// </summary>
        public static void Logout()
        {
            IsAuthenticated = false;
            AuthenticatedAt = null;
            UserName = "";
            IsLocked = false;
        }
        /// <summary>
        /// Returns the user to the login screen but it's slightly different because it keeps them logged in
        /// </summary>
        public static void LockSession()
        {
            IsLocked = true; // So the login screen knows what to display
            IsAuthenticated = false; // So it shows the login screen
        }

        private static void StartSession(string username)
        {
            // Initialises the theme and things
            Kernel.loggedUser = username;

            if (File.Exists(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat"))
            {
                try
                {
                    int theme = int.Parse(File.ReadAllText(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat"));
                    Design.ChangeTheme(theme);
                    Kernel.Theme = theme;
                }
                catch (Exception e)
                {
                    MessageBoxCreator.CreateMessageBox("Config Error", "Theme config was corrupted!\nRadianceOS has restored default settings." + e.Message, MessageBoxCreator.MessageBoxIcon.warning, 500, 175);
                    File.Delete(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat");
                    File.Create(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat");
                    File.WriteAllText(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat", "0");
                }
            }

            Radiance.Security.Logged = true;
            if (Kernel.render)
            {
                Process.Processes.RemoveAt(1); // Kill the login screen, which might be disabled later and just hide it
                Explorer.drawIcons = true;
                DrawDesktopApps.UpdateIcons();
                Explorer.DrawTaskbar = true;
            }
        }
    }
}
