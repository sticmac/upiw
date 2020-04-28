using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

public class InputHandler : MonoBehaviour
{

    [SerializeField] PlayerInput _playerInput;

    public UnityEvent OnJump;
    public UnityEvent OnStoppedJump;
    public FloatEvent OnMove;


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
