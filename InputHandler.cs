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

    public RequiredState requiredState;
    public string actionName;

    [SerializeField] EventArgumentType _eventArgumentType;

    [SerializeField] UnityEvent _unityEvent;
    [SerializeField] FloatEvent _floatEvent;

    public EventEntry(string actionName, RequiredState requiredState, EventArgumentType eventArgumentType) {
        this.requiredState = requiredState;
        this.actionName = actionName;
        this._eventArgumentType = eventArgumentType;
    }
}

public class InputHandler : MonoBehaviour
{

    [SerializeField] PlayerInput _playerInput;

    [SerializeField] EventEntry[] _events;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
