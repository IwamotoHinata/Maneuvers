using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageKey
{
    private int key;
    public int Key
    {
        get => key;
    }

    public MessageKey(int key = 0)
    {
        this.key = key;
    }

    public void Next()
    {
        this.key += 1;
    }

    public void Prev()
    {
        this.key -= 1;
    }
}
