using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			if (TriggerbotOptions.Enabled)
			{
				RaycastInfo info = RaycastUtilities.GenerateRaycast();

				TriggerbotOptions.IsFiring = info.point != Vector3.zero && !TriggerbotOptions.IsFiring;
			}
		}
	}
}
