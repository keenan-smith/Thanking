﻿using System;
using System.Collections;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class WeaponCoroutines
	{
		public static IEnumerator CheckForDeath(Player p)
		{
			if (WeaponOptions.OofOnDeath)
			{
				DateTime time = DateTime.Now;
				while (DateTime.Now - time < TimeSpan.FromSeconds(1.5f))
				{
					if (p.life.isDead)
					{
						AudioSource.PlayClipAtPoint(AssetVariables.Audio["oof"], Player.player.look.aim.position);
						break;
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
		}
	}
}
