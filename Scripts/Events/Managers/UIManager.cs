using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text playerScore;


    void Start()
    {
        
    }

    public void UpdatePlayerScore(float rawScore)
    {
        playerScore.text = "Player Score = " + (int)rawScore / 10;
    }
}
