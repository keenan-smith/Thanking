using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI
{
	[Component]
	[UIComponent]
	public class LogoComponent : MonoBehaviour
	{
		public void OnGUI()
		{
			DrawUtilities.DrawLabel(Variables.LabelLocation.BottomRight, new Vector2(10, 20), "Thanking v2.0.2 Alpha", Color.black, Color.cyan, 4);
		}
	}
}
