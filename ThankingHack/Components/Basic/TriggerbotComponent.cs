using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Options.BotOptions;
using Thanking.Options.SilentAimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[Component]
	public class TriggerbotComponent : MonoBehaviour
	{
		public void Awake() =>
			InvokeRepeating("Check", 0f, 0.15f);

		public void Update()
		{
			if (Input.GetKeyDown(TriggerbotOptions.Toggle))
			{
				TriggerbotOptions.Enabled = !TriggerbotOptions.Enabled;

				if (!TriggerbotOptions.Enabled)
					TriggerbotOptions.IsFiring = false;
			}
		}
		
		public void Check()
		{
			if (TriggerbotOptions.Enabled)
			{
				RaycastInfo info = RaycastUtilities.GenerateRaycast();
				if (info.point != Vector3.zero && !TriggerbotOptions.IsFiring)
					TriggerbotOptions.IsFiring = true;
				else
					TriggerbotOptions.IsFiring = false;
			}
		}
	}
}
