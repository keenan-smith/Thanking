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
			{
				RaycastUtilities.GetPlayers();
				RaycastUtilities.GetClosestObject(RaycastUtilities.Objects, out double Distance, out GameObject Object, out Vector3 Pos);

				if (Object == null)
					TriggerbotOptions.IsFiring = false;

				if (!RaycastUtilities.GenerateRaycast(Object, Pos, out RaycastInfo ri))
					TriggerbotOptions.IsFiring = false;

				TriggerbotOptions.IsFiring = true;
			}
			else
				TriggerbotOptions.IsFiring = false;
		}
	}
}