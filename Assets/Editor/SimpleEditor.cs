using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestThing))]
public class SimpleEditor : Editor
{
    SerializedProperty prop1;
    SerializedProperty prop2;

    void OnEnable()
    {
        prop1 = serializedObject.FindProperty("num1");
        prop2 = serializedObject.FindProperty("num2");
    }

    public override void OnInspectorGUI()
    {
        //update the serializedProperty - always do this in the beginning
        serializedObject.Update();

        //show custom GUI controls
        EditorGUILayout.IntSlider(prop1, 0, 100, new GUIContent("Number 1"));

        //apply changes to the serializedProperty - always do this at the end
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        /*TestThing thingy = target as TestThing;
        //Transform t;
        Handles.color = new Color(1, 1, 1, 150);
        Handles.DrawSolidArc(thingy.transform.position, thingy.transform.up, thingy.transform.right)
        prop2.floatValue = Handles.RadiusHandle(Quaternion.identity, thingy.transform.position, thingy.num2);
        serializedObject.ApplyModifiedProperties();*/
    }
}
