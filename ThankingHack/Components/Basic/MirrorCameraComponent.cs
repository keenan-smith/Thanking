using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI.Menu;
using Thanking.Coroutines;
using Thanking.Options.VisualOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
    /// <summary>
    /// Component used to manage the mirror camera
    /// </summary>
    [Component]
    [SpyComponent]
    public class MirrorCameraComponent : MonoBehaviour
    {
        Rect viewport = new Rect(1075, 10, Screen.width / 4, Screen.height / 4); //Viewport of the mirror camera
        public static GameObject cam_obj;
        public static Camera subCam;

        public void Update()
        {
            if (!cam_obj || !subCam)
                return;

            if (MirrorCameraOptions.Enabled)
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
            if (MirrorCameraOptions.Enabled)
            {
                GUI.color = new Color(1f, 1f, 1f, 0f);
                viewport = GUILayout.Window(99, viewport, DoMenu, "Mirror Camera");
                GUI.color = Color.white;
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
                cam_obj.transform.position = OptimizationVariables.MainCam.gameObject.transform.position;
                cam_obj.transform.rotation = OptimizationVariables.MainCam.gameObject.transform.rotation;
                cam_obj.transform.Rotate(0, 180, 0);
                subCam.transform.SetParent(OptimizationVariables.MainCam.transform, true);
                subCam.enabled = true;
                subCam.rect = new Rect(0.6f, 0.6f, 0.4f, 0.4f);
                subCam.depth = 99;
                DontDestroyOnLoad(cam_obj);
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Mirror Camera");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            float x, y, w, h;
            x = (viewport.x + 5) / Screen.width;
            y = (viewport.y + 5) / Screen.height;
            w = (viewport.width - 10) / Screen.width;
            h = (viewport.height - 10) / Screen.height;
            y = 1 - y;
            y -= h;
            subCam.rect = new Rect(x, y, w, h);

            Drawing.DrawRect(new Rect(0, 0, viewport.width, 5), Color.black);
            Drawing.DrawRect(new Rect(0, 0, 5, viewport.height), Color.black);
            Drawing.DrawRect(new Rect(0, 0 + (viewport.height - 5), viewport.width, 5), Color.black);
            Drawing.DrawRect(new Rect(0 + (viewport.width - 5), 0, 5, viewport.height), Color.black);

            GUI.DragWindow();

        }

        public static void FixCam()
        {
            if (cam_obj != null && subCam != null)
            {
                cam_obj.transform.position = OptimizationVariables.MainCam.gameObject.transform.position;
                cam_obj.transform.rotation = OptimizationVariables.MainCam.gameObject.transform.rotation;
                cam_obj.transform.Rotate(0, 180, 0);
                subCam.transform.SetParent(OptimizationVariables.MainCam.transform, true);
                subCam.depth = 99;
                subCam.enabled = true;
            }
        }

    }
}
