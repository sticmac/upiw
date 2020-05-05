using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatEvent : UnityEvent<float> {}

[Serializable]
public class Vector2Event : UnityEvent<Vector2> {}