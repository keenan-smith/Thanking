using Newtonsoft.Json;
using Thanking.Misc;
using Thanking.Misc.Serializables;
using UnityEngine;

namespace Thanking.Variables.UIVariables
{
    public class ColorVariable
    {
        public SerializableColor color;
        public SerializableColor origColor;
        public string name;
        public string identity;
        public bool disableAlpha;

        [JsonConstructor]
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
            identity = option.identity;
            name = option.name;
            color = option.color;
            origColor = option.origColor;
            disableAlpha = option.disableAlpha;
        }

		public static implicit operator Color(ColorVariable color) =>
			color.color.ToColor();

		public static implicit operator Color32(ColorVariable color) =>
			color.color;
	}
}
