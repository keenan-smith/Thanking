using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Thanking.Misc.Serializables
{
    public class SerializableRect
    {
        public float x, y, width, height;

        public static implicit operator Rect(SerializableRect rect) => new Rect(rect.x, rect.y, rect.width, rect.height);
        public static implicit operator SerializableRect(Rect rect) => new SerializableRect { x = rect.x, y = rect.y, width = rect.width, height = rect.height };
    }
}
