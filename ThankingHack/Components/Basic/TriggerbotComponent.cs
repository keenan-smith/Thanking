using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[Component]
	public class TriggerbotComponent : MonoBehaviour
	{
		public void Awake() =>
			InvokeRepeating("Check", 0f, 0.15f);

		public void Check()
		{
            if (!Provider.isConnected || Provider.isLoading)
                return;

			if (TriggerbotOptions.Enabled)
			{
				RaycastInfo info = RaycastUtilities.GenerateRaycast();

				TriggerbotOptions.IsFiring = info.player != null && !TriggerbotOptions.IsFiring;
            }
            else
            {
                TriggerbotOptions.IsFiring = false;
            }
        }
	}
}
