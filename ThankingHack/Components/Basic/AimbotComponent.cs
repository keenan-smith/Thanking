using System.Collections.Generic;
using Thinking.Attributes;
using Thinking.Components.UI.Menu.Tabs;
using Thinking.Coroutines;
using Thinking.Options;
using Thinking.Options.AimOptions;
using Thinking.Utilities;
using UnityEngine;

namespace Thinking.Components.Basic
{
	/// <summary>
	/// Component used to start aimbot coroutines and check hotkeys if necessary
	/// </summary>
	[Component]
	public class AimbotComponent : MonoBehaviour 
	{
		public void Start()
		{
			CoroutineComponent.LockCoroutine = StartCoroutine(AimbotCoroutines.SetLockedObject());
			CoroutineComponent.AimbotCoroutine = StartCoroutine(AimbotCoroutines.AimToObject());

			StartCoroutine(RaycastCoroutines.UpdateObjects());
		}
	}
}