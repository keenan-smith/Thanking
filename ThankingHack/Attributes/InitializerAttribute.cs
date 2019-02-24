using System;

namespace Thanking.Attributes
{
	/// <summary>
	/// Attribute on a method, invoked on startup
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class InitializerAttribute : Attribute
	{

	}
}