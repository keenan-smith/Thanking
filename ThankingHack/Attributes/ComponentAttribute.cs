using System;

namespace Thinking.Attributes
{
    /// <summary>
    /// Attribute that attaches the target component to the loader hook object on startup
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
    }
}
