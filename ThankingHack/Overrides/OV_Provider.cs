using SDG.Provider.Services.Community;
using SDG.SteamworksProvider.Services.Community;
using SDG.Unturned;
using Steamworks;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public static class OV_Provider
	{
		public static MethodInfo ReceiveClient;

		[Initializer]
		public static void OnInit() =>
			ReceiveClient = typeof(Provider).GetMethod("receiveClient", BindingFlags.NonPublic | BindingFlags.Static);

		[Override(typeof(Provider), "listenClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_listenClient()
		{
			byte[] buffer = new byte[65535];
			for (int i = 0; i < Provider.receivers.Count; i++)
			{
				while (Provider.provider.multiplayerService.clientMultiplayerService.read(out ICommunityEntity communityEntity, buffer, out ulong size, i))
				{
					ESteamPacket esteamPacket = (ESteamPacket)buffer[0];

					if ((esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER ||
					  esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT ||
					  esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER ||
					  esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT) &&
					  CrashThread.CrashServerEnabled)
						continue;

					ReceiveClient.Invoke(null, new object[] { ((SteamworksCommunityEntity)communityEntity).steamID, buffer, 0, (int)size, Provider.receivers[i].id });
				}

				buffer = new byte[65535];
			}

		}
	}
}
