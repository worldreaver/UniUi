using System;
 using System.Diagnostics;
 using UnityEditor;
 
 namespace Worldreaver.Utility
 {
     public static class Log
     {
         private const string ENABLE_LOG = "ENABLE_LOG"; // flag on of log
 
 #if UNITY_EDITOR
 #if ENABLE_LOG
         [MenuItem("Window/Worldreaver/Utility/Disable Log")]
         private static void DisableLog()
         {
             EditorUtility.EditorUtil.RemoveDefineSymbols(ENABLE_LOG);            
         }
 #else
         [MenuItem("Window/Worldreaver/Utility/Enable Log")]
         private static void EnableLog()
         {
             EditorUtility.EditorUtil.AddDefineSymbols(ENABLE_LOG);
         }
 #endif
 #endif
         /// <summary>
         ///  UnityEngine.Debug.LogFormat(string format, params object[] args)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="format"></param>
         /// <param name="args"></param>
         [Conditional(ENABLE_LOG)]
         public static void Info(string header, string format, params object[] args)
         {
             UnityEngine.Debug.LogFormat(header + format, args);
         }
 
         /// <summary>
         /// UnityEngine.Debug.Log(object message)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="message"></param>
         [Conditional(ENABLE_LOG)]
         public static void Info(string header, object message)
         {
             UnityEngine.Debug.Log(header + message);
         }
 
         /// <summary>
         /// UnityEngine.Debug.LogWarningFormat(string format, params object[] args)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="format"></param>
         /// <param name="args"></param>
         [Conditional(ENABLE_LOG)]
         public static void Warning(string header, string format, params object[] args)
         {
             UnityEngine.Debug.LogWarningFormat(header + format, args);
         }
 
         /// <summary>
         /// UnityEngine.Debug.LogWarning(object message)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="message"></param>
         [Conditional(ENABLE_LOG)]
         public static void Warning(string header, object message)
         {
             UnityEngine.Debug.LogWarning(header + message);
         }
 
         /// <summary>
         /// UnityEngine.Debug.LogErrorFormat(string format, params object[] args)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="format"></param>
         /// <param name="args"></param>
         public static void Error(string header, string format, params object[] args)
         {
             UnityEngine.Debug.LogErrorFormat(header + format, args);
         }
 
         /// <summary>
         /// UnityEngine.Debug.LogError(object message)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="message"></param>
         public static void Error(string header, object message)
         {
             UnityEngine.Debug.LogError(header + message);
         }
 
         /// <summary>
         /// UnityEngine.Debug.LogException(Exception exception)
         /// </summary>
         /// <param name="header"></param>
         /// <param name="e"></param>
         public static void Exception(string header, Exception e)
         {
             UnityEngine.Debug.LogException(new Exception(header + e.Message, e.InnerException));
         }
     }
 }