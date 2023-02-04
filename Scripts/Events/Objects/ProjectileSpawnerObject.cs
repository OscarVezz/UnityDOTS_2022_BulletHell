using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Events;

public class ProjectileSpawnerObject : MonoBehaviour
{
    [Header("Spawner Identifiers")]
    public int spawnerID;
    public int reactionTriggerID;


    [Header("Spawner Steps")]
    public List<SpawnerReactStep> steps;
    private int currentStep = 0;

    void Start()
    {
        EventManager.JsonMusicEvent += React;
        EventManager.CleanSpawners += Clean;
        EventManager.DebugSkip += DebugSkip;
    }



    public void React(int eventID, int orderID)
    {
        if (eventID != reactionTriggerID)
            return;

        if (currentStep > steps.Count - 1)
            return;

        if(steps[currentStep].step == orderID)
        {
            if (steps[currentStep].basicBitch)
            {
                SpawnersManager.Instance.spawnersInformation[spawnerID].spawnArc = SpawnersManager.Instance.spawnArc;
                SpawnersManager.Instance.spawnersInformation[spawnerID].spawnRate = SpawnersManager.Instance.spawnRate;
                SpawnersManager.Instance.spawnersInformation[spawnerID].shutDownTime = Time.time + SpawnersManager.Instance.SpawnTime;

                SpawnersManager.Instance.spawnersInformation[spawnerID].projectileSpeed = SpawnersManager.Instance.projectileSpeed;
                SpawnersManager.Instance.spawnersInformation[spawnerID].projectileLifetime = SpawnersManager.Instance.projectileLifetime;
            }
            else
            {
                SpawnersManager.Instance.spawnersInformation[spawnerID].spawnArc = steps[currentStep].spawnArc;
                SpawnersManager.Instance.spawnersInformation[spawnerID].spawnRate = steps[currentStep].spawnRate;
                SpawnersManager.Instance.spawnersInformation[spawnerID].shutDownTime = Time.time + steps[currentStep].maxSpawnTime;

                SpawnersManager.Instance.spawnersInformation[spawnerID].projectileSpeed = steps[currentStep].projectileSpeed;
                SpawnersManager.Instance.spawnersInformation[spawnerID].projectileLifetime = steps[currentStep].projectileLifetime;
            }

            SpawnersManager.Instance.spawnersInformation[spawnerID].react = true;
            currentStep++;
        }
    }

    public void DebugSkip(int identifier, int count, int globalStart)
    {
        
        if (identifier != reactionTriggerID)
            return;

        //Debug.Log("i = " + identifier + " c = " + count);

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].step >= count)
            {
                currentStep = i;
                //Debug.Log(gameObject.name + ", c step = " + currentStep);
                React(identifier, count);
                break;
            }
        }
    }

    public void Clean()
    {
        currentStep = 0;
    }

    private void OnDisable()
    {
        EventManager.JsonMusicEvent -= React;
        EventManager.CleanSpawners -= Clean;
        EventManager.DebugSkip -= DebugSkip;
    }

}
