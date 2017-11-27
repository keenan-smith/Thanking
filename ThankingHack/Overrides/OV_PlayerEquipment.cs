using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public static class OV_PlayerEquipment
	{
		[Override(typeof(PlayerEquipment), "punch", BindingFlags.NonPublic | BindingFlags.Instance)]
		public static void punch(EPlayerPunch mode)
		{
			if (MiscOptions.ExtendMeleeRange)
				OV_DamageTool.OVType = OverrideType.Regular;

			if (RaycastOptions.Enabled)
				OV_DamageTool.OVType = OverrideType.SilentAim;

			if (Provider.isServer)
				OV_DamageTool.OVType = OverrideType.None;

			OverrideUtilities.CallOriginal(Player.player.equipment, mode);
			OV_DamageTool.OVType = OverrideType.None;
		}
	}
}
