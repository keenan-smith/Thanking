using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thanking.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OverrideAttribute : Attribute
    {
        public OverrideAttribute(Type t) =>
            OverrideClass = t;

        public Type OverrideClass
        {
            get;
            set;
        }
    }
}
