using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Ironee

namespace com.IronicEntertainment.Editors.Data
{
    [Serializable]
    public struct EEKeyValue
    {
        [SerializeField] private EditorEnum _Enum;

        [SerializeField] private int _Index;

        public EEKeyValue(EditorEnum pEnum, int pIndex)
        {
            _Enum = pEnum;

            _Index = pIndex;
        }


        public int Index { get => _Index; set => _Index = value; }

        public EditorEnum Enum { get => _Enum; }

        public string Value { get => _Enum.Values[_Index]; }
        public int Key { get => _Enum.Keys[_Index]; }

    }

    [CreateAssetMenu(menuName = "Tools/Dynamic Enum", fileName = "New Dynamic Enum")]
    [Serializable]
    public class EditorEnum : ScriptableObject
    {
        [SerializeField]
        private List<int> _Enum_Values_I = new List<int>() { 0, 1 };

        [SerializeField]
        private List<string> _Enum_Values_S = new List<string>() { "A",  "B" };

        public string Value(int key)
        {
            try
            {
                return _Enum_Values_S[_Enum_Values_I.IndexOf(key)];
            }
            catch
            {
                Debug.Log("The key is not in the enum");
                return "";
            }
        }

        public int Key(string value)
        {
            try
            {
                return _Enum_Values_I[_Enum_Values_S.IndexOf(value)];
            }
            catch
            {
                Debug.Log("The value is not in the enum");
                return -1;
            }
        }

        public string[] Values
        {
            get
            {
                return _Enum_Values_S.ToArray();
            }
        }

        public int[] Keys
        {
            get
            {
                return _Enum_Values_I.ToArray();
            }
        }

        public int Size { get => _Enum_Values_I.Count; }


        public void Add()
        {
            int lKey = 0;
            int lKeyV = 0;

            int lVid = 0;
            string lValue; 
            
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";

            

            while (_Enum_Values_I.Contains(lKey))
            {
                lKey++;
            }


            lKeyV = lKey;


            while (lKeyV >= 0)
            {
                result = letters[lKeyV % 26] + result;
                lKeyV = (lKeyV / 26) - 1;
            }


            lValue= result;

            while (_Enum_Values_S.Contains(lValue))
            {
                lVid++;
                lValue = result + "_" + lVid;
            }

            _Enum_Values_I.Add(lKey);
            _Enum_Values_S.Add(lValue);
        }


        public void RemoveAt(int pAt)
        {
            _Enum_Values_I.RemoveAt(pAt);
            _Enum_Values_S.RemoveAt(pAt);
        }


        public void Remove(string pValue)
        {
            _Enum_Values_I.RemoveAt(_Enum_Values_S.IndexOf(pValue));
            _Enum_Values_S.Remove(pValue);
        }
    }
}
