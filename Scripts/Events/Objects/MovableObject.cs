using GameJam.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Header("Spawner Identifiers")]
    public int reactionTriggerID;
    public bool isDegenerate;
    public bool forceComingBack;

    [Header("Global Variables")]
    public bool comeBack;
    public Vector3 vectorChange;
    public float startSpeed;
    public float returnSpeed;

    bool _comeBack;
    Vector3 _vectorChange;
    float _startSpeed;
    float _returnSpeed;


    [Header("Spawner Steps")]
    public List<MovableReactStep> steps;
    private int currentStep = 0;


    bool canMove;
    Vector3 previousTo;
    Vector3 to;
    float speed;

    bool isComigBack;


    Vector3 firstPosition;

    void Start()
    {
        firstPosition = transform.position;

        EventManager.JsonMusicEvent += React;
        EventManager.CleanSpawners += Clean;
        EventManager.DebugSkip += DebugSkip;
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, to) < 0.01)
            {
                transform.position = to;

                if (isComigBack)
                {
                    canMove = false;
                    isComigBack = false;
                    return;
                }

                if (_comeBack)
                {
                    previousTo = to;
                    to = transform.position - _vectorChange;
                    speed = _returnSpeed;
                    isComigBack = true;
                }
                else
                {
                    canMove = false;
                }
            }
        }
        
    }

    public void React(int eventID, int orderID)
    {
        if (eventID != reactionTriggerID)
            return;

        if (currentStep > steps.Count - 1)
            return;

        if (isDegenerate)
        {
            canMove = true;

            _comeBack = comeBack;
            _vectorChange = vectorChange;
            _startSpeed = startSpeed;
            _returnSpeed = returnSpeed;

            if (isComigBack && forceComingBack)
                to = previousTo;
            else
                to = transform.position + _vectorChange;

            speed = _startSpeed;
            isComigBack = false;

            return;
        }

        if (steps[currentStep].step == orderID)
        {
            canMove = true;

            if (steps[currentStep].basicBitch) {
                _comeBack = comeBack;
                _vectorChange = vectorChange;
                _startSpeed = startSpeed;
                _returnSpeed = returnSpeed;
            } else {
                _comeBack = steps[currentStep].comeBack;
                _vectorChange = steps[currentStep].vectorChange;
                _startSpeed = steps[currentStep].startSpeed;
                _returnSpeed = steps[currentStep].returnSpeed;
            }

            to = transform.position + _vectorChange;   
            speed = _startSpeed;
            isComigBack = false;

            currentStep++;
        }
    }

    public void DebugSkip(int identifier, int count, int globalStart)
    {
        if (identifier != reactionTriggerID)
            return;

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].step > count)
            {
                currentStep = i;
                break;
            }
        }
    }

    public void Clean()
    {
        canMove = false;
        currentStep = 0;
        transform.position = firstPosition;
    }

    private void OnDisable()
    {
        EventManager.JsonMusicEvent -= React;
        EventManager.CleanSpawners -= Clean;
        EventManager.DebugSkip -= DebugSkip;
    }
}
