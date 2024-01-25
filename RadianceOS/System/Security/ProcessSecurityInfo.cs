using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security
{
    public class ProcessSecurityInfo
    {
        /**
         * Built-in apps don't actually have to use this as they don't require permissions because they already have
         * access to everything they could need.
         * */

        public ProcessSecurityLevel Level { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Publisher { get; private set; }
        public bool Verified { get; private set; }
        

        public ProcessSecurityInfo(string name, string description, string publisher, bool verified)
        {
            Name = name;
            Description = description;
            Publisher = publisher;
            Verified = verified; // Probably not a good idea but oh well
        }

        public void AttemptElevation(Processes process, Action<UAC.UACResult?>? success = null, Action<UAC.UACResult?>? failure = null)
        {
            if(Level + 1 >= ProcessSecurityLevel.Root)
            {
                if(failure != null) failure(null);
            }

            UAC.ApplicationElevation applicationElevation = new UAC.ApplicationElevation(process, Level + 1);
            applicationElevation.RequestComplete = (UAC.UACResult result) =>
            {
                if (success != null && result.Success) success(result);
                if (failure != null && !result.Success) failure(result);
            };
            UAC.RequestUAC(applicationElevation);
        }
    }

    public enum ProcessSecurityLevel
    {
        Basic = 0,            // Normal stuff
        SemiRestricted = 1,   // Normal stuff + Disable closing, Change settings, etc...
        Unrestricted = 2,     // Has access to a lot of things
        Administrator = 3,    // Access to everything (User level)
        Root = 4,             // Access to everything (User + Kernel), rare to get
    }
}
