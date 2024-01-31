using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security
{
    public static class SecurityCLI
    {
        /// <summary>
        /// This makes it very easy to transfer it across the Terminal and different things.
        /// </summary>
        /// <param name="commands">The command arguments (All of them including the first one)</param>
        /// <param name="WriteString">To run something like this: `Apps.Process.Processes[index].lines.Add(empty);`</param>
        public static void RunCommand(string[] commands, Action<string> WriteString, Action<TextColor> WriteColouredString)
        {
            switch (commands[0].ToLower())
            {
                default:
                    {
                        WriteString("SecurityCLI");
                        WriteString("Run `security -h` to show all of the commands.");
                    }
                    break;
                case "uac":
                    {
                        RunUACCommand(commands, WriteString, WriteColouredString);
                    }
                    break;
            }
        }

        private static void RunUACCommand(string[] commands, Action<string> WriteString, Action<TextColor> WriteColouredString)
        {
            if(commands.Length == 0)
            {
                WriteString("SecurityCLI - UAC Commands");
                WriteString("No arguments were given.");
                WriteString("Run `uac -h` to show all of the commands.");
            } else if (commands[1] == "-h")
            {
                WriteString("SecurityCLI - UAC Command Help");
                WriteString("Base arguments:");
                WriteString("- --user-e: Currently active user elevation");
            } else if (commands[1] == "--user-e")
            {
                WriteString("Requesting elevation...");
                UAC.UserElevation userElevation = new UAC.UserElevation(Auth.Session.CurrentUserLevel + 1, (UAC.UACResult result) =>
                {
                    WriteString("Elevation completed with a status of: " + (result.Success == true ? "Completed successfully" : "Failed"));
                });
                WriteString("Elevation request complete");
            }
        }
    }
}
