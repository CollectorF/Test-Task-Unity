using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;
    [SerializeField]
    public EnemyManager enemyManager;
    [SerializeField]
    public LevelManager levelManager;
    [SerializeField]
    public UIManager uiManager;

    private PlayerController playerController;
    private float completePercent;

    private void Awake()
    {
        playerController = Player.GetComponent<PlayerController>();
        playerController.OnReachEndPoint += levelManager.LoadLevel; 
    }

    private void Start()
    {
        enemyManager.OnDie += UpdateProgressBar;
        levelManager.OnMove += playerController.MoveToNextPlatform;
    }

    private void UpdateProgressBar(float initialCount, float killedCount)
    {
        completePercent = killedCount / initialCount * 100;
        uiManager.UpdateLevelProgress(completePercent);
    }
}
