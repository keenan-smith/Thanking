using SDG.Unturned;
using Steamworks;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Managers.Main
{
	public static class PacketManager
	{
		public static void Init()
		{
			SteamChannel.onTriggerReceive += OnReceivePacket;
		}

		public static void OnReceivePacket(SteamChannel channel, CSteamID steamID, byte[] packet, int offset, int size)
		{
			if (!channel.checkServer(steamID))
				return;

			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			int num = packet[offset + 1];

			if (esteamPacket == ESteamPacket.UPDATE_VOICE)
				return;

			SteamChannelMethod method = channel.calls[num];

			string call = method.method.Name;
			switch (call)
			{
				case "tellDead":
					{
						object[] objects = SteamPacker.getObjects(steamID, offset, 2, packet, method.types);

						Vector3 Ragdoll = (Vector3)objects[1];
						if (Ragdoll == RaycastOptions.TargetRagdoll.ToVector())
							AudioSource.PlayClipAtPoint(AssetVariables.Audio["oof"], Player.player.look.aim.position);
					}
					break;
			}
		}
	}
}
