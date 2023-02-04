using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTest : MonoBehaviour
{
    [Header("Reaction Triggers")]
    public int reactionTriggerID;
    public int minOrderTrigger;
    public int maxOrderTrigger;
    public int lastTrigger;
    public float collapseSpeed;
    public float collapseTime;


    [Header("Global Attributes")]
    public GameObject prefab;
    public float globalSpeed;
    public float globalLength;
    public float globalProbability;


    List<FireInformation> information;
    int a = 0, b = 3;
    float changeDirection = 1;
    bool collapse;
    float killTime;

    bool closeGate;
    bool openGate;

    void Start()
    {
        information = new List<FireInformation>();
        collapse = false;
        closeGate = false;
        openGate = false;


        EventManager.JsonMusicEvent += React;
        EventManager.CleanSpawners += Clean;
    }


    void Update()
    {
        if (closeGate)
            return;

        foreach(FireInformation individualObject in information)
        {
            if (!collapse)
            {
                if (!individualObject.isRotating)
                {
                    individualObject.gameObject.transform.position = Vector3.MoveTowards(
                        individualObject.gameObject.transform.position, individualObject.direction, globalSpeed * Time.deltaTime);

                    if (Vector3.Distance(individualObject.gameObject.transform.position, individualObject.direction) < 0.01)
                    {
                        individualObject.gameObject.transform.position = individualObject.direction;
                        individualObject.isRotating = true;
                    }
                }
                else
                {
                    individualObject.angle += MoveObject(individualObject.radius, individualObject.individualSpeed);
                    individualObject.gameObject.transform.position = new Vector3(
                        Mathf.Cos(individualObject.angle), 0, Mathf.Sin(individualObject.angle)) * individualObject.radius;

                    Vector3 direction = Vector3.Normalize(individualObject.gameObject.transform.position);
                    Vector3 orthogonal = new Vector3(direction.z * individualObject.change * 1, 0, direction.x * individualObject.change * -1);
                    individualObject.gameObject.transform.rotation = Quaternion.LookRotation(orthogonal);
                }
            }
            else
            {
                individualObject.radius += collapseSpeed * Time.deltaTime;

                individualObject.angle += MoveObject(individualObject.radius, individualObject.individualSpeed);
                individualObject.gameObject.transform.position = new Vector3(
                    Mathf.Cos(individualObject.angle), 0, Mathf.Sin(individualObject.angle)) * individualObject.radius;

                if(Time.time > killTime)
                {
                    openGate = true;
                    break;
                }
            }
        }

        if (openGate)
        {
            Clean();
            closeGate = true;
        }
    }

    float MoveObject(float radius, float speed)
    {
        return (speed / (radius * Mathf.PI * 0.5f)) * Time.deltaTime;
    }

    void React(int eventId, int orderId)
    {
        if (eventId != reactionTriggerID)
            return;

        if (orderId == lastTrigger)
        {
            killTime = Time.time + collapseTime;
            collapse = true;
        }

        if (orderId < minOrderTrigger || orderId > maxOrderTrigger)
            return;

        //Debug.Log("react");
        ReTriggerObjects();

        Vector3 directionA = SpawnersManager.Instance.spawnersInformation[a].position;
        Vector3 directionB = SpawnersManager.Instance.spawnersInformation[b].position;

        Vector3 normalA = Vector3.Normalize(new Vector3(directionA.x, 0, directionA.z));
        Vector3 normalB = Vector3.Normalize(new Vector3(directionB.x, 0, directionB.z));

        information.Add(new FireInformation(normalA, Instantiate(prefab, transform), globalSpeed * changeDirection, globalLength, changeDirection));
        information.Add(new FireInformation(normalB, Instantiate(prefab, transform), globalSpeed * changeDirection, globalLength, changeDirection));
        changeDirection *= -1;

        if (a < 5) a++; else a = 0;
        if (b < 5) b++; else b = 0;
    }



    void ReTriggerObjects()
    {
        foreach (FireInformation individualObject in information)
        {
            if (individualObject.isRotating)
            {
                if(Random.Range(0, 100) < globalProbability)
                {
                    individualObject.radius += globalLength;
                    individualObject.direction = Vector3.Normalize(individualObject.gameObject.transform.position) * individualObject.radius;

                    individualObject.angle = Vector3.SignedAngle(individualObject.direction, Vector3.right, Vector3.up) * Mathf.Deg2Rad;

                    individualObject.isRotating = false;
                }
            }
        }
    }

    void Clean()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        information.Clear();
        collapse = false;
        openGate = false;
        closeGate = false;
    }

    private void OnDisable()
    {
        EventManager.JsonMusicEvent -= React;
        EventManager.CleanSpawners -= Clean;
    }

    //[System.Serializable]
    public class FireInformation
    {
        public float radius;
        public float angle;
        public float individualSpeed;
        public float change;

        public Vector3 direction;
        public bool isRotating;

        public GameObject gameObject;

        public FireInformation(Vector3 direction, GameObject gameObject, float individualSpeed, float length, float change)
        {
            radius = 1 * length;
            isRotating = false;

            this.direction = direction;
            this.gameObject = gameObject;

            angle = Vector3.SignedAngle(direction, Vector3.right, Vector3.up) * Mathf.Deg2Rad;
            this.individualSpeed = individualSpeed;
            this.change = change;
            //angle = Mathf.Atan2(direction.x, direction.z);
        }
    }
}
