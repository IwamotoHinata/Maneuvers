using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "For Tutorial/Create MapDataList")]
    public class MapDataList_offline : ScriptableObject
    {
        public List<MapData_offline> list;
    }
}