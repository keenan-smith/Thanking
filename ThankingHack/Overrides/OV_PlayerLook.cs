using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;

namespace Thanking.Overrides
{
    public class OV_PlayerLook
    {
        [Override(typeof(PlayerLook), "onDamaged", BindingFlags.NonPublic | BindingFlags.Instance)]
        public static void OV_onDamaged(byte damage)
        {
            if (MiscOptions.NoFlinch)
                return;
            else
                OverrideUtilities.CallOriginal(null, damage);
        }
    }
}
