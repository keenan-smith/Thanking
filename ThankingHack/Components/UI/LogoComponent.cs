using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI
{
	[Component]
	[SpyComponent]
	public class LogoComponent : MonoBehaviour
	{
		public void OnGUI()
		{
			if (MiscOptions.LogoEnabled)
				DrawUtilities.DrawLabel(ESPComponent.ESPFont, Variables.LabelLocation.BottomRight, new Vector2(20, 40), "Thanking v2.0.5 Alpha", Color.black, Color.cyan, 3);
		}
	}
}
