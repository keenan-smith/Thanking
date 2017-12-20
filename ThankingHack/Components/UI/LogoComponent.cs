﻿using System.Reflection;
using JetBrains.Annotations;
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
			if (!LoaderCoroutines.IsLoaded || !MiscOptions.LogoEnabled) 
				return;
			
			DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.TopRight, new Vector2(20, 40),
				$"Thanking v{Assembly.GetExecutingAssembly().GetName().Version} Alpha", Color.black, Color.cyan, 0);
			if (Provider.isConnected && !Provider.isLoading)
				DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.TopRight, new Vector2(20, 60),
					$"Movement Checked: {!PlayerMovement.forceTrustClient}", Color.black, Color.red, 0);
		}
	}
}
