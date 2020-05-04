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

    private Dictionary<string, Type> _argTypeToEventEntry = new Dictionary<string, Type>();
    private int _lastSelectedArgTypeIndex = 0;

    private void OnEnable() {
        _playerInputProp = serializedObject.FindProperty("_playerInput");
        _eventsProp = serializedObject.FindProperty("_events");

        PlayerInput playerInput = _playerInputProp.objectReferenceValue as PlayerInput;
        _availableActionsNames = playerInput.actions.FindActionMap(playerInput.defaultActionMap, false).actions.Select(a => a.name).ToArray();

        _eventsUnfolded = new bool[_eventsProp.arraySize];
        
        // Associate labels with event entries
        _argTypeToEventEntry.Clear();
        _argTypeToEventEntry.Add("None", typeof(UnityEventEntry));
        _argTypeToEventEntry.Add("Float", typeof(FloatEventEntry));
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.LabelField("Needed Assets", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_playerInputProp);
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("New Event", EditorStyles.boldLabel);
        _lastSelectedArgTypeIndex = EditorGUILayout.Popup("Argument Type", _lastSelectedArgTypeIndex, _argTypeToEventEntry.Keys.ToArray());
        if (GUILayout.Button("Create New Event")) {
            _eventsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newEventProp = _eventsProp.GetArrayElementAtIndex(0);

            string key = _argTypeToEventEntry.Keys.ToArray()[_lastSelectedArgTypeIndex];
            newEventProp.objectReferenceValue = ScriptableObject.CreateInstance(_argTypeToEventEntry[key]);

            SerializedObject so = new SerializedObject(newEventProp.objectReferenceValue);
            so.Update();
            so.FindProperty("_actionName").stringValue = _availableActionsNames[0];
            so.FindProperty("_requiredState").enumValueIndex = (int)RequiredState.Any;
            so.ApplyModifiedProperties();

            _eventsArrayUnfolded = true;
        }

        EditorGUILayout.Space(5);
        // Is the events foldout open?
        _eventsArrayUnfolded = EditorGUILayout.Foldout(_eventsArrayUnfolded, "Input Action Events", EditorStyles.foldoutHeader);

        if (_eventsArrayUnfolded) {
            Array.Resize(ref _eventsUnfolded, _eventsProp.arraySize);
            using (new EditorGUI.IndentLevelScope()) {

                EditorGUILayout.Space(10);
                for (int i = 0; i < _eventsProp.arraySize; i++) {
                    SerializedProperty property = _eventsProp.GetArrayElementAtIndex(i);
                    SerializedObject eventSo = new SerializedObject(property.objectReferenceValue);

                    SerializedProperty actionNameProp = eventSo.FindProperty("_actionName");
                    SerializedProperty requiredStateProp = eventSo.FindProperty("_requiredState");
                    if (!_eventsUnfolded[i]) {
                        DrawFoldout(i, $"{actionNameProp.stringValue} {requiredStateProp.enumDisplayNames[requiredStateProp.enumValueIndex]}");
                    } else {
                        EditorGUILayout.BeginVertical("Box");
                        DrawFoldout(i, $"{actionNameProp.stringValue} {requiredStateProp.enumDisplayNames[requiredStateProp.enumValueIndex]}");
                        EditorGUILayout.Space(10);
                        // Editing the actual event entry as a serialized object
                        eventSo.Update();

                        using (new EditorGUI.IndentLevelScope()) {
                            EditorGUILayout.Space(2);

                            // Name of action
                            int lastSelectedAction = Array.IndexOf(_availableActionsNames, actionNameProp.stringValue);
                            int selectedAction = EditorGUILayout.Popup("Action Name",
                                lastSelectedAction >= 0 ? lastSelectedAction : 0, _availableActionsNames);
                            actionNameProp.stringValue = _availableActionsNames[selectedAction];

                            // Event parameters
                            EditorGUILayout.PropertyField(eventSo.FindProperty("_requiredState"), true);

                            // Display unity event
                            EditorGUILayout.Space(5);
                            EditorGUILayout.PropertyField(eventSo.FindProperty("_event"), true);
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space(5);
                        eventSo.ApplyModifiedProperties();
                    }
                }
            }
        }

        if (serializedObject.ApplyModifiedProperties()) {
            Repaint();
        }
    }

    private void DrawFoldout(int i, string label) {
        using (new GUILayout.HorizontalScope())
        {
            _eventsUnfolded[i] = EditorGUILayout.Foldout(_eventsUnfolded[i], label);
            EditorGUILayout.Space(15);

            // Deletes the current event
            if (GUILayout.Button("Delete"))
            {
                _eventsProp.DeleteArrayElementAtIndex(i);
                _eventsProp.DeleteArrayElementAtIndex(i);
                _eventsUnfolded[i] = false;
            }
        }
    }
}
