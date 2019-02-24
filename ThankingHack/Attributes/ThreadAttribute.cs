using System;

namespace Thanking.Attributes
{
	/// <summary>
	/// Attribute on a target method used to create a different thread
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class ThreadAttribute : Attribute
	{

	}
}
