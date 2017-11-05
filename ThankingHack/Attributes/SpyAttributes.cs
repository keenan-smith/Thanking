using System;

namespace Thanking.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OnSpyAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class OffSpyAttribute : Attribute
    {
    }
}
