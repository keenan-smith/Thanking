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
<<<<<<< HEAD
	    public static int Step = -1;
	    public static PlayerInputPacket LastPacket;

	    public static byte Analog;
	    public static float Yaw, Pitch;
	    
	    public static int Count;
	    public static int Buffer;
	    public static uint Clock = 1;

	    public static int Rate;

	    public static int ClientSequence = 1;
	    public static int SequenceDiff;

	    public static List<PlayerInputPacket> Packets = new List<PlayerInputPacket>();
	    public static List<RaycastInfo> TargetedInputs = new List<RaycastInfo>();

	    public static float LastReal;

=======
>>>>>>> radar-things
	    public static bool Run;
	    
	    public FieldInfo ClientsidePacketsField =
		    typeof(PlayerInput).GetField("clientsidePackets", BindingFlags.NonPublic | BindingFlags.Instance);

<<<<<<< HEAD
	    //[Override(typeof(PlayerMovement), "tellRecov", BindingFlags.Public | BindingFlags.Instance)]
	    public static void OV_tellRecov(PlayerMovement instance, CSteamID steamID, Vector3 newPosition, int newRecov)
	    {
			OverrideUtilities.CallOriginal(instance, steamID, newPosition, newRecov);
	    }

	    [Override(typeof(PlayerInput), "sendRaycast", BindingFlags.Public | BindingFlags.Instance)]
	    public static void OV_sendRaycast(PlayerInput instance, RaycastInfo info)
	    {
		    TargetedInputs.Add(info);
=======
	    public List<PlayerInputPacket> ClientsidePackets {
		    get {
			    if (!DrawUtilities.ShouldRun() || !Run)
				    return null;
			    
			   return (List<PlayerInputPacket>)ClientsidePacketsField.GetValue(OptimizationVariables.MainPlayer.input);
		    }
>>>>>>> radar-things
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
<<<<<<< HEAD
			    
		    if (!Run && SequenceDiff <= 0)
		    {
			    Rate = 4;
			    SequenceDiff = 0;
			    Step = -1;
				    
			    Run = false;
		    }
		    
		    if (Count % Rate == 0u)
		    {
			    if (Rate == 1)
				    SequenceDiff--;
			    
			    else if (Rate == 2 && Count % 4 == 0)
				    SequenceDiff--;

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
			    playerInputPacket.clientsideInputs = TargetedInputs.ToList(); //copy list
			    
			    TargetedInputs.Clear();
			    
			    if (MiscOptions.PunchAura)
			    {
				    if (Count % 6 == 0)
					    instance.keys[1] = true;

				    else
					    instance.keys[1] = false;
			    }
			    	
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

		    if (Buffer > 1 && Packets.Count > 0)
		    {
			    Buffer = 0;
			    
			    instance.channel.openWrite();
			    instance.channel.write((byte) Packets.Count);

			    bool cng = false;
			    
			    foreach (PlayerInputPacket playerInputPacket3 in Packets)
			    {
				    if (LastPacket != null)
				    {
					    if (playerInputPacket3.clientsideInputs == null &&
					        playerInputPacket3 is WalkingPlayerInputPacket packet &&
					    	LastPacket is WalkingPlayerInputPacket lPacket)
					    {
						    if (packet.analog == lPacket.analog &&
						        Mathf.Abs(packet.pitch - lPacket.pitch) < 0.01f &&
						        Mathf.Abs(packet.yaw - lPacket.yaw) < 0.01f &&
						        VectorUtilities.GetDistance(packet.position, lPacket.position) < 0.001f &&
						        packet.keys == 0 && 
						        packet.sequence != lPacket.sequence &&
						        Time.realtimeSinceStartup - LastReal < 8)
						    {
							    packet.sequence = lPacket.sequence;
							    SequenceDiff++;

							    cng = true;
						    }
					    }
				    }
				    
				    instance.channel.write((byte) (playerInputPacket3 is DrivingPlayerInputPacket ? 1 : 0));
				    playerInputPacket3.write(instance.channel);

				    LastPacket = playerInputPacket3;
			    }
			    
			    if (!cng)
				    LastReal = Time.realtimeSinceStartup;
			    
			    instance.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT);
			    Packets.Clear();

			    ClientSequence = LastPacket?.sequence ?? ClientSequence;
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
		    if (instance.player != Player.player)
		    {
			    OverrideUtilities.CallOriginal(instance);
			    return;
		    } 
			    
		    OptimizationVariables.MainPlayer = Player.player;
		    
		    Rate = 4;
		    Count = 0;
		    Buffer = 0;
		    
		    Packets.Clear();
		    LastPacket = null;
		    
		    SequenceDiff = 0;
		    ClientSequence = 0;

		    Step = -1;
		    
		    OverrideUtilities.CallOriginal(instance);
=======
		    OverrideUtilities.CallOriginal();
		    LastPacket = ClientsidePackets?.Last();
>>>>>>> radar-things
	    }
    }
}