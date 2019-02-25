using Thanking.Misc.Enums;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_Physics
	{
		public static bool ForceReturnFalse = false;
		
		public static bool OV_Linecast(Vector3 start, Vector3 end, int layerMask, QueryTriggerInteraction queryTriggerInteraction) => 
			!ForceReturnFalse && (bool) OverrideUtilities.CallOriginal(null, start, end, layerMask, queryTriggerInteraction);
	}
}