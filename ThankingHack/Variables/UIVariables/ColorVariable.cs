using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Thanking.Misc;
using UnityEngine;

namespace Thanking.Options.UIVariables
{
    public class ColorVariable
    {
        public SerializableColor color;
        public SerializableColor origColor;
        public string name;
        public string identity;
        public bool disableAlpha;

        public ColorVariable()
        {

        }

        public ColorVariable(string identity, string name, Color32 color, Color32 origColor, bool disableAlpha)
        {
            this.identity = identity;
            this.name = name;
            this.color = color;
            this.origColor = origColor;
            this.disableAlpha = disableAlpha;
        }

        public ColorVariable(string identity, string name, Color32 color, bool disableAlpha = true)
        {
            this.identity = identity;
            this.name = name;
            this.color = color;
            this.origColor = color;
            this.disableAlpha = disableAlpha;
        }

        public ColorVariable(ColorVariable option)
        {
            this.identity = option.identity;
            this.name = option.name;
            this.color = option.color;
            this.origColor = option.origColor;
            this.disableAlpha = option.disableAlpha;
        }

		public static implicit operator Color(ColorVariable color) =>
			color.color.ToColor();

		public static implicit operator Color32(ColorVariable color) =>
			color.color;
	}
}
