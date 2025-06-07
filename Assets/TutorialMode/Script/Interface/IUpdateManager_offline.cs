using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public interface IUpdateManager_offline
    {
        bool GetActive();
        void Updated();
    }
}
