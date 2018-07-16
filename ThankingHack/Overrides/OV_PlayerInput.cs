using System.Collections.Generic;
using System.Linq;
using System.Reflection;
 using SDG.Framework.Utilities;
 using SDG.Unturned;
 using Steamworks;
 using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Variables;
 using UnityEngine;
 
 namespace Thanking.Overrides
 {
	 public class OV_PlayerInput
	 {
		 public static bool WasPunching;
		 
		 public static float Pitch;

		 public static float Yaw;
		 
		 
		 public uint Simulation;
		 
		 public uint Clock;
		 
		 public uint Buffer;

		 public uint Consumed;

		 public uint ModCount;
		 
		 
		 public bool[] Keys = new bool[9];

		 public ushort[] Flags = new ushort[9];
		 

		 public int Sequence;

		 public int Recov;
		 

		 public SteamChannel Channel;

		 public Player PInstance;
		 

		 public Queue<InputInfo> Inputs = new Queue<InputInfo>();

		 public List<PlayerInputPacket> ClientsidePackets = new List<PlayerInputPacket>();

		 
		 //[Override(typeof(PlayerInput), "sendRaycast", BindingFlags.Public | BindingFlags.Instance)]
		 public void OV_sendRaycast(RaycastInfo info)
		 {
			 PlayerInputPacket playerInputPacket = ClientsidePackets.Last();
			 if (playerInputPacket.clientsideInputs == null)
				 playerInputPacket.clientsideInputs = new List<RaycastInfo>();

			 playerInputPacket.clientsideInputs.Add(info);
		 }

		 //[Override(typeof(PlayerInput), "askAck", BindingFlags.Public | BindingFlags.Instance)]
		 public void OV_askAck(CSteamID steamID, int ack)
		 {
			 if (!Channel.checkServer(steamID))
				 return;

			 for (int i = ClientsidePackets.Count - 1; i >= 0; i--)
				 if (ClientsidePackets[i].sequence <= ack)
					 ClientsidePackets.RemoveAt(i);
		 }

		 //[Override(typeof(PlayerInput), "FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance)]
		 public void OV_FixedUpdate()
		 {
			 if (Channel == null)
				 Channel = PInstance.input.channel;
			 
			 if (ModCount % PlayerInput.SAMPLES == 0u)
			 {
				 Keys[0] = PInstance.movement.jump;
				 Keys[1] = PInstance.equipment.primary;
				 Keys[2] = PInstance.equipment.secondary;
				 Keys[3] = PInstance.stance.crouch;
				 Keys[4] = PInstance.stance.prone;
				 Keys[5] = PInstance.stance.sprint;
				 Keys[6] = PInstance.animator.leanLeft;
				 Keys[7] = PInstance.animator.leanRight;
				 Keys[8] = false;
				 
				 if (MiscComponent.PunchEnabled)
				 {
					 if (WasPunching)
						 Keys[1] = false;

					 else
						 Keys[1] = true;
				 }
				 
				 PInstance.life.simulate(Simulation);
				 PInstance.stance.simulate(Simulation, PInstance.stance.crouch, PInstance.stance.prone,
					 PInstance.stance.sprint);
				 
				 Pitch = PInstance.look.pitch;
				 Yaw = PInstance.look.yaw;
				 
				 PInstance.movement.simulate(Simulation, 0, PInstance.movement.horizontal - 1,
					 PInstance.movement.vertical - 1, PInstance.look.look_x, PInstance.look.look_y,
					 PInstance.movement.jump, PInstance.stance.sprint, Vector3.zero, PlayerInput.RATE);
				 
				 Sequence++;
				 if (PInstance.stance.stance == EPlayerStance.DRIVING)
					 ClientsidePackets.Add(new DrivingPlayerInputPacket());
				 
				 else
					 ClientsidePackets.Add(new WalkingPlayerInputPacket());

				 PlayerInputPacket playerInputPacket = ClientsidePackets.Last();
				 playerInputPacket.sequence = Sequence;
				 playerInputPacket.recov = Recov;
				 
				 if (AimbotOptions.Enabled && playerInputPacket is WalkingPlayerInputPacket)
				 {
					 WalkingPlayerInputPacket wp = playerInputPacket as WalkingPlayerInputPacket;
					 
					 wp.pitch = Pitch;
					 wp.yaw = Yaw;

					 ClientsidePackets[ClientsidePackets.Count - 1] = wp;
				 }

				 PInstance.equipment.simulate(Simulation, PInstance.equipment.primary, PInstance.equipment.secondary,
					 PInstance.stance.sprint);
				 PInstance.animator.simulate(Simulation, PInstance.animator.leanLeft, PInstance.animator.leanRight);
				 
				 Buffer += PlayerInput.SAMPLES;
				 Simulation++;
			 }

			 if (Consumed < Buffer)
			 {
				 Consumed++;
				 PInstance.equipment.tock(Clock);
				 Clock++;
			 }

			 if (Consumed == Buffer && ClientsidePackets.Count > 0)
			 {
				 ushort num = 0;
				 
				 for (byte b = 0; b < Keys.Length; b++)
					 if (Keys[b])
						 num |= Flags[b];

				 PlayerInputPacket playerInputPacket2 = ClientsidePackets.Last();
				 playerInputPacket2.keys = num;

				 Channel.openWrite();
				 Channel.write((byte) ClientsidePackets.Count);
				 foreach (PlayerInputPacket playerInputPacket3 in ClientsidePackets)
				 {
					 Channel.write(playerInputPacket3 is DrivingPlayerInputPacket ? 1 : 0);
					 playerInputPacket3.write(Channel);
				 }
				 
				 Channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
			 }

			 ModCount++;
		 }

		 
		// [Override(typeof(PlayerInput), "Start", BindingFlags.NonPublic | BindingFlags.Instance)]
		 public void Start()
		 {
			 OptimizationVariables.MainPlayer = Player.player;
			 PInstance = Player.player;
			 
			 Simulation = 0u;
			 Clock = 0u;
			 
			 for (byte b = 0; b < Keys.Length; b++)
				 Flags[b] = (ushort) (1 << 8 - b);

			 Sequence = -1;
			 Recov = -1;
			 
			 ClientsidePackets.Clear();
			 Inputs.Clear();
		 }
	 }
 }