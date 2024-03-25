using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Author: Ironee

namespace com.IronicEntertainment.Editors.Tools
{
    public  static class CETools
    {
        public static void ScriptButtonField(MonoBehaviour monoBehaviour)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Script Path: ", EditorStyles.boldLabel);
            if (GUILayout.Button(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(monoBehaviour))), EditorStyles.miniButton))
            {
                MonoScript script = MonoScript.FromMonoBehaviour(monoBehaviour);
                AssetDatabase.OpenAsset(script);
            }
            EditorGUILayout.EndHorizontal();
        }


        public static void ScriptButtonField(ScriptableObject scriptableObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Script Path: ", EditorStyles.boldLabel);
            if (GUILayout.Button(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(scriptableObject))), EditorStyles.miniButton))
            {
                MonoScript script = MonoScript.FromScriptableObject(scriptableObject);
                AssetDatabase.OpenAsset(script);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
