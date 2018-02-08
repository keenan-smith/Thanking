using System.Linq;
using SDG.Unturned;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class LocationUtilities
    {
        public static LocationNode GetClosestLocation(Vector3 pos)
        {
            double Distance = 1337420;
            LocationNode closestNode = null;
            LocationNode[] LNodes = LevelNodes.nodes.Where(n => n.type == ENodeType.LOCATION)
                .Select(n => (LocationNode) n).ToArray();
            for (int i = 0; i < LNodes.Length; i++)
            {
                LocationNode node = LNodes[i];
                double nDist = VectorUtilities.GetDistance(pos, node.point);
                if (nDist < Distance)
                {
                    Distance = nDist;
                    closestNode = node;
                }
            }

            return closestNode;
        }        
    }
}