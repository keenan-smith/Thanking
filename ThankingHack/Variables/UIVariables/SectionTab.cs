using System;

namespace Thanking.Variables.UIVariables
{
    public class SectionTab
    {
        public static SectionTab CurrentSectionTab;

        public Action code;
        public string name;

        public SectionTab(string name, Action code)
        {
            this.name = name;
            this.code = code;
        }
    }
}
