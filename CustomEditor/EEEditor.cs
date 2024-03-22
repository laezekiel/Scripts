using com.IronicEntertainment.Editors.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using Unity.VisualScripting;

// Author: Ironee

namespace com.IronicEntertainment.Editors
{
    [CustomEditor(typeof(EditorEnum), editorForChildClasses : true)]
    public class EEEditor : Editor
    {
        EditorEnum _Enum;



        private void OnEnable()
        {
            _Enum = (EditorEnum)target;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle titleLabel = new GUIStyle(EditorStyles.largeLabel);
            GUIStyle styleLabel = new GUIStyle(EditorStyles.label);

            GUILayoutOption intFieldWidth = GUILayout.MaxWidth(30);
            GUILayoutOption buttonWidth = GUILayout.MaxWidth(20);
            GUILayoutOption labelWidth = GUILayout.MaxWidth(50);
            GUILayoutOption labelshortWidth = GUILayout.MaxWidth(25);

            serializedObject.Update();

            List<string> _L_S = new List<string>(_Enum.Size);
            List<int> _L_I = new List<int>(_Enum.Size);

            EditorGUILayout.LabelField("Keys and Value :", titleLabel);

            EditorGUILayout.Space(5f);

            SerializedProperty l_IntValue = serializedObject.FindProperty("_Enum_Values_I");
            SerializedProperty l_StringValue = serializedObject.FindProperty("_Enum_Values_S");

            EditorGUILayout.BeginVertical();
            
            for (int i = 0; i < _Enum.Size; i++)
            {
                _L_S.Add("");
                _L_I.Add(i);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Id :", styleLabel, labelshortWidth);
                _L_I[i] = EditorGUILayout.IntField(l_IntValue.GetArrayElementAtIndex(i).intValue, intFieldWidth);

                EditorGUILayout.LabelField("Value :", styleLabel, labelWidth);
                _L_S[i] = EditorGUILayout.TextField(l_StringValue.GetArrayElementAtIndex(i).stringValue);

                EditorGUILayout.EndHorizontal();
            }

            l_IntValue.SetUnderlyingValue(_L_I);
            l_StringValue.SetUnderlyingValue(_L_S);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5f);

            ButtonPM(buttonWidth);
            
            serializedObject.ApplyModifiedProperties();
        }





        public void ButtonPM(GUILayoutOption buttonWidth)
        {
            EditorGUILayout.BeginHorizontal();


            GUILayout.FlexibleSpace();

            if (GUILayout.Button("+", buttonWidth))
            {
                _Enum.Add();
            }

            if (GUILayout.Button("-", buttonWidth))
            {
                _Enum.RemoveAt(_Enum.Size - 1);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
