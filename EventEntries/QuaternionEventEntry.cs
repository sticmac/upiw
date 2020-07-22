using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuaternionEventEntry : EventEntry<QuaternionEvent>
{
    private void Awake() {
        if (_event == null) {
            _event = new QuaternionEvent();    
        }
    }
    public override void Invoke(InputAction.CallbackContext context)
    {
        _event.Invoke(context.ReadValue<Quaternion>());
    }
}
