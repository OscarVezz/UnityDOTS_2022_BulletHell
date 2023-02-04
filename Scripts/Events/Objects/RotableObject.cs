using GameJam.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using GameJam.Easing;

public class RotableObject : MonoBehaviour
{

    [Header("Spawner Identifiers")]
    public int reactionTriggerID;
    public bool forceComplation;

    bool _comeBack;
    float _yChange;
    float _startSpeed;
    float _returnSpeed;
    float _offset;
    EasingType _easingType;


    [Header("Spawner Steps")]
    public List<RotableReactStep> steps;
    private int currentStep = 0;


    bool canMove;
    Vector3 from;
    Vector3 to;
    float speed;


    Vector3 firstAngle;
    Vector3 previousAngle;
    float startTime;
    float endTime;

    bool isComigBack;

    void Start()
    {
        firstAngle = transform.eulerAngles;
        previousAngle = transform.eulerAngles;

        EventManager.JsonMusicEvent += React;
        EventManager.CleanSpawners += Clean;
        EventManager.DebugSkip += DebugSkip;
    }

    void Update()
    {
        if (canMove)
        {
            float progresion = (Time.time - startTime) / (endTime - startTime);
            progresion += _offset;
            progresion = MathEasing.Evaluate(progresion, _easingType);

            float angle = Mathf.Lerp(from.y, to.y, progresion);
            transform.eulerAngles = new Vector3(0, angle, 0);


            if (Time.time > endTime)
            {
               ChangeAngle();
            }
        }
    }

    public void React(int eventID, int orderID)
    {
        if (eventID != reactionTriggerID)
            return;

        if (currentStep > steps.Count - 1)
            return;

        if (steps[currentStep].step == orderID)
        {
            if (canMove & forceComplation)
                ChangeAngle();

            canMove = true;

            if (steps[currentStep].chasePlayer)
            {
                Vector3 lookVector = new Vector3 (GameManager.Instance.playerPosition.x, 0, GameManager.Instance.playerPosition.y);

                float angle = Quaternion.FromToRotation(transform.forward, Vector3.Normalize(lookVector)).eulerAngles.y;
                if (angle > 180) 
                    angle -= 360f; 
                //float angle = Vector3.Angle(Vector3.Normalize(lookVector), transform.forward);
                //float angle =  Mathf.Atan2(lookVector.y, lookVector.x) * 180 / Mathf.PI;

                _yChange = angle * 2;
            }
            else
            {
                _yChange = steps[currentStep].yChange;
            }

            _comeBack = steps[currentStep].comeBack;
            _startSpeed = steps[currentStep].startSpeed;
            _returnSpeed = steps[currentStep].returnSpeed;
            _offset = steps[currentStep].offset;
            _easingType = steps[currentStep].easingType;

            from = previousAngle;
            to = previousAngle + new Vector3(0, _yChange, 0);    
            speed = _startSpeed;
            isComigBack = false;

            startTime = Time.time;
            endTime = (Mathf.Abs(from.y - to.y) / (speed * 360)) + startTime;

            currentStep++;
        }
    }

    public void ChangeAngle()
    {
        transform.eulerAngles = to;
        previousAngle = to;

        if (isComigBack)
        {
            canMove = false;
            isComigBack = false;
            return;
        }

        if (_comeBack)
        {
            to = from;
            from = from + new Vector3(0, _yChange, 0);
            speed = _returnSpeed;
            isComigBack = true;

            if (_easingType > 0)
                _easingType--;


            startTime = Time.time;
            endTime = (Mathf.Abs(from.y - to.y) / (speed * 360)) + startTime;
        }
        else
        {
            canMove = false;
        }
    }

    public void DebugSkip(int identifier, int count, int globalStart)
    {
        if (identifier != reactionTriggerID)
            return;


        float resultingAngle = previousAngle.y;
        for (int i = 0; i < steps.Count; i++)
        {
            if (!steps[i].comeBack)
                resultingAngle += steps[i].yChange;

            if (steps[i].step >= count)
            {
                currentStep = i;

                float prevY = resultingAngle -= steps[i].yChange;
                previousAngle = new Vector3(0,resultingAngle, 0);

                transform.eulerAngles = new Vector3(0, resultingAngle, 0);
                React(identifier, count);
                break;
            }
        }
    }
    public void Clean()
    {
        canMove = false;
        currentStep = 0;
        transform.eulerAngles = firstAngle;
        previousAngle = firstAngle;
    }

    private void OnDisable()
    {
        EventManager.JsonMusicEvent -= React;
        EventManager.CleanSpawners -= Clean;
        EventManager.DebugSkip -= DebugSkip;
    }
}
