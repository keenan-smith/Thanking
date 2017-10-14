using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class VectorUtilities
    {
        public static float GetDistance(Vector3 start, Vector3 point)
        {
            Vector3 firstPosition = start;
            Vector3 heading;
            float distanceSquared;
            float distance;

            heading.x = firstPosition.x - point.x;
            heading.y = firstPosition.y - point.y;
            heading.z = firstPosition.z - point.z;

            distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
            distance = Mathf.Sqrt(distanceSquared);

            return distance;
        }
    }
}
