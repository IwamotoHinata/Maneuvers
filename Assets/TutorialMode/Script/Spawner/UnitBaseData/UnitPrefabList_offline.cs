using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "For Tutorial/Create PrefabDataList")]
    public class UnitPrefabList_offline : ScriptableObject
    {
        public List<GameObject> list;
    }
}
