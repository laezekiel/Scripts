using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Data
{
    public class Health_Resource : Ironee_Resource
    {
        public Health_Resource(string filename = "", bool allnew = true) : base(new IR_Parameters(filename, allnew))
        {

        }

        private int _Max = 1;

        public enum Health_State
        {
            Full = 4,
            UnderFull = 3,
            UnderHalf = 2,
            UnderHeighth = 1,
            Depleted = 0,
        }

        private int _Points;
        private int _Temp = 0;
        private Health_State _State = Health_State.Full;

        public int Points { get => _Points; set { m_PropertiesG[nameof(Points)] = _Points = value; Export(); } }
        public int Max { get => _Max; set { m_PropertiesG[nameof(Max)] = _Max = value; Export(); } }
        public int Temp { get => _Temp; set { m_PropertiesG[nameof(Temp)] = _Temp = value; Export(); } }
        public Health_State State { get => _State; set { m_PropertiesG[nameof(State)] =_State = value; Export(); } }

        public void Init(int max)
        {
            Points = Max = max;
            Export();
        }

    }
}
