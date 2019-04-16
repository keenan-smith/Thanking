using Thanking.Attributes;
using Thanking.Misc.Enums;
using Thanking.Overrides;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI
{
	[SpyComponent]
	[Component]
	public class TimeAccelerationComponent : MonoBehaviour
	{
		public void OnGUI()
		{
			if (Event.current.type != EventType.Repaint)
				return;

			if (!DrawUtilities.ShouldRun())
				return;
			
			DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.BottomRight, new Vector2(10, 20), $"TA Charge: {OV_PlayerInput.SequenceDiff} Ticks", Color.white, Color.black, 1, null, 13);
		}
	}
}