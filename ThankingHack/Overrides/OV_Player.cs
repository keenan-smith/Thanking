using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Coroutines;
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
	}
}