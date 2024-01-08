using RadianceOS.System.Apps;
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
         * Killing this process will cause a crash (Apps will think that killing it will make it easier for them to do bad stuff)
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
            if (updates >= 100)
            {
                updates = 0;
            }
            else return;

            CheckProcesses();
        }

        static void CheckProcesses()
        {
            for (int i = 0; i < Process.Processes.Count; i++)
            {
                Processes p = Process.Processes[i];
                // TODO: Once permissions implemented, go through and check everything's in check
            }
        }
    }
}
