using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

#if true
[CustomPropertyDrawer(typeof(EventEntry))]
public class EventEntryDrawer : PropertyDrawer
{
    private Rect _position;
    private const int _lineHeight = 16;
    private int _totalPropertyHeight = 0;
    private int _marginBetweenFields;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        _position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);
        _position.height = _lineHeight;
        _totalPropertyHeight = _lineHeight;

        _marginBetweenFields = (int)EditorGUIUtility.standardVerticalSpacing;

        //EditorGUI.BeginProperty(position, label, property);

        // Don't make child fields be indented
        DisplayPropertyField(property.FindPropertyRelative("requiredState"));
        DisplayPropertyField(property.FindPropertyRelative("actionName"));
        DisplayPropertyField(property.FindPropertyRelative("_eventArgumentType"));
        Debug.Log(property.FindPropertyRelative("_assignedEvent"));
        DisplayPropertyField(property.FindPropertyRelative("_assignedEvent"), true);

        //EditorGUI.EndProperty();
    }

    private void DisplayPropertyField(SerializedProperty property, bool spaceBelow = true) {
        EditorGUI.PropertyField(_position, property, true);
 
        if (spaceBelow)
        {
            AddToPositionY(_lineHeight + _marginBetweenFields);
        }
    }
 
    private void AddToPositionY(int addY) {
        _position.y += addY;
        _totalPropertyHeight += addY;
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return _totalPropertyHeight + _lineHeight / 2;
    }
}
#endif

/*[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{
    private SerializedProperty _playerInputProp;
    private SerializedProperty _eventsProp;
    private SerializedProperty _keyProp;

    private string[] _availableActionsNames;
    private int _selectedAction = 0;

    private void OnEnable() {
        _playerInputProp = serializedObject.FindProperty("_playerInput");
        _eventsProp = serializedObject.FindProperty("_events");
        _keyProp = serializedObject.FindProperty("_key");

        PlayerInput playerInput = _playerInputProp.objectReferenceValue as PlayerInput;
        _availableActionsNames = playerInput.actions.FindActionMap(playerInput.defaultActionMap, false).actions.Select(a => a.name).ToArray();
    }

    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
        /*serializedObject.Update();
        EditorGUILayout.LabelField("Input Asset", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_playerInputProp);

        if (_playerInputProp != null) {
            EditorGUILayout.Space(15);
            _selectedAction = EditorGUILayout.Popup("Action Name", _selectedAction, _availableActionsNames);

            EditorGUILayout.Space(15);
            EditorGUILayout.PropertyField(_eventsProp);
        }

        if (serializedObject.ApplyModifiedProperties()) {
            Repaint();
        }
    }
}*/
