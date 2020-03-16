#if UNITY_EDITOR
using Worldreaver.EditorUtility;

namespace Worldreaver.UniUI
{
    [UnityEditor.CustomEditor(typeof(UniButtonTMP), true)]
    [UnityEditor.CanEditMultipleObjects]
    public class UniButtonTMPEditor : UniButtonEditor
    {
        protected override void DrawInspector()
        {
            EditorUtil.DrawSeparator();
            EditorUtil.SerializeField(serializedObject, "pivot");
            EditorUtil.SerializeField(serializedObject, "text");
            Draw();
        }
    }
}
#endif