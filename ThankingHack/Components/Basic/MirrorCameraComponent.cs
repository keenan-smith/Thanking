using Thanking.Attributes;
using Thanking.Components.UI.Menu;
using Thanking.Options.VisualOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
    /// <summary>
    /// Component used to manage the mirror camera
    /// </summary>
    [Component]
    public class MirrorCameraComponent : MonoBehaviour
    {
        public static Rect viewport = new Rect(1075, 10, Screen.width / 4, Screen.height / 4); //Viewport of the mirror camera
        public static GameObject cam_obj;
        public static Camera subCam;
        public static bool WasEnabled;
        
        [OnSpy]
        public static void Disable()
        {
            WasEnabled = MirrorCameraOptions.Enabled;
            MirrorCameraOptions.Enabled = false;
            
            Destroy(cam_obj);
        }
        
        [OffSpy]
        public static void Enable() =>
            MirrorCameraOptions.Enabled = WasEnabled;
        
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
            GUILayout.Label("Mirror Camera");

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
