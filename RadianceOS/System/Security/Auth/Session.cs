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
        public static DateTime AuthenticatedAt { get; private set; }
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

        }
        /// <summary>
        /// Returns the user to the login screen but it's slightly different because it keeps them logged in
        /// </summary>
        public static void LockSession()
        {

        }

        private static void StartSession(string username)
        {
            // Initialises the theme and things
        }
    }
}
