using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.SystemConfig
{
    public static class BootConfig
    {
        public static bool RecoverOnLaunch = false;

        public static void GetConfig()
        {
            string[] configs = FetchConfigs();

            for (int i = 0; i < configs.Length; i++)
            {
                if(configs[i] == null)
                {
                    switch (i)
                    {
                        case 0:
                            RecoverOnLaunch = true; // We want to boot into recovery because something has clearly gone wrong.
                            break;
                    }
                } else
                {
                    var _json = Json.JsonTextReader.Create(configs[i]);
                    _json.Read();
                    var json = _json.Value;

                    switch (i)
                    {
                        case 0:
                            BootCfg cfg = (BootCfg)json;

                            RecoverOnLaunch = cfg.RecoverOnLaunch;

                            break;
                    }
                }
            }
        }

        private class BootCfg
        {
            public bool RecoverOnLaunch = false;
        }

        private static string[] FetchConfigs()
        {
            string[] configs = new string[1];

            if (!Directory.Exists(@"0:\RadianceOS\System\Config\"))
            {
                Directory.CreateDirectory(@"0:\RadianceOS\System\Config\");
            }

            try
            {
                if (!File.Exists(@"0:\RadianceOS\System\Config\boot.cfg"))
                {
                    configs[0] = File.ReadAllText(@"0:\RadianceOS\System\Config\boot.cfg");
                }
                else
                {
                    configs[0] = null;
                }
            } catch
            {
                configs[0] = null;
            }

            return configs;
        }

        public static void SetConfig()
        {

        }
    }
}
