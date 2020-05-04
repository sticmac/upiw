using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public enum RequiredState {
    Any,
    Started,
    Performed,
    Canceled
}

[Serializable]
public abstract class EventEntry : ScriptableObject {

    [SerializeField] protected RequiredState _requiredState;
    [SerializeField] protected string _actionName;

    public RequiredState RequiredState => _requiredState;
    public string ActionName => _actionName;

    public abstract void Invoke(InputAction.CallbackContext context);
}

[Serializable]
public abstract class EventEntry<T> : EventEntry where T : UnityEventBase {
    [SerializeField] protected T _event;
}
