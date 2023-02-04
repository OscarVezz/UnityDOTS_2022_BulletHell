using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float speed;
    public float oneArmPenalty;
    public float twoArmPenalty;

    [Space]
    public float hitboxRadious;
    public float hitboxOneArmBuff;
    public float hitboxTwoArmBuff;
    public Vector3 hitboxOffset;

    [Header("Arms")]
    public GameObject firstArm;
    public GameObject secondArm;

    [Header("Arena Limits")]
    public Vector2 radius = new Vector2(1f, 1f);
    public Vector2 offset = new Vector2(0f, 0f);

    bool firstArmIsActive;
    bool secondArmIsActive;
    float _speed;
    float _hitboxRadious;

    void Start()
    {
        firstArmIsActive = false;
        secondArmIsActive = false;
    }

    void Update()
    {
        GetArmInputs();

        transform.rotation = Quaternion.LookRotation(Vector3.zero - transform.position);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 desireMovementVector = Vector3.Normalize(new Vector3(x, 0, z));
        Vector3 desireMovementPosition = Vector3.MoveTowards(transform.position, transform.position + desireMovementVector, _speed * Time.deltaTime);

        if (ValidatePosition(desireMovementPosition)) {
            transform.position = desireMovementPosition;
        } else {
            Pain(x, z, desireMovementVector, desireMovementPosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + hitboxOffset, _hitboxRadious);
    }

    void GetArmInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firstArmIsActive = !firstArmIsActive;
            firstArm.SetActive(firstArmIsActive);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            secondArmIsActive = !secondArmIsActive;
            secondArm.SetActive(secondArmIsActive);
        }

        if (firstArmIsActive && secondArmIsActive)
        {
            _speed = speed * twoArmPenalty;
            _hitboxRadious = hitboxRadious * hitboxTwoArmBuff;
        }
        else
        {
            if (firstArmIsActive || secondArmIsActive)
            {
                _speed = speed * oneArmPenalty;
                _hitboxRadious = hitboxRadious * hitboxOneArmBuff;
            }
            else
            {
                _speed = speed;
                _hitboxRadious = hitboxRadious;
            }
        }
    }

    void Pain(float x, float z, Vector3 desireMovementVector, Vector3 desireMovementPosition)
    {
        //Debug.Log(desireMovementVector);

        float signX = 1;
        float signZ = 1;
        if (Mathf.Sign(x) * Mathf.Sign(z) > 0)
        {
            if (x == 0 || z == 0) {
                signZ = -1;
            } else {
                signX = -1;
            }
        }
        else
        {
            if (x == 0 || z == 0) {
                signZ = -1;
            } else {
                signX = -1;
            }
        }

        Vector3 ortogonalVector = new Vector3(desireMovementVector.z * signZ, 0, desireMovementVector.x * signX);
        desireMovementPosition = Vector3.MoveTowards(transform.position, transform.position + ortogonalVector, _speed * Time.deltaTime);

        if (ValidatePosition(desireMovementPosition))
        {
            transform.position = desireMovementPosition;
        }
    }

    bool ValidatePosition(Vector3 pos)
    {
        float x = pos.x - offset.x;
        float y = pos.z - offset.y;
        if (((x * x) / (radius.x * radius.x)) + ((y * y) / (radius.y * radius.y)) <= 1)
            return true;

        return false;
    }

    public float GetPlayerHitboxRadious()
    {
        return _hitboxRadious;
    }
}
