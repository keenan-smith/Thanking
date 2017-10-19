using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Options.VisualOptions;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[Component]
	public class InputComponent : MonoBehaviour
	{
		public void Update()
		{
			if (Input.GetKeyDown(TriggerbotOptions.Toggle))
			{
				TriggerbotOptions.Enabled = !TriggerbotOptions.Enabled;

				if (!TriggerbotOptions.Enabled)
					TriggerbotOptions.IsFiring = false;
			}

			if (Input.GetKeyDown(ESPOptions.Toggle))
				ESPOptions.Enabled = !ESPOptions.Enabled;

			if (Input.GetKeyDown(SphereOptions.Toggle))
				SphereOptions.Enabled = !SphereOptions.Enabled;
		}
	}
}
