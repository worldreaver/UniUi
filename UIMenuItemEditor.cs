#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Worldreaver.UniUI
{
    public static class UIMenuItemEditor
    {
        [MenuItem("GameObject/UI/Button - Empty", false)]
        private static void AddUniButton()
        {
            AddUniObjectComponent<UniButton>("Button");
        }

        [MenuItem("GameObject/UI/Button - TMP", false)]
        private static void AddUniButtonTMP()
        {
            AddUniObjectComponent<UniButtonTMP>("Button");
        }

        [MenuItem("GameObject/UI/Tab", false)]
        private static void AddUniTab()
        {
            AddUniObjectComponent<UniTap>("Tab");
        }

        [MenuItem("GameObject/UI/Tab - TMP", false)]
        private static void AddUniTabTMP()
        {
            AddUniObjectComponent<UniTapTMP>("Tab");
        }

        [MenuItem("GameObject/UI/UniProgress", false)]
        private static void AddProgress()
        {
            var root = AddUniObjectComponent<UniProgressBar>("ProgressBar");
            if (root != null)
            {
                root.sizeDelta = new Vector2(200, 20);
                var foreground = CreateUniObject<Image>(root, "Foreground");
                foreground.sizeDelta = root.sizeDelta;
                root.GetComponent<UniProgressBar>().ForegroundBar = foreground;
            }
        }

        [MenuItem("GameObject/UI/UniProgress - TMP", false)]
        private static void AddProgressTMP()
        {
            var root = AddUniObjectComponent<UniProgressBarTMP>("ProgressBar");
            if (root != null)
            {
                root.sizeDelta = new Vector2(200, 20);
                var foreground = CreateUniObject<Image>(root, "Foreground");
                foreground.sizeDelta = root.sizeDelta;
                var text = CreateUniObject<TextMeshProUGUI>(root, "Text");
                text.sizeDelta = root.sizeDelta;
                var progressBarTmp = root.GetComponent<UniProgressBarTMP>();
                progressBarTmp.ForegroundBar = foreground;
                progressBarTmp.Text = text.GetComponent<TextMeshProUGUI>();
            }
        }

        private static RectTransform AddUniObjectComponent<T>(string name) where T : Component
        {
            // find canvas in scene
            var allCanvases = (Canvas[]) Object.FindObjectsOfType(typeof(Canvas));
            if (allCanvases.Length > 0)
            {
                if (Selection.activeTransform == null) return CreateUniObject<T>(allCanvases[0].transform, name);

                for (int i = 0; i < allCanvases.Length; i++)
                {
                    if (!Selection.activeTransform.IsChildOf(allCanvases[i].transform)) continue;
                    return CreateUniObject<T>(Selection.activeTransform, name);
                }

                return CreateUniObject<T>(allCanvases[0].transform, name);
            }

            var canvas = CreateCanvas();
            return CreateUniObject<T>(canvas.transform, name);
        }

        private static Canvas CreateCanvas()
        {
            var canvas = new GameObject("Canvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.gameObject.AddComponent<GraphicRaycaster>();

            var eventSystem = (EventSystem) Object.FindObjectOfType(typeof(EventSystem));
            if (eventSystem == null)
            {
                eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }

            return canvas;
        }

        private static RectTransform CreateUniObject<T>(Transform parent,
            string name) where T : Component
        {
            var button = new GameObject(name).AddComponent<T>();
            RectTransform rt = button.GetComponent<RectTransform>();
            if (rt == null)
            {
                rt = button.gameObject.AddComponent<RectTransform>();
            }

            button.transform.SetParent(parent, false);
            return rt;
        }
    }
}
#endif