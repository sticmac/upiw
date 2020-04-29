using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using NaughtyAttributes;

[Serializable]
public enum RequiredState {
    Any,
    Started,
    Performed,
    Canceled
}

[Serializable]
public struct EventEntry {

    public RequiredState requiredState;
    public string actionName;

    public UnityEventBase AssignedEvent {
        get => _assignedEvent;
    }

    public EventArgumentType ArgumentType {
        get => _eventArgumentType;
        set {
            _eventArgumentType = value;
            _assignedEvent = EventFactory.GetEventWithArgumentType(_eventArgumentType); 
        }
    }

    [SerializeField] EventArgumentType _eventArgumentType;
    [SerializeField] UnityEvent _assignedEvent;

    public EventEntry(string actionName, RequiredState requiredState, EventArgumentType eventArgumentType) {
        this.requiredState = requiredState;
        this.actionName = actionName;
        this._eventArgumentType = eventArgumentType;
        this._assignedEvent = EventFactory.GetEventWithArgumentType(eventArgumentType);
        ArgumentType = eventArgumentType;
    }
}

public class InputHandler : MonoBehaviour
{

    [SerializeField] PlayerInput _playerInput;

    public UnityEvent OnJump;
    public UnityEvent OnStoppedJump;
    public FloatEvent OnMove;


    [SerializeField] EventEntry[] _events;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput.onActionTriggered += (ctx) => {
            if (ctx.action.name.Equals("Jump")) {
                if (ctx.started) {
                    OnJump?.Invoke();
                } else if (ctx.canceled) {
                    OnStoppedJump?.Invoke();
                }
            } else if (ctx.action.name.Equals("Move")) {
                OnMove?.Invoke(ctx.ReadValue<float>());
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
