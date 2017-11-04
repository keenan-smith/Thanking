using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Thanking.Misc;
using UnityEngine;

namespace Thanking.Options.UIVariables
{
    [JsonObject]
    public class ColorOption
    {
        [JsonProperty]
        public SerializableColor color;
        [JsonProperty]
        public SerializableColor origColor;
        [JsonProperty]
        public string name;
        [JsonProperty]
        public string identity;
        [JsonProperty]
        public bool disableAlpha;

        static ColorOption()
        {
            initDictionary();
        }

        public ColorOption(string identity, string name, Color32 color, Color32 origColor, bool disableAlpha)
        {
            this.identity = identity;
            this.name = name;
            this.color = color;
            this.origColor = origColor;
            this.disableAlpha = disableAlpha;
        }

        public ColorOption(string identity, string name, Color32 color, bool disableAlpha = true)
        {
            this.identity = identity;
            this.name = name;
            this.color = color;
            this.origColor = color;
            this.disableAlpha = disableAlpha;
        }

        public ColorOption(ColorOption option)
        {
            this.identity = option.identity;
            this.name = option.name;
            this.color = option.color;
            this.origColor = option.origColor;
            this.disableAlpha = option.disableAlpha;
        }
        
        public static Dictionary<string, ColorOption> ColorDict;
        public static List<string> keys;
        public static ColorOption errorColor;
        public static string selectedOption;
        public static ColorOption preview;
        public static ColorOption previewselected;

        public static implicit operator Color(ColorOption color)
        {
            return color.color.ToColor();
        }

        public static implicit operator Color32(ColorOption color)
        {
            return color.color;
        }

        public static void initDictionary()
        {
            ColorDict = new Dictionary<string, ColorOption>();
            keys = new List<string>();

            errorColor = new ColorOption("errorColor", "#ERROR.NOTFOUND", Color.magenta);
            preview = new ColorOption("preview", "No Color Selected", Color.white);
        }

        public static void addColor(ColorOption coloroption)
        {
            ColorDict.Add(coloroption.identity, coloroption);
            keys.Add(coloroption.identity);
        }

        public static ColorOption getColor(string identifier)
        {
            ColorOption toret;
            if (ColorDict.TryGetValue(identifier, out toret))
                return toret;
            else
                return errorColor;
        }

        public static string getHex(string identifier)
        {
            ColorOption toret;
            if (ColorDict.TryGetValue(identifier, out toret))
                return ColorToHex(toret);
            else
                return ColorToHex(errorColor);
        }

        public static void setColor(string identifier, Color32 color)
        {
            ColorOption co;
            if (ColorDict.TryGetValue(identifier, out co))
            {
                co.color = color.ToSerializableColor();
            }
        }

        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + "ff";
            return hex;
        }

        public static ColorOption[] getArray()
        {
            return ColorDict.Values.ToList().ToArray();
        }

        public static Color32 HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }
    }
}
