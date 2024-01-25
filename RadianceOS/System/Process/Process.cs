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

        public ProcessType CurrentProcessType()
        {
            if (GetType() == typeof(Process)) return ProcessType.Process;
            if (GetType() == typeof(Service)) return ProcessType.Service;
            if (GetType() == typeof(Application)) return ProcessType.Application;
            else return default;
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
