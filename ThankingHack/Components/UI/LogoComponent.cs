using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;
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
			{
				DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.BottomLeft, new Vector2(20, 40),
					"Thanking v2.1.1 Alpha", Color.black, Color.cyan, 3);
				if (Provider.isConnected && !Provider.isLoading)
					DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.BottomLeft, new Vector2(20, 60),
						$"Movement Checked: {!PlayerMovement.forceTrustClient}", Color.black, Color.red, 3);
			}
		}
	}
}
