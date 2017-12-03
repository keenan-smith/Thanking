using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using Thanking.Options;

namespace Thanking.Overrides
{
	public class OV_UseableMelee
	{
		[Override(typeof(UseableMelee), "fire", BindingFlags.NonPublic | BindingFlags.Instance)]
		public void OV_fire()
		{
			if (MiscOptions.ExtendMeleeRange)
				OV_DamageTool.OVType = OverrideType.Regular;

			if (RaycastOptions.Enabled)
				OV_DamageTool.OVType = OverrideType.SilentAim;

			if (Provider.isServer)
				OV_DamageTool.OVType = OverrideType.None;

			OverrideUtilities.CallOriginal(Player.player.equipment.asset);
			OV_DamageTool.OVType = OverrideType.None;
		}
	}
}