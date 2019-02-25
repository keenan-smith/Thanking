using System;

namespace Thanking.Attributes
{
    /// <summary>
    /// Attribute that calls the target method before a spy is executed
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OnSpyAttribute : Attribute
    {
    }
}
