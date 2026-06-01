using System;
using UnityEngine;

public class InputManager
{
    public Action keyAction = null;
    
    public void OnUpdate()
    {
        if(!Input.anyKey) { return; }

        keyAction?.Invoke();
    }
}
