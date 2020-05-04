using System;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable]
public class UnityEventEntry : EventEntry<UnityEvent>
{
    private void Awake() {
        _event = new UnityEvent();
    }

    public override void Invoke(InputAction.CallbackContext context)
    {
        _event.Invoke();
    }
}
