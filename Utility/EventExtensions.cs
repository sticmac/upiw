﻿using System;
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