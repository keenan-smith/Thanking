using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thanking.Components.UI;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Coroutines
{
    public static class TrajectoryCoroutines
    {
        private static readonly List<GameObject> gameObjectOut = new List<GameObject>();
        public static IEnumerator UpdateBodiesInMotionSet()
        {
            while (true)
            {
                if (!DrawUtilities.ShouldRun() || !WeaponOptions.EnableBulletDropPrediction)
                {
                    yield return new WaitForSeconds(1);
                    continue;
                }

                Level.effects.FindAllChildrenWithName("Projectile", gameObjectOut);
                Level.effects.FindAllChildrenWithName("Throwable", gameObjectOut);

                foreach (var gameObject in gameObjectOut)
                {
                    if (gameObject.GetComponent<Rigidbody>()?.velocity == Vector3.zero)
                        continue;

                    if (gameObject.name == "Projectile")
                        TrajectoryComponent.BodiesInMotion.Add(gameObject);
                    else if (gameObject.name == "Throwable")
                    {
                        var sticky = gameObject.GetComponent<StickyGrenade>();
                        if (sticky != null && gameObject.GetComponent<Rigidbody>()?.useGravity == false)
                            continue;

                        TrajectoryComponent.BodiesInMotion.Add(gameObject);
                    }
                }

                gameObjectOut.Clear();

                yield return new WaitForSeconds(0.2F);
            }
        }
    }
}
