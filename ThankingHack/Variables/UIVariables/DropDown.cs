using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thanking.Options.UIVariables
{
    public class DropDown
    {
        public static Dictionary<string, DropDown> DropDownManager = new Dictionary<string, DropDown>();

        public bool IsEnabled;
        public int ListIndex;
        public DropDown()
        {
            IsEnabled = false;
            ListIndex = 0;
        }

        public static DropDown Get(string identifier)
        {
            DropDown dropdown;
            if (DropDownManager.TryGetValue(identifier, out dropdown))
            {
                return dropdown;
            }
            else
            {
                dropdown = new DropDown();
                DropDownManager.Add(identifier, dropdown);
                return dropdown;
            }
        }

    }
}
