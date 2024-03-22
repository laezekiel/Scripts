using com.IronicEntertainment.Editors.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Author: Ironee

namespace com.IronicEntertainment.Editors.Windows
{
    public class ItemWindow : EditorWindow
    {
        [SerializeField] private EditorEnum _ObjectsType;
        [SerializeField] private EditorEnum _ObjectRarity;

        private int _TypeSelector;
        private int _RarityObject;

        private bool _BaseStat = false;

        [MenuItem("Create/Item")]
        public static void ShowWindow()
        {
            ItemWindow window = GetWindow<ItemWindow>("Create An Item");

            window.minSize = new Vector2(720, 405);
            window.maxSize = new Vector2(1920, 1080);
        }

        public void OnGUI()
        {
            if(_ObjectRarity == null || _ObjectsType == null)
            {
                EditorGUILayout.HelpBox("Enums are not correctly set to the script", MessageType.Warning);
                return;
            }

            _BaseStat = EditorGUILayout.Foldout(_BaseStat, "Common Base stats and similarities");

            if (_BaseStat) 
            {
                EditorGUI.indentLevel++;
                _RarityObject = EditorGUILayout.Popup("Object Type", _RarityObject, _ObjectRarity.Values, GUILayout.MaxWidth(300));
                EditorGUI.indentLevel--;
            }

            if (!_ObjectRarity.Keys.Contains(_RarityObject))
            {
                EditorGUILayout.HelpBox("Base are not set properly", MessageType.Warning);
            }

            GUILayout.Space(10);

            GUILayout.Label("Select an object type to create");
            _TypeSelector = GUILayout.Toolbar(_TypeSelector, _ObjectsType.Values);

            EditorGUI.indentLevel++;

            switch (_TypeSelector)
            {
                default:
                    GUILayout.Label("Type not prepared yet");
                    break;
            }

            EditorGUI.indentLevel--;
        }
    }
}
