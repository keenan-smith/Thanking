using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
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
			if (LoaderCoroutines.IsLoaded)
            {
                //DebugUtilities.Log("Called!");
                //GUI.Label(new Rect(20, 40, 100, 50), "Thanking v2.1.2 Alpha");
                DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.TopRight, new Vector2(20, 40),
					"Thanking v2.1.3 Alpha", Color.black, Color.cyan, 0);
				if (Provider.isConnected && !Provider.isLoading)
					DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.TopRight, new Vector2(20, 60),
						$"Movement Checked: {!PlayerMovement.forceTrustClient}", Color.black, Color.red, 0);
			}
		}
	}
}
