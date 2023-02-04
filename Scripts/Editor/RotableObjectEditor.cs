using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameJam.Events;
using GameJam.Easing;


[CustomEditor(typeof(RotableObject))]
public class RotableObjectEditor : Editor
{
    RotableObject ro;
    SerializedObject sObjectRo;
    SerializedProperty steps;

    string sectionStyle = "Tooltip";


    private void OnEnable()
    {
        ro = (RotableObject)target;
        EditorUtility.SetDirty(ro);

        sObjectRo = new SerializedObject(target);
        steps = sObjectRo.FindProperty("steps");
    }

    public override void OnInspectorGUI()
    {
        sObjectRo.Update();

        //base.OnInspectorGUI();


        GUIStyle titleStyle = new GUIStyle(GUI.skin.textField)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };



        // Property Label
        EditorGUIUtility.labelWidth = 215;
        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);



        // Section 1 (Identifiers)
        EditorGUILayout.BeginVertical(sectionStyle);
            EditorGUILayout.LabelField("Reaction Identifiers", titleStyle);
            ro.reactionTriggerID = EditorGUILayout.IntField("Reaction Trigger Id", ro.reactionTriggerID);
            ro.forceComplation = EditorGUILayout.Toggle("Force Complation", ro.forceComplation);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        //Section 2 (Steps)
        EditorGUILayout.BeginVertical(sectionStyle);
        EditorGUILayout.LabelField("Rotable Steps", titleStyle);

        int listSize;
        listSize = steps.arraySize;
        listSize = Mathf.Clamp(EditorGUILayout.IntField("Number of Steps", listSize), 1, 128);

        if (listSize < 1)
            listSize = 1;

        if (listSize != steps.arraySize)
        {
            while (listSize > steps.arraySize)
            {
                steps.InsertArrayElementAtIndex(steps.arraySize);
            }
            while (listSize < steps.arraySize)
            {
                steps.DeleteArrayElementAtIndex(steps.arraySize - 1);
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Step"))
        {
            ro.steps.Add(new RotableReactStep(ro.steps[ro.steps.Count - 1].step + 1));
        }
        if (GUILayout.Button("Remove Step"))
        {
            ro.steps.RemoveAt(ro.steps.Count - 1);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();



        EditorGUILayout.LabelField("Id   Trigger   ChaseP?   CmBck?     Angle Change      StrSp        RtrSp              In           Offset");

        for (int i = 0; i < steps.arraySize; i++)
        {
            SerializedProperty segRef = steps.GetArrayElementAtIndex(i);

            SerializedProperty step = segRef.FindPropertyRelative("step");
            SerializedProperty chasePlayer = segRef.FindPropertyRelative("chasePlayer");

            SerializedProperty comeBack = segRef.FindPropertyRelative("comeBack");
            SerializedProperty yChange = segRef.FindPropertyRelative("yChange");
            SerializedProperty startSpeed = segRef.FindPropertyRelative("startSpeed");
            SerializedProperty returnSpeed = segRef.FindPropertyRelative("returnSpeed");
            SerializedProperty offset = segRef.FindPropertyRelative("offset");

            SerializedProperty easingType = segRef.FindPropertyRelative("easingType");



            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("" + i, GUILayout.MaxWidth(20));

            step.intValue = EditorGUILayout.IntField(step.intValue, GUILayout.MaxWidth(35));

            GUILayout.Space(26);
            chasePlayer.boolValue = EditorGUILayout.Toggle(chasePlayer.boolValue, GUILayout.MaxWidth(15));

            GUILayout.Space(38);
            comeBack.boolValue = EditorGUILayout.Toggle(comeBack.boolValue, GUILayout.MaxWidth(35));

            GUILayout.Space(20);
            yChange.floatValue = EditorGUILayout.FloatField(yChange.floatValue, GUILayout.MaxWidth(55));

            GUILayout.Space(17);
            startSpeed.floatValue = EditorGUILayout.FloatField(startSpeed.floatValue, GUILayout.MaxWidth(45));

            GUILayout.Space(10);
            returnSpeed.floatValue = EditorGUILayout.FloatField(returnSpeed.floatValue, GUILayout.MaxWidth(45));

            GUILayout.Space(8);
            EditorGUILayout.PropertyField(easingType, new GUIContent(""), GUILayout.MaxWidth(55));

            GUILayout.Space(10);
            offset.floatValue = EditorGUILayout.FloatField(offset.floatValue, GUILayout.MaxWidth(45));


            EditorGUILayout.EndHorizontal();
        }



        EditorGUILayout.EndVertical();


        sObjectRo.ApplyModifiedProperties();
    }
}
