using com.IronicEntertainment.Editors.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.IO;
using com.IronicEntertainment.Editors.Tools;

// Author: Ironee

namespace com.IronicEntertainment.Editors
{

    [CustomEditor(typeof(EditorEnum), editorForChildClasses : true)]
    public class EEEditor : Editor
    {
        EditorEnum _Enum;

        bool _Extensions;

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

            CETools.ScriptButtonField(_Enum);


            List<string> _L_S = new List<string>();
            List<int> _L_I = new List<int>();
            List<string> _L_T = new List<string>();

            EditorGUILayout.LabelField("Keys and Value :", titleLabel);

            EditorGUILayout.Space(5f);

            SerializedProperty l_IntValue = serializedObject.FindProperty("_Enum_Values_I");
            SerializedProperty l_StringValue = serializedObject.FindProperty("_Enum_Values_S");
            SerializedProperty l_TypeValue = serializedObject.FindPropertyOrFail("_Enum_Values_T");
            SerializedProperty l_ToType = serializedObject.FindPropertyOrFail("_ToType");

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

            serializedObject.Update();

            EditorGUILayout.Space(5f);

            _Extensions = EditorGUILayout.Foldout(_Extensions, "Extensions");

            if (_Extensions)
            {
                l_ToType.boolValue = EditorGUILayout.ToggleLeft("To Type",l_ToType.boolValue);

                if (l_ToType.boolValue)
                {
                    for (int i = 0; i < _Enum.Size; i++)
                    {
                        _L_T.Add("");
                        EditorGUI.indentLevel++;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"Id : ({l_IntValue.GetArrayElementAtIndex(i).intValue}, {l_StringValue.GetArrayElementAtIndex(i).stringValue})", styleLabel, GUILayout.MaxWidth(150));
                        _L_T[i] = EditorGUILayout.TextField(l_TypeValue.GetArrayElementAtIndex(i).stringValue);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.EndHorizontal();
                    }
                    l_TypeValue.SetUnderlyingValue(_L_T);
                }

            }
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
