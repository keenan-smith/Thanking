﻿using SDG.Unturned;
using System;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class VectorUtilities
    {
        public static double GetDistance(Vector3 start, Vector3 point)
        {
            Vector3 heading;

            heading.x = start.x - point.x;
            heading.y = start.y - point.y;
            heading.z = start.z - point.z;
            
            return Math.Sqrt(heading.x * heading.x + heading.y * heading.y + heading.z * heading.z);
        }

        public static double GetDistance(Vector3 point)
        {
            Vector3 start = MainCamera.instance.transform.position;
            Vector3 heading;

            heading.x = start.x - point.x;
            heading.y = start.y - point.y;
            heading.z = start.z - point.z;

            return Math.Sqrt(heading.x * heading.x + heading.y * heading.y + heading.z * heading.z);
        }
    }
}
