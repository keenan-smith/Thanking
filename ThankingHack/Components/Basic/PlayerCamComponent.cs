using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Components.UI.Menu;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Options.VisualOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
    /// <summary>
    /// Component used to manage the mirror camera
    /// </summary>
    [Component]
    public class PlayerCamComponent : MonoBehaviour
    {
        public static Rect viewport = new Rect(1075, 10, Screen.width / 4, Screen.height / 4); //Viewport of the mirror camera
        public static GameObject cam_obj;
        public static Camera subCam;
        public static bool WasEnabled;
        public static bool Enabled = true;

        [OnSpy]
        public static void Disable()
        {
            WasEnabled = MiscOptions.CamPlayer == null;
            Enabled = false;

            Destroy(cam_obj);
        }

        [OffSpy]
        public static void Enable() =>
            Enabled = WasEnabled;

        public void Update()
        {
            if (!cam_obj || !subCam)
                return;

            if (Enabled)
                subCam.enabled = true;
            else
                subCam.enabled = false;

        }

        void Start()
        {
            //InvokeRepeating("FixCam", 0f, 15f);
        }

        void OnGUI()
        {
            if (Enabled && MiscOptions.CamPlayer != null && Provider.isConnected)
            {
                GUI.color = new Color(1f, 1f, 1f, 0f);
                viewport = GUILayout.Window(98, viewport, DoMenu, "Player Cam");
                GUI.color = Color.white;
            }
            if (MiscOptions.CamPlayer == null || !Provider.isConnected && (subCam != null && cam_obj != null))
            {
                Destroy(subCam);
                subCam = null;
                cam_obj = null;
            }
        }

        void DoMenu(int windowID)
        {
            if (cam_obj == null || subCam == null)
            {
                cam_obj = new GameObject();
                if (subCam != null)
                    Destroy(subCam);
                subCam = cam_obj.AddComponent<Camera>();
                subCam.CopyFrom(OptimizationVariables.MainCam);
                cam_obj.AddComponent<GUILayer>();
                subCam.enabled = true;
                subCam.rect = new Rect(0.6f, 0.6f, 0.6f, 0.4f);
                subCam.depth = 98;
                DontDestroyOnLoad(cam_obj);
            }
            subCam.transform.SetPositionAndRotation(AimbotCoroutines.GetAimPosition(MiscOptions.CamPlayer.transform, "Skull") + new Vector3(0, 0.2f, 0) + (MiscOptions.CamPlayer.look.aim.forward / 1.6f), MiscOptions.CamPlayer.look.aim.rotation);

            float x, y, w, h;
            x = (viewport.x) / Screen.width;
            y = (viewport.y + 25) / Screen.height;
            w = (viewport.width) / Screen.width;
            h = (viewport.height) / Screen.height;
            y = 1 - y;
            y -= h;
            subCam.rect = new Rect(x, y, w, h);

            Drawing.DrawRect(new Rect(0, 0, viewport.width, 20), new Color32(44, 44, 44, 255));
            Drawing.DrawRect(new Rect(0, 20, viewport.width, 5), new Color32(34, 34, 34, 255));
            GUILayout.Space(-19);
            GUILayout.Label("Player Cam: " + ESPComponent.GetSteamPlayer(MiscOptions.CamPlayer).playerID.characterName);

            GUI.DragWindow();
        }
    }
}
