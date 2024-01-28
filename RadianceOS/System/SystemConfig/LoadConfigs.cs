using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.SystemConfig
{
    public static class LoadConfigs
    {
        public static void LoadAll()
        {
            BootConfig.GetConfig();
        }
    }
}
