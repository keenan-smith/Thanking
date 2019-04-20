﻿using System.Collections.Generic;
using Thanking.Misc.Classes.ESP;

namespace Thanking.Variables
{
    public class ESPVariables
    {
        public static List<ESPObject> Objects = new List<ESPObject>();

        public static Queue<ESPBox> DrawBuffer = new Queue<ESPBox>();
        public static Queue<ESPBox2> DrawBuffer2 = new Queue<ESPBox2>();
    }
}
