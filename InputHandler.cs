using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Linq;

[Serializable]
public enum RequiredState {
    Any,
    Started,
    Performed,
    Canceled
}

[Serializable]
public class EventEntry {

    [SerializeField] RequiredState _requiredState;
    [SerializeField] string _actionName;
    [SerializeField] EventArgumentType _eventArgumentType;
    [SerializeField] UnityEvent _unityEvent;
    [SerializeField] FloatEvent _floatEvent;

    public EventEntry(string actionName, RequiredState requiredState, EventArgumentType eventArgumentType) {
        this._requiredState = requiredState;
        this._actionName = actionName;
        this._eventArgumentType = eventArgumentType;
    }

    public RequiredState RequiredState => _requiredState;
    public string ActionName => _actionName;
    public EventArgumentType EventArgumentType => _eventArgumentType;
    public UnityEventBase Event {
        get {
            if (_eventArgumentType == EventArgumentType.Float) {
                return _floatEvent;
            } else {
                return _unityEvent;
            }
        }
    }
}

public class InputHandler : MonoBehaviour
{

    [SerializeField] PlayerInput _playerInput;

    [SerializeField] EventEntry[] _events;

    private void OnEnable() {
        _playerInput.onActionTriggered += HandleInput;
    }

    private void OnDisable() {
        _playerInput.onActionTriggered -= HandleInput;
    }

    private void HandleInput(InputAction.CallbackContext context) {
        _events.Where(e => e.ActionName == context.action.name).ToList().ForEach(
            e => {
                if ((e.RequiredState == RequiredState.Started && context.started)
                    || (e.RequiredState == RequiredState.Performed && context.performed)
                    || (e.RequiredState == RequiredState.Canceled && context.canceled)
                    || e.RequiredState == RequiredState.Any) {
                        switch(e.EventArgumentType) {
                            case (EventArgumentType.Float):
                                ((FloatEvent)e.Event)?.Invoke(context.ReadValue<float>());
                                break;
                            default:
                                ((UnityEvent)e.Event)?.Invoke();
                                break;
                        }
                    }
            }
        );
    }
}
