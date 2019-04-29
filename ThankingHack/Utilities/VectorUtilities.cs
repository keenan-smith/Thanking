using System;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class VectorUtilities
    {
        public static double GetDistance(Vector3 point) =>
            Vector3.Distance(OptimizationVariables.MainPlayer.look.aim.position, point);
        
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

        public static double GetAngleDelta(Vector3 mainPos, Vector3 forward, Vector3 target)
        {
            Vector3 normalized = Normalize(target - mainPos);
            return Math.Atan2(GetMagnitude(Vector3.Cross(normalized, forward)), Vector3.Dot(normalized, forward)) * (180 / Math.PI);
        }
    }
}
