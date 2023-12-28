using Cosmos.HAL;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Networking
{
	public static class NetworkManager
	{
		public static bool Network;
		public static void Connect()
		{
			try
			{
				NetworkDevice nic = NetworkDevice.GetDeviceByName("eth0"); //get network device by name
				IPConfig.Enable(nic, new Address(192, 168, 1, 69), new Address(255, 255, 255, 0), new Address(192, 168, 1, 254)); //enable IPv4 configuration
																																  //Kernel.WriteLineOK("Found network device! Current IP: " + NetworkConfiguration.CurrentAddress);
				using (var xClient = new Cosmos.System.Network.IPv4.UDP.DHCP.DHCPClient())
				{
					/** Send a DHCP Discover packet **/
					//This will automatically set the IP config after DHCP response
					xClient.SendDiscoverPacket();

				}
			}
			finally
			{
				Network = true;
			}
		}
	}
}
