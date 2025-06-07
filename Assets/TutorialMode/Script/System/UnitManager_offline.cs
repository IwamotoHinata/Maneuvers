using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitManager_offline : MonoBehaviour
    {
        public List<GameObject> playerVisible { get; set; } = new List<GameObject>();
        public List<GameObject> enemyVisible { get; set; } = new List<GameObject>();
    }
}
