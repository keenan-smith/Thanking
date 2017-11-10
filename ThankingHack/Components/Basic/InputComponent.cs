using Thanking.Attributes;
using Thanking.Managers.Main;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Options.VisualOptions;
using Thanking.Threads;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[SpyComponent]
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

			if (Input.GetKeyDown(MiscOptions.LogoToggle))
				MiscOptions.LogoEnabled = !MiscOptions.LogoEnabled;

			if (Input.GetKeyDown(MiscOptions.ReloadConfig))
				ConfigManager.Init();

			if (Input.GetKeyDown(MiscOptions.SaveConfig))
				ConfigManager.SaveConfig(ConfigManager.CollectConfig());

			if (Input.GetKeyDown(MiscOptions.CrashServer))
				CrashThread.CrashServerEnabled = !CrashThread.CrashServerEnabled;
		}
	}
}
