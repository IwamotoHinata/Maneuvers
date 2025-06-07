using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMessageKeyEvent : MonoBehaviour
{
    public abstract void Event(int key);
    public abstract void EffectEvent(int key, Action action, Action action2);
}
