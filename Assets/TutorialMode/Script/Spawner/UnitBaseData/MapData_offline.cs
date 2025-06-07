using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "For Tutorial/Create MapData")]
    public class MapData_offline : ScriptableObject
    {
        public List<Vector3> pos;
    }
}
