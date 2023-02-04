using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameJam.Events;

[CustomEditor(typeof(ProjectileSpawnerObject))]
public class ProjectileSpawnerObjectEditor : Editor
{
    ProjectileSpawnerObject pos;
    SerializedObject sObjectPos;
    SerializedProperty steps;

    string sectionStyle = "Tooltip";


    private void OnEnable()
    {
        pos = (ProjectileSpawnerObject)target;
        EditorUtility.SetDirty(pos);

        sObjectPos = new SerializedObject(target);
        steps = sObjectPos.FindProperty("steps");
    }

    public override void OnInspectorGUI()
    {
        sObjectPos.Update();


        //base.OnInspectorGUI();

        GUIStyle titleStyle = new GUIStyle(GUI.skin.textField)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };




        // Property Label
        EditorGUIUtility.labelWidth = 215;
        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);



        // Section 1 (Spawner Identifiers)
        EditorGUILayout.BeginVertical(sectionStyle);
            EditorGUILayout.LabelField("Spawner Identifiers", titleStyle);
            pos.spawnerID = EditorGUILayout.IntField("Spawner Id", pos.spawnerID);
            pos.reactionTriggerID = EditorGUILayout.IntField("Reaction Trigger Id", pos.reactionTriggerID);
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();



        //Section 2 (Spawner Steps)
        EditorGUILayout.BeginVertical(sectionStyle);
            EditorGUILayout.LabelField("Spawner Steps", titleStyle);

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
            pos.steps.Add(new SpawnerReactStep(pos.steps[pos.steps.Count - 1].step + 1, 
                SpawnersManager.Instance.spawnArc, SpawnersManager.Instance.spawnRate, SpawnersManager.Instance.SpawnTime,
                SpawnersManager.Instance.projectileSpeed, SpawnersManager.Instance.projectileLifetime));
        }
        if (GUILayout.Button("Remove Step"))
        {
            pos.steps.RemoveAt(pos.steps.Count - 1);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();




        EditorGUILayout.LabelField("Id   Trigger   BasicB?   SpwnArc   SpwnRte    SpwnTime   PrjtSpeed   PrjtLifet");

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
                SerializedProperty spawnArc = segRef.FindPropertyRelative("spawnArc");
                SerializedProperty spawnRate = segRef.FindPropertyRelative("spawnRate");
                SerializedProperty maxSpawnTime = segRef.FindPropertyRelative("maxSpawnTime");

                SerializedProperty projectileSpeed = segRef.FindPropertyRelative("projectileSpeed");
                SerializedProperty projectileLifetime = segRef.FindPropertyRelative("projectileLifetime");


                GUILayout.Space(25);
                spawnArc.floatValue = Mathf.Clamp01(EditorGUILayout.FloatField(spawnArc.floatValue, GUILayout.MaxWidth(45)));

                GUILayout.Space(13);
                spawnRate.floatValue = EditorGUILayout.FloatField(spawnRate.floatValue, GUILayout.MaxWidth(45));

                GUILayout.Space(18);
                maxSpawnTime.floatValue = EditorGUILayout.FloatField(maxSpawnTime.floatValue, GUILayout.MaxWidth(45));

                GUILayout.Space(17);
                projectileSpeed.floatValue = EditorGUILayout.FloatField(projectileSpeed.floatValue, GUILayout.MaxWidth(45));

                GUILayout.Space(11);
                projectileLifetime.floatValue = EditorGUILayout.FloatField(projectileLifetime.floatValue, GUILayout.MaxWidth(45));
            }

                

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();



        sObjectPos.ApplyModifiedProperties();
    }
}
