using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCollision : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.playerScore += damage * 100 * Random.Range(0.85f, 1.15f);
    }
}
