using System;

namespace Thanking.Attributes
{
	/// <summary>
	/// Attribute that calls the target method after a spy is executed
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class OffSpyAttribute : Attribute
	{
	}
}