using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class EventTrigger_offline : MonoBehaviour
    {
        [SerializeField] private SceneManager1_offline _SceneManager;
        [SerializeField] private int _eventNo;

        void OnTriggerEnter(Collider target)
        {
            if (!target.gameObject.TryGetComponent<UnitBaseData_offline>(out var targetUnitBaseData))
                return;

            if (targetUnitBaseData.isHasInputAuthority())
            {
                _SceneManager.SetGameEvent(_eventNo);
                Destroy(gameObject);
            }
        }
    }
}
