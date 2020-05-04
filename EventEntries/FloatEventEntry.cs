using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FloatEventEntry : EventEntry<FloatEvent>
{
    private void Awake() {
        _event = new FloatEvent();    
        Debug.Log(_event);
    }

    public override void Invoke(InputAction.CallbackContext context)
    {
        _event.Invoke(context.ReadValue<float>());
    }
}
