using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
	public class OV_Player : MonoBehaviour
	{
		[Override(typeof(Player), "askScreenshot", BindingFlags.Public | BindingFlags.Instance)]
		public void OV_askScreenshot(CSteamID steamid)
		{
			if (OptimizationVariables.MainPlayer.channel.checkServer(steamid))
				StartCoroutine(PlayerCoroutines.TakeScreenshot());
		}

        [Override(typeof(Player), "tellStat", BindingFlags.Public | BindingFlags.Instance)]
        public void OV_tellStat(CSteamID steamID, byte newStat)
        {
            if (OptimizationVariables.MainPlayer.channel.checkServer(steamID) && (EPlayerStat)newStat == EPlayerStat.KILLS_PLAYERS)
            {
                if (WeaponOptions.OofOnDeath)
                    OptimizationVariables.MainPlayer.GetComponentInChildren<AudioSource>().PlayOneShot(AssetVariables.Audio["oof"], 3);

                if (MiscOptions.MessageOnKill)
                    ChatManager.instance.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                    (byte)EChatMode.GLOBAL, MiscOptions.KillMessage);
            }

            OverrideUtilities.CallOriginal(instance: OptimizationVariables.MainPlayer, steamID, newStat);
        }
    }
}