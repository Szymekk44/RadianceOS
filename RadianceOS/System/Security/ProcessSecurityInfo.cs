using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security
{
    public class ProcessSecurityInfo
    {
        public SecurityLevel Level { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Publisher { get; private set; }
        public bool Verified { get; private set; }
        public enum SecurityLevel
        {
            Basic = 0,            // Normal stuff
            SemiRestricted = 1,   // Normal stuff + Disable closing, Change settings, etc...
            Unrestricted = 2,     // Has access to a lot of things
            Administrator = 3,    // Access to everything (User level)
            Root = 4,             // Access to everything (User + Kernel), rare to get
        }

        public ProcessSecurityInfo(string name, string description, string publisher, bool verified)
        {
            Name = name;
            Description = description;
            Publisher = publisher;
            Verified = verified; // Probably not a good idea but oh well
        }

        public void AttemptElevation()
        {

        }
    }
}
