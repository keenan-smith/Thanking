using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Thanking.Variables
{
    public static class BFlags
    {
        public static BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static;
        public static BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
        public static BindingFlags PrivateStatic = BindingFlags.NonPublic | BindingFlags.Static;
        public static BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
    }
}
