using System; 
using UnityEngine;

namespace Thinking.Variables
{
    public class TracerLine
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public bool Hit = false;
        public DateTime CreationTime;
    }
}