using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "For Tutorial/Create UnitDataList")]
    public class UnitDataList_offline : ScriptableObject
    {
        public List<UnitData_offline> list;
    }
}
