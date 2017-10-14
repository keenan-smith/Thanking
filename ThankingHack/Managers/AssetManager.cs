using UnityEngine;

namespace Thanking.Managers
{
	public static class AssetManager
	{
		public static void Init() =>
			AssetBundle.LoadFromFile(Application.dataPath + "/ThankingAssets.unity3d");
	}
}
