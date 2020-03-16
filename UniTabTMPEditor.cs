#if UNITY_EDITOR
using Worldreaver.EditorUtility;

namespace Worldreaver.UniUI
{
    [UnityEditor.CustomEditor(typeof(UniTapTMP), true)]
    public class UniTabTMPEditor : UniTabEditor
    {
        protected override void DrawInspector()
        {
            EditorUtil.DrawSeparator();
            EditorUtil.SerializeField(serializedObject, "pivot");
            EditorUtil.SerializeField(serializedObject, "text");
            EditorUtil.SerializeField(serializedObject, "activeObjects");
            EditorUtil.SerializeField(serializedObject, "deactiveObjects");
            Draw();
        }
    }
}
#endif