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

    private EventArgumentType _lastSelectedArgType = EventArgumentType.None;


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

        using (new GUILayout.HorizontalScope()) {
            _lastSelectedArgType = (EventArgumentType)EditorGUILayout.EnumPopup(_lastSelectedArgType);
            if (GUILayout.Button("+")) {
                _eventsProp.InsertArrayElementAtIndex(0);
                SerializedProperty newEventProp = _eventsProp.GetArrayElementAtIndex(0);
                switch (_lastSelectedArgType) {
                    case EventArgumentType.Float:
                        newEventProp.objectReferenceValue = ScriptableObject.CreateInstance<FloatEventEntry>();
                        break;
                    default:
                        newEventProp.objectReferenceValue = ScriptableObject.CreateInstance<UnityEventEntry>();
                        break;
                }
                SerializedObject so = new SerializedObject(newEventProp.objectReferenceValue);
                so.Update();
                so.FindProperty("_actionName").stringValue = _availableActionsNames[0];
                so.FindProperty("_requiredState").enumValueIndex = (int)RequiredState.Any;
                so.FindProperty("_eventArgumentType").enumValueIndex = (int)_lastSelectedArgType;
                so.ApplyModifiedProperties();

                _eventsArrayUnfolded = true;
            }
        }

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
                    SerializedProperty property = _eventsProp.GetArrayElementAtIndex(i);
                    SerializedObject eventSo = new SerializedObject(property.objectReferenceValue);

                    eventSo.Update();
                    SerializedProperty actionNameProp = eventSo.FindProperty("_actionName");
                    _eventsUnfolded[i] = EditorGUILayout.Foldout(_eventsUnfolded[i], "Action: " + actionNameProp.stringValue);
                    if (_eventsUnfolded[i]) {
                        using (new EditorGUI.IndentLevelScope()) {
                            EditorGUILayout.Space(2);

                            // Name of action
                            int lastSelectedAction = Array.IndexOf(_availableActionsNames, actionNameProp.stringValue);
                            int selectedAction = EditorGUILayout.Popup("Action Name",
                                lastSelectedAction >= 0 ? lastSelectedAction : 0, _availableActionsNames);
                            actionNameProp.stringValue = _availableActionsNames[selectedAction];

                            // Event parameters
                            EditorGUILayout.PropertyField(eventSo.FindProperty("_requiredState"), true);
                            SerializedProperty eventArgumentTypeProp = eventSo.FindProperty("_eventArgumentType");
                            EditorGUILayout.PropertyField(eventArgumentTypeProp, true);

                            // Display unity event
                            SerializedProperty eventProp = eventSo.FindProperty("_event");
                            EditorGUILayout.PropertyField(eventSo.FindProperty("_event"), true);
                        }
                    }
                    eventSo.ApplyModifiedProperties();
                }
            }
        }

        if (serializedObject.ApplyModifiedProperties()) {
            Repaint();
        }
    }
}
