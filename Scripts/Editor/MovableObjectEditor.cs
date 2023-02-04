using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameJam.Events;


[CustomEditor(typeof(MovableObject))]
public class MovableObjectEditor : Editor
{
    MovableObject mo;
    SerializedObject sObjectMo;
    SerializedProperty steps;

    string sectionStyle = "Tooltip";


    private void OnEnable()
    {
        mo = (MovableObject)target;
        EditorUtility.SetDirty(mo);

        sObjectMo = new SerializedObject(target);
        steps = sObjectMo.FindProperty("steps");
    }

    public override void OnInspectorGUI()
    {
        sObjectMo.Update();

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
            mo.reactionTriggerID = EditorGUILayout.IntField("Reaction Trigger Id", mo.reactionTriggerID);
            mo.isDegenerate = EditorGUILayout.Toggle("Is Degenerate?", mo.isDegenerate);
            mo.forceComingBack = EditorGUILayout.Toggle("Force Coming Back?", mo.forceComingBack);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();



        //Section 2 (Global Values)
        EditorGUILayout.BeginVertical(sectionStyle);
            EditorGUILayout.LabelField("Global Values", titleStyle);
            mo.comeBack = EditorGUILayout.Toggle("Come Back?", mo.comeBack);
            mo.vectorChange = EditorGUILayout.Vector3Field("Delta Movement Vector", mo.vectorChange);
            mo.startSpeed = EditorGUILayout.FloatField("Start Speed", mo.startSpeed);
            mo.returnSpeed = EditorGUILayout.FloatField("Return Speed", mo.returnSpeed);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();



        //Section 3 (Steps)

        if (!mo.isDegenerate)
        {

            EditorGUILayout.BeginVertical(sectionStyle);
            EditorGUILayout.LabelField("Movable Steps", titleStyle);

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
                mo.steps.Add(new MovableReactStep(mo.steps[mo.steps.Count - 1].step + 1, mo.comeBack, mo.vectorChange, mo.startSpeed, mo.returnSpeed));
            }
            if (GUILayout.Button("Remove Step"))
            {
                mo.steps.RemoveAt(mo.steps.Count - 1);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();



            EditorGUILayout.LabelField("Id   Trigger   BasicB?   CmBck?                     Vector Change                 StrSp     RtrSp");

            for (int i = 0; i < steps.arraySize; i++)
            {
                SerializedProperty segRef = steps.GetArrayElementAtIndex(i);

                SerializedProperty step = segRef.FindPropertyRelative("step");
                SerializedProperty basicBitch = segRef.FindPropertyRelative("basicBitch");



                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("" + i, GUILayout.MaxWidth(20));

                step.intValue = EditorGUILayout.IntField(step.intValue, GUILayout.MaxWidth(35));
                GUILayout.Space(22);
                basicBitch.boolValue = EditorGUILayout.Toggle(basicBitch.boolValue, GUILayout.MaxWidth(15));

                if (!basicBitch.boolValue)
                {
                    SerializedProperty comeBack = segRef.FindPropertyRelative("comeBack");
                    SerializedProperty vectorChange = segRef.FindPropertyRelative("vectorChange");
                    SerializedProperty startSpeed = segRef.FindPropertyRelative("startSpeed");
                    SerializedProperty returnSpeed = segRef.FindPropertyRelative("returnSpeed");
                    
                    GUILayout.Space(37);
                    comeBack.boolValue = EditorGUILayout.Toggle(comeBack.boolValue, GUILayout.MaxWidth(35));

                    GUILayout.Space(5);
                    vectorChange.vector3Value = EditorGUILayout.Vector3Field("", vectorChange.vector3Value, GUILayout.MaxWidth(175));

                    GUILayout.Space(10);
                    startSpeed.floatValue = EditorGUILayout.FloatField(startSpeed.floatValue, GUILayout.MaxWidth(35));

                    GUILayout.Space(8);
                    returnSpeed.floatValue = EditorGUILayout.FloatField(returnSpeed.floatValue, GUILayout.MaxWidth(35));
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

       


        sObjectMo.ApplyModifiedProperties();
    }
}
