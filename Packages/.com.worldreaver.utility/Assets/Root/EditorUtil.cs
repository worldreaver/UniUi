#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Worldreaver.EditorUtility
{
    public static partial class EditorUtil
    {
        /// <summary>
        /// add define symbols <paramref name="symbols"/> in to player setting define symbols
        /// </summary>
        /// <param name="symbols"></param>
        public static void AddDefineSymbols(params string[] symbols)
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();
            allDefines.AddRange(symbols.Except(allDefines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        /// <summary>
        /// remove define symbols <paramref name="symbols"/> from player setting define symbols
        /// </summary>
        /// <param name="symbols"></param>
        public static void RemoveDefineSymbols(params string[] symbols)
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();
            foreach (var item in symbols.Except(allDefines))
            {
                allDefines.Remove(item);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedProperty"></param>
        /// <param name="properties"></param>
        public static void SerializeFields(SerializedProperty serializedProperty, params string[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var item = serializedProperty.FindPropertyRelative(properties[i]);
                EditorGUILayout.PropertyField(item, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="properties"></param>
        public static void SerializeFields(SerializedObject serializedObject, params string[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var item = serializedObject.FindProperty(properties[i]);
                EditorGUILayout.PropertyField(item, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="displayName"></param>
        /// <param name="includeChild"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SerializedProperty SerializeField(SerializedObject serializedObject, string propertyName, string displayName = null, bool includeChild = true, params GUILayoutOption[] options)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                Debug.Log("Not found serializedProperty " + propertyName);
                return null;
            }

            if (!property.isArray)
            {
                EditorGUILayout.PropertyField(property, new GUIContent(string.IsNullOrEmpty(displayName) ? property.displayName : displayName), includeChild);
                return property;
            }

            if (property.isExpanded)
                EditorGUILayout.PropertyField(property, includeChild, options);
            else
                EditorGUILayout.PropertyField(property, new GUIContent(property.displayName), includeChild, options);
            return property;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool DrawHeader(string text)
        {
            var key = text;
            var state = EditorPrefs.GetBool(key, true);

            GUILayout.Space(3f);
            if (!state)
            {
                GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            }

            GUILayout.BeginHorizontal();
            GUI.changed = false;


            text = "<b><size=11>" + text + "</size></b>";
            if (state)
            {
                text = "\u25BC " + text;
            }
            else
            {
                text = "\u25BA " + text;
            }

            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f)))
            {
                state = !state;
            }

            if (GUI.changed)
            {
                EditorPrefs.SetBool(key, state);
                state = EditorPrefs.GetBool(text, true);
            }

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!state)
            {
                GUILayout.Space(3f);
            }

            return state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        public static void DrawSeparator(float height = 1f)
        {
            GUILayout.Space(2);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));
            GUILayout.Space(2);
        }

        /// <summary>
        /// draw ui line
        /// reference: @alexanderameye
        /// link: https://forum.unity.com/threads/horizontal-line-in-editor-window.520812/
        /// </summary>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        /// <param name="padding"></param>
        public static void DrawUiLine(Color color, int thickness = 2, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2f;
            r.x -= 2f;
            r.width += 6f;
            EditorGUI.DrawRect(r, color);
        }
    }
    
    public static class TypeUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static bool Exist(string assemblyName)
        {
            return System.AppDomain.CurrentDomain
                       .GetAssemblies()
                       .FirstOrDefault(assembly => assembly.GetName().Name.Equals(assemblyName)) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static System.Type GetTypeByName(string className)
        {
            return System.AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.Name == className);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static System.Type GetTypeByName(string className, string assemblyName)
        {
            return System.Reflection.Assembly
                .Load(assemblyName)
                .GetTypes()
                .FirstOrDefault(type => type.Name == className);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetAllImplementFormType<T>() where T : class
        {
            var allImplements = new System.Collections.Generic.List<string>();

            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = assembly.GetTypes()
                    .Where(t => t != typeof(T))
                    .Where(t => typeof(T).IsAssignableFrom(t))
                    .Select(t => t.Name);
                allImplements.AddRange(types);
            }

            return allImplements.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetAllImplementFormType<T>(string assemblyName) where T : class
        {
            var allImplements = new System.Collections.Generic.List<string>();
            var types = System.Reflection.Assembly.Load(assemblyName)
                .GetTypes()
                .Where(t => t != typeof(T))
                .Where(t => typeof(T).IsAssignableFrom(t))
                .Select(t => t.Name);
            allImplements.AddRange(types);
            return allImplements.ToArray();
        }
    }
}
#endif