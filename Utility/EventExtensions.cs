using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum EventArgumentType {
    None,
    Float
}

[Serializable]
public class FloatEvent : UnityEvent<float> {}

public static class EventFactory {
    public static UnityEvent GetEventWithArgumentType(EventArgumentType argumentType) {
        switch (argumentType) {
            case EventArgumentType.Float:
                return new UnityEvent();
            default:
                return new UnityEvent();
        }
    } 
}