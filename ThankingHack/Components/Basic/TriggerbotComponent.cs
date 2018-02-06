using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[Component]
	public class TriggerbotComponent : MonoBehaviour
	{
		public void Awake() =>
		  InvokeRepeating(nameof(Check), 0f, 0.2f);

		public void Check()
		{
			if (!DrawUtilities.ShouldRun())
				return;

			if (TriggerbotOptions.Enabled)
			{
				RaycastUtilities.GetPlayers();

				if (!RaycastUtilities.GenerateRaycast(out RaycastInfo ri, true))
				{
					TriggerbotOptions.IsFiring = false;
					return;
				}
				
				TriggerbotOptions.IsFiring = !TriggerbotOptions.IsFiring;
			}
			else
				TriggerbotOptions.IsFiring = false;
		}
	}
}