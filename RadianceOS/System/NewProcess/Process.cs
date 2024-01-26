using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Process
{
    public class Process
    {
        public string ApplicationName { get; private set; } = null;
        public string Title { get; set; } = null;

        public Action Update = null;
        public Security.ProcessSecurityInfo SecurityInfo { get; private set; }

        public bool IsResponding { get; private set; } // If the time it took to run is longer than 5 seconds, it sets it as not and the user will get a popup while its frozen.
        public DateTime LastResponse {  get; private set; }

        public bool Suspended { get; private set; } = false;

        public string OwnedBy { get; private set; } = default; // null = SYSTEM owned, default = not set
        public ProcessHandle ProcessHandle { get; private set; } = default;

        public ProcessType CurrentProcessType()
        {
            if (GetType() == typeof(Process)) return ProcessType.Process;
            if (GetType() == typeof(Service)) return ProcessType.Service;
            if (GetType() == typeof(Application)) return ProcessType.Application;
            else return default;
        }

        public Type GetProcessType() => GetType();
        public bool CheckType(Type match) => GetType() == match;

        public void Suspend()
        {
            // May need to add security checks in future
            Suspended = true;
        }

        public void Unsuspend()
        {
            // May need to add security checks in future
            Suspended = false;
        }
    }

    public class Service : Process
    {

    }

    public class Application : Process
    {

    }

    public enum ProcessType
    {
        Process = 0,
        Service = 1,
        Application = 2
    }
}
