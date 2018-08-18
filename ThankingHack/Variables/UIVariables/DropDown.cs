using System.Collections.Generic;

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
                return dropdown;
            
            dropdown = new DropDown();
            DropDownManager.Add(identifier, dropdown);
            return dropdown;
        }

    }
}
