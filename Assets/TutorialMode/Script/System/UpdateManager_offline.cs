using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UpdateManager_offline : MonoBehaviour
    {
        public List<IUpdated> upd = new List<IUpdated>(); //Update
        public List<IFixedUpdated> fix = new List<IFixedUpdated>(); //FixedUpdate
        public List<ILateUpdated> late = new List<ILateUpdated>(); //LateUpdate

        void Update()
        {
            for (int i = 0; i < upd.Count; i++)
            {
                if (upd[i].UpdateRequest()) upd[i].Updated();
                else upd.RemoveAt(i);
            }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < fix.Count; i++)
            {
                if (fix[i].UpdateRequest()) fix[i].FixedUpdated();
                else fix.RemoveAt(i);
            }
        }

        void LateUpdate()
        {
            for (int i = 0; i < late.Count; i++)
            {
                if (late[i].UpdateRequest()) late[i].LateUpdated();
                else late.RemoveAt(i);
            }
        }
    }
}
