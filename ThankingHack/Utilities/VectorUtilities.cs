using System;
using SDG.Unturned;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class VectorUtilities
    {
        public static double GetDistance(Vector3 point) =>
            GetDistance(MainCamera.instance.transform.position, point);
        
        public static double GetDistance(Vector3 start, Vector3 point)
        {
            Vector3 heading;

            heading.x = start.x - point.x;
            heading.y = start.y - point.y;
            heading.z = start.z - point.z;
            
            return Math.Sqrt(heading.x * heading.x + heading.y * heading.y + heading.z * heading.z);
        }

        public static double GetMagnitude(Vector3 vector) =>
            Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        
        public static Vector3 Normalize(Vector3 vector) =>
            vector / (float)GetMagnitude(vector);
    }
}
