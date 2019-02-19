using System;

namespace Thinking.Attributes
{
	/// <summary>
	/// Attribute on a method, invoked on startup
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class InitializerAttribute : Attribute
	{

	}
}