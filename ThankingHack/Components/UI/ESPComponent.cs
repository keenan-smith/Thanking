using SDG.Unturned;
using Thanking.Attributes;
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
			if (Event.current.type == EventType.Repaint)
			{
				SteamPlayer[] players = Provider.clients.ToArray();
				for (int index = 0; index < players.Length; index++)
				{
					SteamPlayer plr = players[index];

					if (plr == null || plr.player == Player.player || plr.player.life.isDead ||
						plr.player.transform == null) continue;

					Bounds bounds = new Bounds(plr.player.transform.position + new Vector3(0, 1.1f, 0),
						   plr.player.transform.localScale + new Vector3(0, .95f, 0));

					DrawUtilities.DrawOutline(bounds, mat);
				}
			}
		}
	}
}
