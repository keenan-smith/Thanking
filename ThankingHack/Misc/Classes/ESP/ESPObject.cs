using Thanking.Misc.Enums;
using UnityEngine;

namespace Thanking.Misc.Classes.ESP
{
	public class ESPObject
	{
		public ESPTarget Target;
		public object Object;
		public GameObject GObject;

		public ESPObject(ESPTarget t, object o, GameObject go)
		{
			Target = t;
			Object = o;
			GObject = go;
		}
	}
}