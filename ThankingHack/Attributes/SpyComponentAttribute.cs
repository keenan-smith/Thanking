using System;

namespace Thanking.Attributes
{
	/// <summary>
	/// Attribute on a component that destroys a component on spy and attach is it again afterwards
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class SpyComponentAttribute : Attribute
	{
	}
}
