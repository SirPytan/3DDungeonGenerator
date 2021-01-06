using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AdjacentTile))]
public class AdjacentTilePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        label.text = "Orientation";

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect firstOptionRect = new Rect(position.x, position.y, (position.width * 0.25f), position.height);
        Rect secondOptionRect = new Rect(position.x + (position.width * 0.25f), position.y, position.width * 0.25f, position.height);
        Rect thirdOptionRect = new Rect(position.x + (position.width * 0.5f), position.y, position.width * 0.25f, position.height);
        Rect fourthOptionRect = new Rect(position.x + (position.width * 0.75f), position.y, position.width * 0.25f, position.height);

        SerializedProperty zeroProperty = property.FindPropertyRelative("m_ZeroDegreeToParent");
        SerializedProperty minus90Property = property.FindPropertyRelative("m_Minus90DegreeToParent");
        SerializedProperty plus90Property = property.FindPropertyRelative("m_Plus90DegreeToParent");
        SerializedProperty plus180Property = property.FindPropertyRelative("m_Plus180DegreeToParent");

        EditorGUIUtility.labelWidth = 15f;
        EditorGUI.PropertyField(firstOptionRect, zeroProperty, new GUIContent("0°"));
        EditorGUIUtility.labelWidth = 30f;
        EditorGUI.PropertyField(secondOptionRect, minus90Property, new GUIContent("-90°"));
        EditorGUI.PropertyField(thirdOptionRect, plus90Property, new GUIContent("+90°"));
        EditorGUIUtility.labelWidth = 35f;
        EditorGUI.PropertyField(fourthOptionRect, plus180Property, new GUIContent("+180°"));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
        //base.OnGUI(position, property, label); //If this is called you will have at the beginning and end of the Property the text: "GUI not implemented"
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}

