﻿using System;
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
	    public static int Step = -1;

	    public static int ClientSequence = -1;
	    public static int SequenceDiff = 0;

	    public static float LastReal;

	    public static bool Run;
	    public static bool P;
	    
	    public static FieldInfo SequenceField = 
		    typeof(PlayerInput).GetField("sequence", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static FieldInfo SimField = 
		    typeof(PlayerInput).GetField("_simulation", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static FieldInfo ClientsidePacketsField =
		    typeof(PlayerInput).GetField("clientsidePackets", BindingFlags.NonPublic | BindingFlags.Instance);

	    public static int GetSequence(PlayerInput instance) =>
		    (int)SequenceField.GetValue(instance);
	    
	    public static void SetSequence(PlayerInput instance, int value) =>
		    SequenceField.SetValue(instance, value);
	    
	    public static uint GetSim(PlayerInput instance) =>
		    (uint)SimField.GetValue(instance);
	    
	    public static void SetSim(PlayerInput instance, uint value) =>
		    SimField.SetValue(instance, value); 
	    
	    public static List<PlayerInputPacket> CSPackets(PlayerInput instance) =>
			(List<PlayerInputPacket>)ClientsidePacketsField.GetValue(instance);

	    private static Vector3 lastSentPositon = Vector3.zero;

	    [Override(typeof(PlayerMovement), "tellRecov", BindingFlags.Public | BindingFlags.Instance)]
	    public static void OV_tellRecov(PlayerMovement instance, CSteamID steamID, Vector3 newPosition, int newRecov)
	    {
		    if (P)
		    {
			    instance.player.input.recov = newRecov;
			    return;
		    }

		    OverrideUtilities.CallOriginal(instance, steamID, newPosition, newRecov);
	    }

	    [Override(typeof(PlayerInput), "FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public static void OV_FixedUpdate(PlayerInput instance) 	
	    {
		    if (instance.player != OptimizationVariables.MainPlayer)
			    return;

		    if (!P)
		    {
			    bool changed = false;
		     
			    int orig = GetSequence(instance);
			    OverrideUtilities.CallOriginal(instance);
			    if (GetSequence(instance) != orig) // Sequence was incremented.
				    changed = true;

			    if (changed)
			    {
				    lastSentPositon = instance.transform.localPosition;			    
			    
				    if (Run)
				    {
					    if (Time.realtimeSinceStartup - LastReal > 8)
					    {
						    LastReal = Time.realtimeSinceStartup;
				    
						    SequenceDiff--;
						    ClientSequence++;
					    }
			    
					    SetSequence(instance, ClientSequence);
					    SequenceDiff++;
				    }
		    
				    else
					    ClientSequence = orig + 1;
			    }
		    }

		    if (Step == 0 && !Run)
		    {
			    Run = true;
			    /*
			    if (RealSequence - FakeSequence > 25)
			    {
				    First = true;
				    SetSequence(instance, RealSequence + 3);
				    
				    instance.channel.openWrite();
				    instance.channel.write((byte)25); // 25 packets
				    
				    Ray         ray         = new Ray(instance.player.look.aim.position, instance.player.look.aim.forward);
				    RaycastInfo raycastInfo = DamageTool.raycast(ray, 6f, RayMasks.DAMAGE_CLIENT);
				    
				    for (int i = 0; i < 25; i++)
				    {
					    PlayerInputPacket lastPacket = CSPackets(instance).Last();

					    lastPacket.sequence = FakeSequence + i + 2;

					    if (i % 7 == 0)
					    {
						    lastPacket.clientsideInputs = new List<RaycastInfo> {raycastInfo};
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
				    */
			}
		    
		    else if (Step == 1)
			    Run = false;
		   
		    else if (Step == 2)
		    {
			    SequenceDiff--;
			    ClientSequence++;
			    
			    if (SequenceDiff <= 0)
			    {
				    SequenceDiff = 0;
				    P = false;
				    
				    Step = -1;
				    SetSequence(instance, ClientSequence);
				    
				    return;
			    }
			    
				P = true;

			    byte analog = (byte)(instance.player.movement.horizontal << 4 | instance.player.movement.vertical);	
			    
				instance.player.life.simulate(instance.simulation);
				instance.player.stance.simulate(instance.simulation, instance.player.stance.crouch, instance.player.stance.prone, instance.player.stance.sprint);
			   
			    float pitch = instance.player.look.pitch;
			    float yaw = instance.player.look.yaw;
			    
			    instance.player.movement.simulate(instance.simulation, 0, instance.player.movement.horizontal - 1, instance.player.movement.vertical - 1, instance.player.look.look_x, instance.player.look.look_y, instance.player.movement.jump, instance.player.stance.sprint, Vector3.zero, PlayerInput.RATE);
			   
			   	instance.player.equipment.simulate(instance.simulation, instance.player.equipment.primary, instance.player.equipment.secondary, instance.player.stance.sprint);
			    instance.player.animator.simulate(instance.simulation, instance.player.animator.leanLeft, instance.player.animator.leanRight);

			    SetSim(instance, GetSim(instance) + 1);
			    
			    instance.channel.openWrite();
			    instance.channel.write((byte)1); // one packet
				    
			    WalkingPlayerInputPacket packet = new WalkingPlayerInputPacket();
			    
			    packet.sequence = ClientSequence;
			    packet.recov = instance.recov; // max recov to fuck over tellRecov
			    packet.analog = analog; //1 << 4 | 1; // not moving
			    packet.position = instance.transform.localPosition; //lastSentPositon;// last sent position to server

			    packet.pitch = pitch;
			    packet.yaw = yaw;

			    if (MiscOptions.PunchAura)
			    {
				    if (SequenceDiff % 6 == 0)
				    {
					    if (MiscOptions.PunchSilentAim)
						    OV_DamageTool.OVType = OverrideType.PlayerHit;
			    
					    Ray         ray         = new Ray(instance.player.look.aim.position, instance.player.look.aim.forward);
					    RaycastInfo raycastInfo = DamageTool.raycast(ray, 6f, RayMasks.DAMAGE_CLIENT);
			    
					    OV_DamageTool.OVType = OverrideType.None;	
				    
					    packet.clientsideInputs = new List<RaycastInfo> {raycastInfo};
					    packet.keys |= 1 << 1; //set bit
				    }
					    
				    else
					    packet.keys = (ushort) (packet.keys & ~(1 << 1)); //clear bit
			    }
			    
			    instance.channel.write((byte)0); // walking player input packet
			    packet.write(instance.channel);
			    
			    instance.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
		    }
	    }

	    [Override(typeof(PlayerInput), "Start", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public static void OV_Start(PlayerInput instance)
	    {
		    OptimizationVariables.MainPlayer = Player.player;
		    OverrideUtilities.CallOriginal(instance);
	    }
    }
}