using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thinking.Attributes;
using Thinking.Components.UI;
using Thinking.Options;
using Thinking.Utilities;
using Thinking.Variables;
using UnityEngine;

namespace Thinking.Overrides
{
    public class OV_PlayerInput
    {
	    public static bool Run;
	    
	    public FieldInfo ClientsidePacketsField =
		    typeof(PlayerInput).GetField("clientsidePackets", BindingFlags.NonPublic | BindingFlags.Instance);

	    public List<PlayerInputPacket> ClientsidePackets {
		    get {
			    if (!DrawUtilities.ShouldRun() || !Run)
				    return null;
			    
			   return (List<PlayerInputPacket>)ClientsidePacketsField.GetValue(OptimizationVariables.MainPlayer.input);
		    }
	    }
	    
	    public PlayerInputPacket LastPacket;
	   // [Override(typeof(PlayerInput), "FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public void OV_FixedUpdate() 
	    {
		    if (LastPacket != null && MiscOptions.PunchAura)
		    {
			    EPlayerStance stance = OptimizationVariables.MainPlayer.stance.stance;

			    if (stance != EPlayerStance.SWIM &&
			        stance != EPlayerStance.CLIMB &&
			        stance != EPlayerStance.DRIVING &&
			        stance != EPlayerStance.PRONE &&
			        stance != EPlayerStance.SITTING) {

				    for (int i = 0; i < 5; i++) {
					    OptimizationVariables.MainPlayer.input.keys[1] =
						    !OptimizationVariables.MainPlayer.input.keys[1];
					    
					    OverrideUtilities.CallOriginal();
				    }
			    }
		    }
		    OverrideUtilities.CallOriginal();
		    LastPacket = ClientsidePackets?.Last();
	    }
    }
}