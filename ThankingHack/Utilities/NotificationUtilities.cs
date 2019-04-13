using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thanking.Attributes;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class NotificationUtilities
    {
        private static bool beingSpied;

        [OnSpy]
        public static void OnSpy()
        {
            beingSpied = true;
        }

        [OffSpy]
        public static void OffSpy()
        {
            beingSpied = false;
        }

        public static void DisplayNotification(EPlayerMessage type, string message, Color color, float displayTime) => 
            OptimizationVariables.MainPlayer.StartCoroutine(DisplayNotificationCoroutine(type, message, color, displayTime));

        private static IEnumerator DisplayNotificationCoroutine(EPlayerMessage type, string message, Color color, float displayTime)
        {
            var started = Time.realtimeSinceStartup;
            while (true)
            {
                yield return new WaitForEndOfFrame();

                if (!beingSpied)
                    PlayerUI.hint(null, type, message, color);

                if (Time.realtimeSinceStartup - started > displayTime)
                    yield break;
            }
        }
    }
}
