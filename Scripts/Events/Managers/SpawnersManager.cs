using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Events;
using System;

public class SpawnersManager : MonoBehaviour
{
    [Header("Optimization")]
    [Range(1, 60)]
    public int readsPerSecond;
    private float previuosTime;
    private float elapseTime;


    [Header("Global Values")]
    public float spawnRate;
    [Range(0f, 1f)]
    public float spawnArc;
    public float SpawnTime;

    [Space]
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileSize;


    [Header("Spawner Objects")]
    public SpawnersInformation[] spawnersInformation;

    [HideInInspector]
    public bool killAllProjectiles;



    #region Singleton

    private static SpawnersManager _instance = null;
    public static SpawnersManager Instance
    {
        get 
        {
            if (_instance == null)   //--------------
                _instance = (SpawnersManager)FindObjectOfType(typeof(SpawnersManager)); //--------------
            return _instance; 
        }
    }

    
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    #endregion


    void Start()
    {
        previuosTime = Time.time;
        elapseTime = 1 / (float)readsPerSecond;

        EventManager.CleanSpawners += StopSpawnersInmediatly;
    }

    private void Update()
    {
        if (Time.time > previuosTime + elapseTime)
        {
            if (!EventManager.isMainEventActive)
                return;

            foreach (SpawnersInformation si in spawnersInformation)
            {
                si.position = si.go.transform.position;
                si.right = si.go.transform.right;

                if (si.react && Time.time > si.shutDownTime)
                {
                    si.react = false;
                }
            }

            previuosTime = Time.time;
        }
    }

    void StopSpawnersInmediatly()
    {
        foreach(SpawnersInformation si in spawnersInformation)
        {
            si.react = false;
        }
        killAllProjectiles = true;
    }

    private void OnDisable()
    {
        EventManager.CleanSpawners -= StopSpawnersInmediatly;
    }
}
