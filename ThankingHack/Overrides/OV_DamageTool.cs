﻿using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public class OV_DamageTool
	{
		//[Override(typeof(DamageTool), "raycast", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)]
		public RaycastInfo OV_Raycast(Ray ray, float range, int mask) =>
			RaycastUtilities.GenerateRaycast();
	}
}
