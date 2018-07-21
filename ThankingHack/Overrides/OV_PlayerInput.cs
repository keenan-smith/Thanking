using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public class OV_PlayerInput
    {
	    public float Tick;
	    
	    public uint Simulation;
	    public uint ESiumulation;
	    public uint Count;
	    public uint Clock;
	    
	    public byte Analog;
	    
	    public int Sequence;
	    
	    public List<PlayerInputPacket> ClientsidePackets;
	    
	    [Override(typeof(PlayerInput), "get_tick", BindingFlags.Public | BindingFlags.Instance)]
	    public float OV_get_tick() =>
		    Tick;

	    [Override(typeof(PlayerInput), "get_simulation", BindingFlags.Public | BindingFlags.Instance)]
	    public uint OV_get_simulation() =>
		    Simulation;

	    [Override(typeof(PlayerInput), "get_clock", BindingFlags.Public | BindingFlags.Instance)]
	    public uint OV_get_clock() =>
		    Clock;

	    [Override(typeof(PlayerInput), "sendRaycast", BindingFlags.Public | BindingFlags.Instance)]
        public void OV_sendRaycast(RaycastInfo ri)
        {
            TracerLine tl = new TracerLine
            {
                StartPosition = OptimizationVariables.MainPlayer.look.aim.transform.position,
                EndPosition = ri.point,
                Hit = ri.animal || ri.player || ri.zombie,
                CreationTime = DateTime.Now    
            };
            WeaponComponent.Tracers.Add(tl);
            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.input, ri);
        }

	    [Override(typeof(PlayerInput), "FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance)]
	    private void OV_FixedUpdate()
	    {
		    if (Provider.isServer)
		    {
			    OverrideUtilities.CallOriginal();
			    return;
		    }

		    Player plr = OptimizationVariables.MainPlayer;
		    PlayerInput inp = plr.input;

		    if (Count % PlayerInput.SAMPLES == 0u)
		    {
			    Tick = Time.realtimeSinceStartup;
			    inp.keys[0] = plr.movement.jump;
			    inp.keys[1] = plr.equipment.primary;
			    inp.keys[2] = plr.equipment.secondary;
			    inp.keys[3] = plr.stance.crouch;
			    inp.keys[4] = plr.stance.prone;
			    inp.keys[5] = plr.stance.sprint;
			    inp.keys[6] = plr.animator.leanLeft;
			    inp.keys[7] = plr.animator.leanRight;
			    inp.keys[8] = false;
			    Analog = (byte) (plr.movement.horizontal << 4 | plr.movement.vertical);
			    inp.player.life.simulate(Simulation);
			    inp.player.stance.simulate(Simulation, plr.stance.crouch, plr.stance.prone, plr.stance.sprint);
			    plr.movement.simulate(Simulation, 0, plr.movement.horizontal - 1, plr.movement.vertical - 1, plr.look.look_x, plr.look.look_y, plr.movement.jump, plr.stance.sprint, Vector3.zero, PlayerInput.RATE);
			    if (plr.stance.stance == EPlayerStance.DRIVING)
				    ClientsidePackets.Add(new DrivingPlayerInputPacket());

			    else
				    ClientsidePackets.Add(new WalkingPlayerInputPacket());

			    PlayerInputPacket playerInputPacket = ClientsidePackets.Last();
			    playerInputPacket.sequence = ++Sequence;
			    playerInputPacket.recov = inp.recov;

			    plr.equipment.simulate(Simulation, plr.equipment.primary, plr.equipment.secondary, plr.stance.sprint);
			    plr.animator.simulate(Simulation, plr.animator.leanLeft, plr.animator.leanRight);
			    
			    if (plr.stance.stance != EPlayerStance.DRIVING && 
			        plr.stance.stance != EPlayerStance.SPRINT &&
			        plr.stance.stance != EPlayerStance.PRONE && 
			        plr.stance.stance != EPlayerStance.CLIMB &&
			        plr.stance.stance != EPlayerStance.SITTING &&
			        MiscOptions.PunchAura)
			    {
				    for (int i = 0; i < 20; i++)
				    {
					    inp.keys[1] = !inp.keys[1];
					    
					    ushort num2 = 0;
					    for (int b = 0; b < inp.keys.Length; b++)
						    if (inp.keys[b])
							    num2 |= (ushort) (1 << 8 - b);
					    
					    WalkingPlayerInputPacket playerInputPacket4 = new WalkingPlayerInputPacket
					    {
						    sequence = ++Sequence,
						    recov = inp.recov,
						    keys = num2
					    };

					    ClientsidePackets.Add(playerInputPacket4);
					    plr.equipment.simulate(ESiumulation++, plr.equipment.primary, plr.equipment.secondary, plr.stance.sprint);
				    }
			    }
			    plr.equipment.simulate(ESiumulation++, plr.equipment.primary, plr.equipment.secondary, plr.stance.sprint);
			    Simulation++;
		    }

		    plr.equipment.tock(Clock);

		    ushort num = 0;
		    for (int b = 0; b < inp.keys.Length; b++)
			    if (inp.keys[b])
				    num |= (ushort) (1 << 8 - b);

		    PlayerInputPacket playerInputPacket2 = ClientsidePackets.Last();
		    playerInputPacket2.keys = num;
		    if (playerInputPacket2 is DrivingPlayerInputPacket drivingPlayerInputPacket)
		    {
			    if (plr.movement.getVehicle() != null && plr.movement.getVehicle().asset.engine == EEngine.TRAIN)
				    drivingPlayerInputPacket.position = new Vector3(plr.movement.getVehicle().roadPosition, 0f, 0f);

			    else
				    drivingPlayerInputPacket.position = plr.transform.parent.parent.parent.position;

			    drivingPlayerInputPacket.angle_x =
				    MeasurementTool.angleToByte2(plr.transform.parent.parent.parent.rotation.eulerAngles.x);
			    drivingPlayerInputPacket.angle_y =
				    MeasurementTool.angleToByte2(plr.transform.parent.parent.parent.rotation.eulerAngles.y);
			    drivingPlayerInputPacket.angle_z =
				    MeasurementTool.angleToByte2(plr.transform.parent.parent.parent.rotation.eulerAngles.z);
			    drivingPlayerInputPacket.speed =
				    (byte) (Mathf.Clamp(plr.movement.getVehicle().speed, -100f, 100f) + 128f);
			    drivingPlayerInputPacket.physicsSpeed =
				    (byte) (Mathf.Clamp(plr.movement.getVehicle().physicsSpeed, -100f, 100f) + 128f);
			    drivingPlayerInputPacket.turn = (byte) (plr.movement.getVehicle().turn + 1);
		    }
		    else
		    {
			    WalkingPlayerInputPacket walkingPlayerInputPacket = playerInputPacket2 as WalkingPlayerInputPacket;
			    walkingPlayerInputPacket.analog = Analog;
			    walkingPlayerInputPacket.position = plr.transform.localPosition;
			    walkingPlayerInputPacket.yaw = plr.look.yaw;
			    walkingPlayerInputPacket.pitch = plr.look.pitch;
		    }

		    inp.channel.openWrite();

		    inp.channel.write((byte) ClientsidePackets.Count);
		    foreach (PlayerInputPacket playerInputPacket3 in ClientsidePackets)
		    {
			    inp.channel.write(playerInputPacket3 is DrivingPlayerInputPacket ? 1 : 0);
			    playerInputPacket3.write(inp.channel);
		    }

		    inp.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
		    Count++;
	    }
	    
	    [Override(typeof(PlayerInput), "askAck", BindingFlags.Public | BindingFlags.Instance)]
	    public void OV_askAck(CSteamID steamID, int ack)
	    {
		    if (!OptimizationVariables.MainPlayer.channel.checkServer(steamID))
			    return;
		    
		    for (int i = ClientsidePackets.Count - 1; i >= 0; i--)
		    {
			    PlayerInputPacket playerInputPacket = ClientsidePackets[i];
			    if (playerInputPacket.sequence <= ack)
				    ClientsidePackets.RemoveAt(i);
		    }
	    }

	    [Override(typeof(PlayerInput), "Start", BindingFlags.NonPublic | BindingFlags.Instance)]
	    public void OV_Start()
	    {
		    OverrideUtilities.CallOriginal();
		    
		    Tick = Time.realtimeSinceStartup;
		    Simulation = ESiumulation = Count = Clock = 0;
		    Analog = 0;
		    Sequence = 0;
		    ClientsidePackets = new List<PlayerInputPacket>();
	    }
    }
}