using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thinking.Attributes;
using Thinking.Coroutines;
using Thinking.Variables;
using UnityEngine;

namespace Thinking.Overrides
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