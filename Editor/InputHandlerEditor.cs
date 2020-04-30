using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{
    private SerializedProperty _playerInputProp;
    private SerializedProperty _eventsProp;

    private string[] _availableActionsNames;

    // Foldouts
    private bool _eventsArrayUnfolded = false;
    private bool[] _eventsUnfolded;


    private void OnEnable() {
        _playerInputProp = serializedObject.FindProperty("_playerInput");
        _eventsProp = serializedObject.FindProperty("_events");

        PlayerInput playerInput = _playerInputProp.objectReferenceValue as PlayerInput;
        _availableActionsNames = playerInput.actions.FindActionMap(playerInput.defaultActionMap, false).actions.Select(a => a.name).ToArray();

        _eventsUnfolded = new bool[_eventsProp.arraySize];
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.LabelField("Needed Assets", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_playerInputProp);
        EditorGUILayout.Space(15);

        // Is the events foldout open?
        _eventsArrayUnfolded = EditorGUILayout.Foldout(_eventsArrayUnfolded, "Input Action Events", EditorStyles.foldoutHeader);

        if (_eventsArrayUnfolded) {
            using (new EditorGUI.IndentLevelScope()) {
                // Size of the events array
                _eventsProp.arraySize = EditorGUILayout.IntField("Size", _eventsProp.arraySize);
                Array.Resize(ref _eventsUnfolded, _eventsProp.arraySize);

                EditorGUILayout.Space(10);
                for (int i = 0; i < _eventsProp.arraySize; i++) {
                    // Space in-between events
                    if (i != 0) {
                        EditorGUILayout.Space(5);
                    }
                    SerializedProperty property = _eventsProp.GetArrayElementAtIndex(i);

                    SerializedProperty actionName = property.FindPropertyRelative("actionName");

                    //EditorGUILayout.LabelField("Event: " + actionName.stringValue);
                    _eventsUnfolded[i] = EditorGUILayout.Foldout(_eventsUnfolded[i], "Action: " + actionName.stringValue);
                    if (_eventsUnfolded[i]) {
                        using (new EditorGUI.IndentLevelScope()) {
                            EditorGUILayout.Space(2);

                            // Name of action
                            int selectedAction = EditorGUILayout.Popup("Action Name",
                                Array.IndexOf(_availableActionsNames, actionName.stringValue), _availableActionsNames);
                            actionName.stringValue = _availableActionsNames[selectedAction];

                            // Event parameters
                            EditorGUILayout.PropertyField(property.FindPropertyRelative("requiredState"), true);
                            EditorGUILayout.PropertyField(property.FindPropertyRelative("_eventArgumentType"), true);

                            // Display unity event
                            switch ((EventArgumentType)property.FindPropertyRelative("_eventArgumentType").enumValueIndex) {
                                case EventArgumentType.Float:
                                    EditorGUILayout.PropertyField(property.FindPropertyRelative("_floatEvent"), true);
                                    break;
                                default:
                                    EditorGUILayout.PropertyField(property.FindPropertyRelative("_unityEvent"), true);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        if (serializedObject.ApplyModifiedProperties()) {
            Repaint();
        }
    }
}
