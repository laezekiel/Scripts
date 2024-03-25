using com.IronicEntertainment.Editors.Data;
using com.IronicEntertainment.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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

        private bool _BaseStat = true;


        // Item base stats
        private EEKeyValue _EEKeyValue;
        private Sprite _Sprite = null;
        private string _Name = "";

        [MenuItem("Create/Item")]
        public static void ShowWindow()
        {
            ItemWindow window = GetWindow<ItemWindow>("Create An Item");

            window.minSize = new Vector2(720, 405);
            window.maxSize = new Vector2(1920, 1080);
        }

        public void OnGUI()
        {
            GUIStyle myWindowStyle = new GUIStyle(GUI.skin.window);

            myWindowStyle.fontSize = 14; 

            myWindowStyle.alignment = TextAnchor.MiddleCenter; 

            myWindowStyle.padding = new RectOffset(0, 5, 5, 10);
            


            if (_ObjectRarity == null || _ObjectsType == null)
            {
                EditorGUILayout.HelpBox("Enums are not correctly set to the script", MessageType.Warning);
                return;
            }
            GUILayout.Space(4);
            GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);

            EditorGUI.indentLevel += 1;

            _BaseStat = EditorGUILayout.Foldout(_BaseStat, "Common Base stats and similarities");

            if (_BaseStat)
            {
                myWindowStyle.margin = new RectOffset(30 * EditorGUI.indentLevel,5, 0,0);

                GUILayout.BeginVertical(myWindowStyle, GUILayout.MaxHeight(50));

                _Name = EditorGUILayout.TextField("Name ",_Name, GUILayout.MaxWidth(300));

                _RarityObject = EditorGUILayout.Popup("Object Rarity", _RarityObject, _ObjectRarity.Values, GUILayout.MaxWidth(300));

                _EEKeyValue = new EEKeyValue(_ObjectRarity,_RarityObject);


                EditorGUILayout.BeginHorizontal();

                _Sprite = (Sprite)EditorGUILayout.ObjectField("Icon", _Sprite, typeof(Sprite), false);

                DrawIcon();
                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            if (!_ObjectRarity.Keys.Contains(_RarityObject))
            {
                EditorGUILayout.HelpBox("Base are not set properly", MessageType.Warning);
            }


            GUILayout.Space(4);
            GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);


            EditorGUI.indentLevel++;
            GUILayout.Label("Select an object type to create :");
            GUILayout.Space(4);
            EditorGUI.indentLevel--;

            _TypeSelector = GUILayout.Toolbar(_TypeSelector, _ObjectsType.Values);

            EditorGUI.indentLevel++;

            myWindowStyle.margin = new RectOffset(5, 4, 0, 0);

            GUILayout.BeginVertical(myWindowStyle, GUILayout.MinHeight(1));

            Item_Resource newObject = CreateInstance<Item_Resource>();

            bool canCreate = true;


            switch (_TypeSelector)
            {
                case 0:
                    if(false)
                    {

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Type Scriptable Object not set.", MessageType.Warning);
                        canCreate = false;
                    }
                    break;
                case 1:
                    if(false)
                    {

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Type Scriptable Object not set.", MessageType.Warning);
                        canCreate = false;
                    }
                    break;
                case 2:
                    if(false)
                    {

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Type Scriptable Object not set.", MessageType.Warning);
                        canCreate = false;
                    }
                    break;
                case 3:
                    if(false)
                    {

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Type Scriptable Object not set.", MessageType.Warning);
                        canCreate = false;
                    }
                    break;
                case 4:

                    if(true)
                    {

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Type Scriptable Object not set.", MessageType.Warning);
                        canCreate = false;
                    }
                    break;
                default:
                    EditorGUILayout.HelpBox("Type Scriptable Object not set.", MessageType.Warning);
                    break;
            }
            GUILayout.EndVertical();

            GUILayout.Space(5);
            GUILayout.Box(new GUIContent(), GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);


            if (canCreate)
            {
                if (GUILayout.Button("Create", EditorStyles.miniButton, GUILayout.MaxWidth(100)))
                {
                    if (_Name != null) newObject.GetType().GetProperty("Name").SetValue(newObject, _Name);
                    if (_Sprite != null) newObject.GetType().GetProperty("Icon").SetValue(newObject, _Sprite);
                    if (_EEKeyValue.Enum == null)
                    {
                        Debug.Log(nameof(_EEKeyValue.Enum));
                        //newObject.GetType().GetProperty("TypeIndex").SetValueOptimized(newObject, _EEKeyValue);  
                    }
                    string path = EditorUtility.SaveFilePanelInProject("Save Item", _Name, "asset", "Enter a name for your item");
                    if (path != "")
                    {
                        AssetDatabase.CreateAsset(newObject, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Debug.Log("Item created at: " + path);
                    }
                }
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }

        public void DrawIcon()
        {
            if (_Sprite == null) return;

            Texture2D texture = AssetPreview.GetAssetPreview(_Sprite);
            GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }
    }

}
