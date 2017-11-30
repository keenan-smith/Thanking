using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Components.MultiAttach
{
	public class VelocityComponent : MonoBehaviour
	{
		public Vector3 Previous;
		public float Speed; //it's technically not velocity since it has no direction xdd

		public void Update()
		{
			Speed = ((transform.position - Previous).magnitude) / Time.deltaTime;
			Previous = transform.position;
		}
	}
}
