using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Components.UI.Menu.Tabs;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
	/// <summary>
	/// Component used to start aimbot coroutines and check hotkeys if necessary
	/// </summary>
	[Component]
	public class AimbotComponent : MonoBehaviour 
	{
		Dictionary<string, KeyCode> keys = HotkeyOptions.HotkeyDict;
		public void Start()
		{
			CoroutineComponent.LockCoroutine = StartCoroutine(AimbotCoroutines.SetLockedObject());
			CoroutineComponent.AimbotCoroutine = StartCoroutine(AimbotCoroutines.AimToObject());

			StartCoroutine(RaycastCoroutines.UpdateObjects());
		}

		public void Update()
		{
			if (!HotkeyTab.IsInitialized)
				return;
			
			if (Input.GetKeyDown(keys["_ToggleAimbot"]))
				AimbotOptions.Enabled = !AimbotOptions.Enabled;

			if (Input.GetKeyDown(keys["_AimbotOnKey"]))
				AimbotOptions.OnKey = !AimbotOptions.OnKey;
		}
	}
}