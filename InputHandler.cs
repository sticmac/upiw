using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using System.Linq;

public class InputHandler : MonoBehaviour
{

    [SerializeField] PlayerInput _playerInput;

    [SerializeField] List<EventEntry> _events;

    private void OnEnable() {
        _playerInput.onActionTriggered += HandleInput;
    }

    private void OnDisable() {
        _playerInput.onActionTriggered -= HandleInput;
    }

    private void HandleInput(InputAction.CallbackContext context) {
        _events.Where(e => e.ActionName == context.action.name)
            .Where(e => (e.RequiredState == RequiredState.Started && context.started)
                || (e.RequiredState == RequiredState.Performed && context.performed)
                || (e.RequiredState == RequiredState.Canceled && context.canceled)
                || e.RequiredState == RequiredState.Any)
            .ToList().ForEach(
                e => {
                    e.Invoke(context);
                });
    }
}
