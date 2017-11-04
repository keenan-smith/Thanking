using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thanking.Options.UIVariables
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
