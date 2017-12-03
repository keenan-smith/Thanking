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
		  InvokeRepeating(nameof(Check), 0f, 0.15f);

		public void Check()
		{
			if (!DrawUtilities.ShouldRun())
				return;

			if (TriggerbotOptions.Enabled)
				TriggerbotOptions.IsFiring = RaycastUtilities.GenerateRaycast(out RaycastInfo info) && !TriggerbotOptions.IsFiring;
			else
				TriggerbotOptions.IsFiring = false;
		}
	}
}