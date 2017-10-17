using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Overrides
{
	public class OV_Player : MonoBehaviour
	{
		[Override(typeof(Player), "askScreenshot", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)]
		public void OV_askScreenshot(CSteamID steamid)
		{
			if (Player.player.channel.checkServer(steamid))
			{
				StartCoroutine(PlayerCoroutines.TakeScreenshot());
			}
		}
	}
}
