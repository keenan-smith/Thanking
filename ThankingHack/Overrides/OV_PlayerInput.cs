﻿﻿using System.Collections.Generic;
 using System.Linq;
 using System.Reflection;
 using SDG.Unturned;
 using Steamworks;
 using Thinking.Attributes;
 using Thinking.Options;
 using Thinking.Utilities;
 using Thinking.Variables;
 using UnityEngine;

namespace Thinking.Overrides
{
    public class OV_PlayerInput
    {
	    public static int Step = -1;

	    public static byte Analog;
	    public static float Yaw, Pitch;
	    
	    public static int Count;
	    public static int Buffer;
	    public static uint Clock = 1;

	    public static int Rate;

	    public static int ClientSequence = 1;
	    public static int SequenceDiff;

	    public static List<PlayerInputPacket> Packets = new List<PlayerInputPacket>();

	    public static float LastReal;

	    public static bool Run;
	    public static bool P;
	    
	    public static FieldInfo SimField = 
		    typeof(PlayerInput).GetField("_simulation", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static FieldInfo ClockField = 
		    typeof(PlayerInput).GetField("_clock", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static FieldInfo TickField = 
		    typeof(PlayerInput).GetField("_tick", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static float GetTick(PlayerInput instance) =>
		    (float)TickField.GetValue(instance);
	    
	    public static void SetTick(PlayerInput instance, float value) =>
		    TickField.SetValue(instance, value); 

	    
	    public static uint GetSim(PlayerInput instance) =>
		    (uint)SimField.GetValue(instance);
	    
	    public static void SetSim(PlayerInput instance, uint value) =>
		    SimField.SetValue(instance, value); 
	    
	    
	    public static uint GetClock(PlayerInput instance) =>
		    (uint)ClockField.GetValue(instance);
	    
	    public static void SetClock(PlayerInput instance, uint value) =>
		    ClockField.SetValue(instance, value); 
	    
	    private static Vector3 lastSentPositon = Vector3.zero;

	    //[Override(typeof(PlayerMovement), "tellRecov", BindingFlags.Public | BindingFlags.Instance)]
	    //public static void OV_tellRecov(PlayerMovement instance, CSteamID steamID, Vector3 newPosition, int newRecov)
	    //{
		//    if (P)
		//    {
		//	    player.input.recov = newRecov;
		//	    return;
		//    }
//
		//    OverrideUtilities.CallOriginal(instance, steamID, newPosition, newRecov);
	    //}

	    [Override(typeof(PlayerInput), "sendRaycast", BindingFlags.Public | BindingFlags.Instance)]
	    public static void OV_sendRaycast(PlayerInput instance, RaycastInfo info)
	    {
		    PlayerInputPacket last =
			    Packets.Last();
		    
		    if (last.clientsideInputs == null)
			    last.clientsideInputs = new List<RaycastInfo>();
		    
		    last.clientsideInputs.Add(info);
	    }
	    
	    [Override(typeof(PlayerInput), "FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public static void OV_FixedUpdate(PlayerInput instance)
	    {
		    if (instance.player != OptimizationVariables.MainPlayer)
			    return;

		    Player player = OptimizationVariables.MainPlayer;
		    
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
				    
				    Ray         ray         = new Ray(player.look.aim.position, player.look.aim.forward);
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
			    /*
			    Buffer++;
			    if (Buffer > 3)
				    Buffer = 0;

			    if (MiscOptions.TimeAcceleration == 1 && Buffer % 4 != 0)
				    return;

			    if (MiscOptions.TimeAcceleration == 2 && Buffer % 2 != 0)
				    return;
*/
			    
			    Run = false;

			    switch (MiscOptions.TimeAcceleration)
			    {
				    case 1:
					    Rate = 4;
					    break;

				    case 2:
					    Rate = 2;
					    break;

				    case 4:
					    Rate = 1;
					    break;
			    }

			    P = true;
		    }
		    
		    if (Count % Rate == 0u)
		    {
			    if (Rate == 1)
				    SequenceDiff--;
			    
			    else if (Rate == 2 && Count % 4 == 0)
				    SequenceDiff--;
			    
			    if (SequenceDiff < 0)
			    {
				    Rate = 4;
				    SequenceDiff = 0;
				    Step = -1;
				    
				    Run = false;
			    }

			    SetTick(instance, Time.realtimeSinceStartup);

			    instance.keys[0] = player.movement.jump;
			    instance.keys[1] = player.equipment.primary;
			    instance.keys[2] = player.equipment.secondary;
			    instance.keys[3] = player.stance.crouch;
			    instance.keys[4] = player.stance.prone;
			    instance.keys[5] = player.stance.sprint;
			    instance.keys[6] = player.animator.leanLeft;
			    instance.keys[7] = player.animator.leanRight;
			    instance.keys[8] = false;

			    for (int i = 0; i < (int) ControlsSettings.NUM_PLUGIN_KEYS; i++)
			    {
				    int num = instance.keys.Length - ControlsSettings.NUM_PLUGIN_KEYS + i;
				    instance.keys[num] = Input.GetKey(ControlsSettings.getPluginKeyCode(i));
			    }

			    Analog = (byte) (player.movement.horizontal << 4 | player.movement.vertical);
			    
			    player.life.simulate(instance.simulation);
			    player.stance.simulate(instance.simulation, player.stance.crouch, player.stance.prone, player.stance.sprint);
			  	
			    Pitch = player.look.pitch;
			 	Yaw = player.look.yaw;
			    
			    player.movement.simulate(instance.simulation, 0, player.movement.horizontal - 1, player.movement.vertical - 1, player.look.look_x, player.look.look_y, player.movement.jump, player.stance.sprint, Vector3.zero, PlayerInput.RATE);

			    if (Run)
			    {
				    if (Time.realtimeSinceStartup - LastReal > 8)
				    {
					    LastReal = Time.realtimeSinceStartup;

					    SequenceDiff--;
					    ClientSequence++;
				    }

				    SequenceDiff++;
			    }

			    else
				    ClientSequence++;

			    if (player.stance.stance == EPlayerStance.DRIVING)
				    Packets.Add(new DrivingPlayerInputPacket());

			    else
				    Packets.Add(new WalkingPlayerInputPacket());

			    PlayerInputPacket playerInputPacket = Packets.Last();
			    playerInputPacket.sequence = ClientSequence;
			    playerInputPacket.recov = instance.recov;
			    
			    ushort num2 = 0;
			    
			    for (byte b = 0; b < instance.keys.Length; b++)
				    if (instance.keys[b])
					    num2 |= (ushort) (1 << b);
			    	
			    playerInputPacket.keys = num2;

			    if (playerInputPacket is DrivingPlayerInputPacket drivingPlayerInputPacket)
			    {
				    InteractableVehicle vehicle = player.movement.getVehicle();
				    if (vehicle != null)
				    {
					    Transform transform = vehicle.transform;
					    drivingPlayerInputPacket.position = vehicle.asset.engine == EEngine.TRAIN
						    ? new Vector3(vehicle.roadPosition, 0f, 0f)
						    : transform.position;

					    drivingPlayerInputPacket.angle_x =
						    MeasurementTool.angleToByte2(transform.rotation.eulerAngles.x);
					    drivingPlayerInputPacket.angle_y =
						    MeasurementTool.angleToByte2(transform.rotation.eulerAngles.y);
					    drivingPlayerInputPacket.angle_z =
						    MeasurementTool.angleToByte2(transform.rotation.eulerAngles.z);
					    drivingPlayerInputPacket.speed = (byte) (Mathf.Clamp(vehicle.speed, -100f, 100f) + 128f);
					    drivingPlayerInputPacket.physicsSpeed =
						    (byte) (Mathf.Clamp(vehicle.physicsSpeed, -100f, 100f) + 128f);
					    drivingPlayerInputPacket.turn = (byte) (vehicle.turn + 1);
				    }
			    }
			    else
			    {
				    WalkingPlayerInputPacket walkingPlayerInputPacket = playerInputPacket as WalkingPlayerInputPacket;
				    
				    walkingPlayerInputPacket.analog = Analog;
				    walkingPlayerInputPacket.position = instance.transform.localPosition;
				    walkingPlayerInputPacket.yaw = Yaw;
				    walkingPlayerInputPacket.pitch = Pitch;
			    }

			    player.equipment.simulate(instance.simulation, player.equipment.primary, player.equipment.secondary, player.stance.sprint);
			    player.animator.simulate(instance.simulation, player.animator.leanLeft, player.animator.leanRight);
			    
			    SetSim(instance, GetSim(instance) + 1);
			    Buffer += Rate;
		    }

		    player.equipment.tock(Clock++);

		    if (Buffer > 4 && Packets.Count > 0)
		    {
			    Buffer = 0;
			    
			    instance.channel.openWrite();
			    instance.channel.write((byte) Packets.Count);
			    foreach (PlayerInputPacket playerInputPacket3 in Packets)
			    {
				    instance.channel.write((byte) (playerInputPacket3 is DrivingPlayerInputPacket ? 1 : 0));
				    playerInputPacket3.write(instance.channel);
			    }

			    instance.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT);
			    Packets.Clear();
		    }

		    Count++;

		    /*
		    SBuffer++;
		    if (SBuffer > 3)
			    SBuffer = 0;
		    
		    if (Step == 2 && SBuffer % 4 == 0 && FakePakets.Count > 0)
		    {
			    instance.channel.openWrite();
			    instance.channel.write((byte) FakePakets.Count); // one packet
			    
			    foreach (var packet in FakePakets)
			    {
				    instance.channel.write((byte) 0); // walking player input packet
				    packet.write(instance.channel);
			    }
			   
			    instance.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT);
			    FakePakets.Clear();
		    }*/
	    }

	    [Override(typeof(PlayerInput), "Start", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public static void OV_Start(PlayerInput instance)
	    {
		    OptimizationVariables.MainPlayer = Player.player;
		    
		    Rate = 4;
		    Count = 0;
		    Buffer = 0;
		    
		    Packets.Clear();
		    
		    SequenceDiff = 0;
		    ClientSequence = 0;

		    Step = -1;
		    
		    OverrideUtilities.CallOriginal(instance);
	    }
    }
}