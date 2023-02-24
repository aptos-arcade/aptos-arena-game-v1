using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public KeyCode Key { get; private set; }

    public Command(KeyCode key)
    {
        this.Key = key;
    }

    public virtual void GetKeyDown()
    {

    }

    public virtual void GetKeyUp()
    {

    }

    public virtual void GetKey()
    {

    }
}
