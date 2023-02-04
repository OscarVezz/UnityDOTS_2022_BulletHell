using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameJam.Events;
using GameJam.Audio;

public class EventManager : MonoBehaviour
{
    [Header("Optimization")]
    [Range(1, 60)]
    public int readsPerSecond = 1;
    private float previuosTime;
    private float elapseTime;


    [Header("Events")]
    public TextAsset textJson;
    public static event Action<int, int> JsonMusicEvent;
    public static event Action CleanSpawners;
    public static event Action<int, int, int> DebugSkip;
    private int[] totalTriggersCalls;

    public AudioManager audioManager;
    public int globalStart;



    TriggerList triggerList = new TriggerList();

    float startTime;
    public static bool isMainEventActive;     //Cambiar a gameManager y hacer gameManager singleton
    int count = 0;


    void Start()
    {
        isMainEventActive = false;

        previuosTime = Time.time;
        elapseTime = 1 / (float)readsPerSecond;

        triggerList = JsonUtility.FromJson<TriggerList>(textJson.text);
        totalTriggersCalls = new int[5];
        CleanTriggerConstants();
    }


    void Update()
    {
        // The main process is in a set clock to lessing up system stress
        if (Time.time > previuosTime + elapseTime)
        {
            ProcessMainJsonEvent();     // To process al the Json timing information

            previuosTime = Time.time;
        }


        if(Input.GetKeyDown(KeyCode.O))   //Cambiar a gameManager y hacer gameManager singleton
        {
            startTime = Time.time + audioManager.audioLatencyDelay;
            isMainEventActive = true;
            count = 0;

            CleanTriggerConstants();
            CleanSpawners?.Invoke();

            audioManager.Play(SoundType.Main, false, 0);
        }

        if(Input.GetKeyDown(KeyCode.Q))     //For Debug Purposes Only (Delete Later or IDK)
        {
            count = 0;
            CleanTriggerConstants();
            CleanSpawners?.Invoke();


            int internalCount = 0;
            float curretTime = 0;

            for(int i = 0; i < triggerList.triggers.Length; i++)
            {
                int id = triggerList.triggers[i].Id;
                totalTriggersCalls[id]++;

                if (id == 2)
                {
                    internalCount++;
                    totalTriggersCalls[0]++;
                    totalTriggersCalls[1]++;
                }

                if(internalCount == globalStart)
                {
                    totalTriggersCalls[0]--;
                    totalTriggersCalls[1]--;
                    curretTime = triggerList.triggers[i].Start;
                    count = i;
                    break;
                }
            }

            startTime = Time.time + audioManager.audioLatencyDelay - curretTime;

            for (int i = 0; i < totalTriggersCalls.Length; i++)
            {
                DebugSkip?.Invoke(i, totalTriggersCalls[i], globalStart);
            }
            
            isMainEventActive = true;
            audioManager.Play(SoundType.Main, true, curretTime);
        }
    }


    void ProcessMainJsonEvent()
    {
        if (!isMainEventActive)
            return;

        if (count > triggerList.triggers.Length - 1)
            return;


        if (Time.time - startTime > triggerList.triggers[count].Start)
        {
            int id = triggerList.triggers[count].Id;
            
            if(id == 2)
            {
                JsonMusicEvent?.Invoke(0, totalTriggersCalls[0]);
                JsonMusicEvent?.Invoke(1, totalTriggersCalls[1]);
                JsonMusicEvent?.Invoke(2, totalTriggersCalls[2]);
                totalTriggersCalls[0]++;
                totalTriggersCalls[1]++;
                totalTriggersCalls[2]++;
            }
            else
            {
            
                JsonMusicEvent?.Invoke(id, totalTriggersCalls[id]);
                totalTriggersCalls[id]++;
            }
            
            count++; 
        }
    }


    void CleanTriggerConstants()
    {
        for(int i = 0; i < 5; i++)
        {
            totalTriggersCalls[i] = 0;
        }
    }
}
