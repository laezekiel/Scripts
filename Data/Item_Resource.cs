using com.IronicEntertainment.Editors.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.IronicEntertainment.Scripts.Data
{
    [CreateAssetMenu(menuName = "Item/Misc.", fileName = "New Item")]
    public class Item_Resource : ScriptableObject
    {
        [SerializeField] private string _Name;
        [SerializeField] private Sprite _Icon;
        [SerializeField] private EEKeyValue _TypeIndex;

        public string Name { get { return _Name; } set { _Name = value; } }
        public Sprite Icon { get { return _Icon; } set { _Icon = value; } }
        public EEKeyValue Type { get { return _TypeIndex; } set { _TypeIndex = value; } }
    }
}
