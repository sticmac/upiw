using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vector2EventEntry : EventEntry<Vector2Event>
{
    private void Awake() {
        if (_event == null) {
            _event = new Vector2Event();    
        }
    }
    public override void Invoke(InputAction.CallbackContext context)
    {
        _event.Invoke(context.ReadValue<Vector2>());
    }
}
