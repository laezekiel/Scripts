using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEditor;
using UnityEngine;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Data
{
    [Serializable]
    public class Health_Resource : ScriptableObject
    {
        [SerializeField] private int _Max = 1;

        public enum Health_State
        {
            Full = 4,
            UnderFull = 3,
            UnderHalf = 2,
            UnderHeighth = 1,
            Depleted = 0,
        }

        private int _Points = 0;
        private int _Temp = 0;
        private Health_State _State = Health_State.Full;

        public int Points { get => _Points; set => _Points = value; }
        public int Max { get => _Max; set => _Max = value; }
        public int Temp { get => _Temp; set => _Temp = value; }
        public Health_State State { get => _State; set => _State = value; }

        public void Init(int max = 0)
        {
            if (max > 0) Points = Max = max;
            Points = Max;
        }

    }
}
