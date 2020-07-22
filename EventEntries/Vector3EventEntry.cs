using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vector3EventEntry : EventEntry<Vector3Event>
{
    private void Awake() {
        if (_event == null) {
            _event = new Vector3Event();    
        }
    }
    public override void Invoke(InputAction.CallbackContext context)
    {
        _event.Invoke(context.ReadValue<Vector3>());
    }
}
