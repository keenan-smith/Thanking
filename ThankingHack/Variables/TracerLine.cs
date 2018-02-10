using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Variables
{
    public class TracerLine
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public bool Hit = false;
        public DateTime CreationTime;

        public TracerLine() =>
            CreationTime = DateTime.Now;
    }
}
