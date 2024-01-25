using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security
{
    public static class Service
    {
        /**
         * This is the background task (Service) of Radiance Security.
         * --- Killing this process will cause a crash (Apps will think that killing it will make it easier for them to do bad stuff)
         * This is something built into Explorer, you cannot kill it without killing the computer
         * 
         * This observes processes and makes sure that they're not doing anything that's unauthorised
         * If so, it will either prevent or kill.
         * Preventing Example - App tries to make its self unlosable (Hide 'X' button), RS (Radiance Security) will then undo that.
         * Killing Example    - When an app tries to do something and trying to get past UAC by doing it via some
         *                      different way to get around the UAC restrictions, RS will pick up on that and kill
         *                      the process.
         * 
         * Developed by MrBisquit (WTDawson)
         * */

        static int updates = 0;

        /// <summary>
        /// Updating the RS background service
        /// </summary>
        /// <param name="i">The ID of the process</param>
        public static void Update(int i)
        {
            // Render every 100 frames, prevents lag
            updates++;
            if (updates >= 250)
            {
                updates = 0;
            }
            else return;

            CheckProcesses();
        }

        /// <summary>
        /// This is is the built-in service, built-in to the Explorer process.
        /// Mainly to do checks, involving things with detecting processes attempting to kill RS Security.
        /// </summary>
        public static void UpdateInternal()
        {
            UAC.Render();

            // Render every 250 frames, prevents lag
            updates++;
            if (updates >= 250)
            {
                updates = 0;
            }
            else return;

            CheckProcesses();
        }

        public static void UpdateUAC()
        {
            UAC.Render();
        }

        static void CheckProcesses()
        {
            bool hasRSSP = false;

            for (int i = 0; i < Process.Processes.Count; i++)
            {
                Processes p = Process.Processes[i];
                // TODO: Once permissions implemented, go through and check everything's in check

                if(p.ID == 101)
                {
                    // Detect if the Security process exists.
                    hasRSSP = true;
                }
            }

            if (!hasRSSP && Session.IsAuthenticated)
            {
                Process.Processes.Clear(); // Kill all apps
                InitService();
                MessageBoxCreator.CreateMessageBox("Radiance Security - Security Alert", "Something has occurred and the Radiance Security background process has been killed,\nRadiance Security (Explorer Built-In) has detected this and has taken measures to kill all running applications to\nprevent possible malware from executing further.", MessageBoxCreator.MessageBoxIcon.STOP, 1200);
                ResetExplorer();
            }
        }

        public static void PauseAllProcesses()
        {
            for (int i = 0; i < Process.Processes.Count; i++)
            {
                Process.Processes[i].hidden = true;
            }
        }

        public static void UnPauseAllProcesses()
        {
            for (int i = 0; i < Process.Processes.Count; i++)
            {
                Process.Processes[i].hidden = false;
            }
        }

        /// <summary>
        /// Initialises RS Security, should only be called once
        /// </summary>
        public static void Initialise()
        {
            InitService();
        }

        /// <summary>
        /// This is called to start the RS Security background process
        /// </summary>
        public static void InitService()
        {
            Processes service = new Processes()
            {
                Name = "RS Security Service",
                ID = 101
            };

            Process.Processes.Add(service);
        }

        static void ResetExplorer()
        {
            Explorer.drawIcons = true;
            Explorer.DrawMenu = true;
            Explorer.DrawTaskbar = true;
        }
    }
}
