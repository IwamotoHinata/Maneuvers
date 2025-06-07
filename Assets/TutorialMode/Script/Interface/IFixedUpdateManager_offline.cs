using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public interface IFixedUpdateManager_offline
    {
        bool GetActive();
        void FixedUpdated();
    }
}