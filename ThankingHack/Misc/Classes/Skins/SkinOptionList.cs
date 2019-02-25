using System.Collections.Generic;
using Thanking.Misc.Enums;

namespace Thanking.Misc.Classes.Skins
{
	public class SkinOptionList
	{
		public SkinType Type = SkinType.Weapons;
		public HashSet<Skin> Skins = new HashSet<Skin>();

		public SkinOptionList(SkinType Type)
		{
			this.Type = Type;
		}
	}
}