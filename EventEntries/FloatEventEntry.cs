using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FloatEventEntry : EventEntry<FloatEvent>
{
    private void Awake() {
        if (_event == null) {
            _event = new FloatEvent();    
        }
    }

    public override void Invoke(InputAction.CallbackContext context)
    {
        _event.Invoke(context.ReadValue<float>());
    }
}
