using System.Collections.Generic;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options
{
	public static class MiscOptions
	{
		[Save] public static bool NoSnow = false;
		[Save] public static bool NoRain = false;

		[Save] public static bool CustomSalvageTime = false;
		[Save] public static float SalvageTime = 1f;

		[Save] public static bool SetTimeEnabled = false;
		[Save] public static float Time = 0f;

		[Save] public static bool SlowFall = false;
        [Save] public static bool AirStick = false;

        [Save] public static bool Compass = false;
        [Save] public static bool GPS = false;
        [Save] public static bool SPOM = false; // Show Players On Map
        [Save] public static bool NightVision = false;

        public static bool WasNightVision = false;

        [Save] public static bool LogoEnabled = true;
		[Save] public static KeyCode LogoToggle = KeyCode.Slash;

		[Save] public static KeyCode ReloadConfig = KeyCode.Period;
		[Save] public static KeyCode SaveConfig = KeyCode.Comma;
		
		[Save] public static bool VisualsEnabled = true;

        [Save] public static string SpamText = "http://ironic.services/";
        [Save] public static bool SpammerEnabled = false;
        [Save] public static int SpammerDelay = 0;

        [Save] public static bool VehicleFly = false;
        [Save] public static float SpeedMultiplier = 1f;

		[Save] public static bool ExtendMeleeRange = false;
		[Save] public static float MeleeRangeExtension = 10f;

        [Save] public static KeyCode StrafeUp = KeyCode.RightControl;
        [Save] public static KeyCode StrafeDown = KeyCode.LeftControl;
        [Save] public static KeyCode StrafeLeft = KeyCode.LeftBracket;
        [Save] public static KeyCode StrafeRight = KeyCode.RightBracket;
        [Save] public static KeyCode MoveForward = KeyCode.W;
        [Save] public static KeyCode MoveBackward = KeyCode.S;
        [Save] public static KeyCode RotateLeft = KeyCode.A;
        [Save] public static KeyCode RotateRight = KeyCode.D;
        [Save] public static KeyCode RollLeft = KeyCode.Q;
        [Save] public static KeyCode RollRight = KeyCode.E;
        [Save] public static KeyCode RotateUp = KeyCode.Space;
        [Save] public static KeyCode RotateDown = KeyCode.LeftShift;


		[Save] public static HashSet<ulong> Friends = new HashSet<ulong>();

		[Save] public static KeyCode CrashServer = KeyCode.Keypad0;
	}
}
