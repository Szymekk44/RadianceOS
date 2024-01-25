using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Process
{
    public static class ProcessManager
    {
        public static List<Process> Processes { get; private set; } = new List<Process>();

        public static void Update()
        {
            foreach (Process process in Processes)
            {
                if (process.Update != null) process.Update();
                else throw new InvalidProcessException(InvalidProcessException.Type.InvalidUpdateAction);
            }
        }

        public static void RegisterProcess(Process process)
        {
            if(process.CurrentProcessType() == ProcessType.Process)
            {
                throw new InvalidProcessException(InvalidProcessException.Type.InvalidProcessType);
            }
        }

        public class InvalidProcessException : Exception
        {
            public enum Type
            {
                InvalidProcessType = 0,
                InvalidUpdateAction = 1,
            }

            public Type ExceptionType;

            public InvalidProcessException(Type ExceptionType)
            {
                this.ExceptionType = ExceptionType;
            }
        }
    }
}
