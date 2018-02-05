using SDG.Unturned;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Variables
{
    public static class OptimizationVariables
    {
        public static Player MainPlayer;
        public static Camera MainCam;

        [Initializer]
        public static void Init()
        {
            MainPlayer = Player.player;
            MainCam = Camera.main;
        }
    }
}