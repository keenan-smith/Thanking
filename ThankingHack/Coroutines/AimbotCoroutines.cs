using System.Collections;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Coroutines
{
    [SpyComponent]
    [Component]
    public static class AimbotCoroutines
    {
        public static Player LockedPlayer;

        public static IEnumerator SetLockedObject()
        {
            while (true)
            {
                Player p = null;
                SteamPlayer[] players = Provider.clients.ToArray();
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].playerID.steamID != Provider.client && players[i].player.life != null && !players[i].player.life.isDead)
                    {
                        switch (AimbotOptions.TargetMode)
                        {
                            case TargetMode.Distance:
                                {
                                    Vector3 v2dist = Camera.main.WorldToScreenPoint(GetAimPosition(players[i].player.transform, "Skull"));

                                    Vector2 pos = new Vector2(v2dist.x, v2dist.y);
                                    float vdist = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), pos);

                                    if (vdist < AimbotOptions.FOV)
                                        p = players[i].player;
                                    break;
                                }
                            case TargetMode.FOV:
                                {
                                    if (p == null)
                                        p = players[i].player;
                                    else
                                    {
                                        if (p != null)
                                            if (VectorUtilities.GetDistance(p.transform.position) > VectorUtilities.GetDistance(players[i].player.transform.position))
                                                p = players[i].player;
                                    }
                                    break;
                                }
                        }
                    }
                    LockedPlayer = p;
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public static IEnumerator AimToObject()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public static Vector3 GetAimPosition(Transform parent, string name)
        {
            Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null)
            {
                Transform[] array = componentsInChildren;
                for (int i = 0; i < array.Length; i++)
                {
                    Transform tr = array[i];
                    if (tr.name.Trim() == name)
                        return tr.position + new Vector3(0f, 0.4f, 0f);
                }
            }
            return new Vector3(1000, 1000, 1000);
        }
    }
}