using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI
{
	[UIComponent]
	[Component]
	public class ESPComponent : MonoBehaviour
	{
		public static Material mat = null;

		public void Start()
		{
			mat = new Material(Shader.Find("Hidden/Internal-Colored")) { hideFlags = HideFlags.HideAndDontSave };
			mat.SetInt("_SrcBlend", 5);
			mat.SetInt("_DstBlend", 10);
			mat.SetInt("_Cull", 0);
			mat.SetInt("_ZWrite", 0);
		}

		public void OnGUI()	
		{
			if (Event.current.type == EventType.Repaint && ESPOptions.Enabled)
			{
				for (int i = 0; i < ESPOptions.EnabledOptions.Length; i++)
				{
					if (ESPOptions.EnabledOptions[i])
					{
						switch (i)
						{
							case 0:
								DrawPlayers();
								break;
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
								break;
						}
					}
				}
			}
		}

		public void DrawItems()
		{
			InteractableItem[] items = FindObjectsOfType<InteractableItem>();
			for (int i = 0; i < items.Length; i++)
				DrawUtilities.DrawTransform(items[i].transform, mat);
		}

		public void DrawPlayers()
		{
			SteamPlayer[] players = Provider.clients.ToArray();
			for (int index = 0; index < players.Length; index++)
			{
				SteamPlayer plr = players[index];

				if (plr == null || plr.player == Player.player || plr.player.life.isDead ||
					plr.player.transform == null) continue;

				DrawUtilities.DrawTransform(plr.player.transform, mat);
			}
		}
	}
}
