﻿using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Attributes;

namespace Thanking.Options
{
	public static class MiscOptions
	{
		public static bool PanicMode = false;

		[Save] public static bool PunchSilentAim = false;
		[Save] public static bool PunchAura = false;
		
		[Save] public static bool NoSnow = false;
		[Save] public static bool NoRain = false;
        [Save] public static bool NoFlinch = false;
        [Save] public static bool NoGrayscale = false;

		[Save] public static bool CustomSalvageTime = false;
		[Save] public static float SalvageTime = 1f;

		[Save] public static bool SetTimeEnabled = false;
        [Save] public static float Time = 0f;

		[Save] public static bool SlowFall = false;
        [Save] public static bool AirStick = false;
 
        [Save] public static bool Compass = false;
        [Save] public static bool GPS = false;
        [Save] public static bool ShowPlayersOnMap = false; 
        [Save] public static bool NightVision = false;

        public static bool WasNightVision = false;

        [Save] public static string SpamText = "http://ironic.services/";
        [Save] public static bool SpammerEnabled = false;
        [Save] public static int SpammerDelay = 0;

        [Save] public static bool VehicleFly = false;
		[Save] public static bool VehicleUseMaxSpeed = false;
        [Save] public static float SpeedMultiplier = 1f;

		[Save] public static bool ExtendMeleeRange = false;
		[Save] public static float MeleeRangeExtension = 7.5f;

		public static bool NoMovementVerification = false;
		[Save] public static bool AlwaysCheckMovementVerification = false;

		public static Player SpectatedPlayer = null;
		public static Player CamPlayer = null;
		[Save] public static bool PlayerFlight = false;
		[Save] public static float FlightSpeedMultiplier = 1;
		
		public static bool Freecam = false;
		
		[Save] public static HashSet<ulong> Friends = new HashSet<ulong>();
		
		[Save] public static int SCrashMethod = 1;

		[Save] public static int AntiSpyMethod = 0;
		[Save] public static string AntiSpyPath = "";
		
		[Save] public static bool AlertOnSpy = false;
		[Save] public static int TimeAcceleration = 2;
		
		[Save] public static bool EnableDistanceCrash = false;
		[Save] public static float CrashDistance = 100;

		[Save] public static bool CrashByName = false;
        [Save] public static List<string> CrashWords = new List<string>();
        [Save] public static List<string> CrashIDs = new List<string>();

		[Save] public static bool NearbyItemRaycast = false;
		
		[Save] public static bool IncreaseNearbyItemDistance = false;
		[Save] public static float NearbyItemDistance = 15f;
	}
}
