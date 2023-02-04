using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    [Header("Optimization")]
    [Range(1, 60)]
    public int readsPerSecond = 1;
    private float previuosTime;
    private float elapseTime;


    [Header("Target FPS")]
    public FPS targetedFPS;
    //[HideInInspector]
    public int actualFPS;
    public int _i;
    public int _j;

    [Header("Player")]
    public GameObject player;
    public float playerScore;

    [Header("UI")]
    public UIManager uIManager;




    [HideInInspector]
    public PlayerManager playerManager;
    [HideInInspector]
    public float2 playerPosition;
    [HideInInspector]
    public float playerHitbox;


    #region Singleton

    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;     // VSync must be disabled
            SetAppFps();
        #endif

    }

    #endregion



    void Start()
    {
        playerManager = player.GetComponent<PlayerManager>();

        previuosTime = Time.time;
        elapseTime = 1 / (float)readsPerSecond;

        EventManager.CleanSpawners += Clean;
    }

    void Update()
    {
        if (Time.time > previuosTime + elapseTime)
        {
            playerPosition = new float2(player.transform.position.x, player.transform.position.z);
            playerHitbox = playerManager.GetPlayerHitboxRadious();

            uIManager.UpdatePlayerScore(playerScore);


            previuosTime = Time.time;
        }
    }


    void Clean()
    {
        playerScore = 0;
    }




    private void OnDisable()
    {
        EventManager.CleanSpawners -= Clean;
    }

    private void OnValidate()
    {
        SetAppFps();
    }

    void SetAppFps()
    {
        actualFPS = (int)targetedFPS * 30;
        Application.targetFrameRate = actualFPS;
        _i = (actualFPS == 240) ? 2 : Mathf.Clamp(360 / actualFPS, 1, 3);
        _j = Mathf.Clamp(360 / actualFPS / 3, 1, 4);
    }

    public enum FPS
    {
        _30 = 1,
        _60 = 2,
        _120 = 4,
        _240 = 8,
    }
}
