using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thanking.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ThreadAttribute : Attribute
	{
		public string StartMethod;

		public ThreadAttribute(string method) => 
			StartMethod = method;
	}
}
