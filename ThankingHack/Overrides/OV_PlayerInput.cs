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
	    public static int RealSequence = -1;
	    public static int FakeSequence = -1;
	    
	    public static bool Run;
	    public static bool First = true;
	    
	    public static FieldInfo SequenceField = 
		    typeof(PlayerInput).GetField("sequence", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static FieldInfo ClientsidePacketsField =
		    typeof(PlayerInput).GetField("clientsidePackets", BindingFlags.NonPublic | BindingFlags.Instance);

	    public static int GetSequence(PlayerInput instance) =>
		    (int)SequenceField.GetValue(instance);
	    
	    public static void SetSequence(PlayerInput instance, int value) =>
		    SequenceField.SetValue(instance, value);
	    
	    public static List<PlayerInputPacket> CSPackets(PlayerInput instance) =>
			(List<PlayerInputPacket>)ClientsidePacketsField.GetValue(instance);
	    
	    public PlayerInputPacket LastPacket;
	    [Override(typeof(PlayerInput), "FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public static void OV_FixedUpdate(PlayerInput instance) 	
	    {
		    if (instance.player != OptimizationVariables.MainPlayer)
		    {
			    OverrideUtilities.CallOriginal(instance);
			    return;
		    }
		    
		    if (MiscOptions.PunchAura)
		    {
			    if (First)
			    {
				    RealSequence = GetSequence(instance) + 1;
				    FakeSequence = RealSequence;
				    
				    First = false;
			    }

			    SetSequence(instance, FakeSequence);

			    OverrideUtilities.CallOriginal(instance);

			    if (GetSequence(instance) != FakeSequence) // Sequence was incremented.
				    RealSequence++;
	
			    if (RealSequence - FakeSequence > 25)
			    {
				    First = true;
				    SetSequence(instance, RealSequence + 3);
				    
				    instance.channel.openWrite();
				    instance.channel.write((byte)25); // 25 packets
				    
				    Ray         ray         = new Ray(instance.player.look.aim.position, instance.player.look.aim.forward);
				    RaycastInfo raycastInfo = DamageTool.raycast(ray, 1.75f, RayMasks.DAMAGE_CLIENT);
				    
				    for (int i = 0; i < 25; i++)
				    {
					    PlayerInputPacket lastPacket = CSPackets(instance).Last();

					    lastPacket.sequence = FakeSequence + i + 2;

					    if (i % 7 == 0)
					    {
						    lastPacket.clientsideInputs = new List<RaycastInfo>();
						    
						    lastPacket.clientsideInputs.Add(raycastInfo);
						    lastPacket.keys |= 1 << 1;
					    }
					    
					    else if (i % 7 == 1)
						    lastPacket.keys = (ushort) (lastPacket.keys & ~(1 << 1));
					    
					    instance.channel.write((byte)0); // walking player input packet
					    lastPacket.write(instance.channel);

					    if (i % 7 == 0)
							lastPacket.clientsideInputs.Remove(raycastInfo);
				    }
				    
				    instance.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
			    }
			}

		    else
		    {
			    First = true;
			    OverrideUtilities.CallOriginal(instance);
		    }
	    }
    }
}