﻿using System.Reflection;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Options;
using Thinking.Utilities;

namespace Thinking.Overrides
{
    public static class OV_PlayerLifeUI
    {
        public static bool WasCompassEnabled;
        
        [Override(typeof(PlayerLifeUI), "hasCompassInInventory", BindingFlags.NonPublic | BindingFlags.Static)]
        public static bool OV_hasCompassInInventory()
        {
            if (MiscOptions.Compass)
                return true;

            return (bool) OverrideUtilities.CallOriginal();
        }

        [OnSpy]
        public static void Disable()
        {
            if (!DrawUtilities.ShouldRun())
                return;
            
            WasCompassEnabled = MiscOptions.Compass;
            MiscOptions.Compass = false;
		    
            PlayerLifeUI.updateCompass();
        }
        
        [OffSpy]
        public static void Enable()
        { 
            if (!DrawUtilities.ShouldRun())
                return;
		    
            MiscOptions.Compass = WasCompassEnabled;
            PlayerLifeUI.updateCompass();
        }
    }
}