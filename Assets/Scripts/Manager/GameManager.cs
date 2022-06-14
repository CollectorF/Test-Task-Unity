using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
    Win,
    Lose
}

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
    private EnemyController closesetEnemy;

    private void Awake()
    {
        playerController = Player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        uiManager.ActivateGameplayUI(levelManager.GetCurrentLevelNumber());

        playerController.OnReachEndPoint += UpdateUI;
        playerController.OnFirstShot += UpdateStarterUI;

        enemyManager.OnDie += uiManager.UpdateProgressBar;

        levelManager.OnLoad += uiManager.ActivateGameplayUI;
        levelManager.OnMove += playerController.MoveToNextPlatform;

        DrawTargetOnClosestEnemy();
    }

    private void UpdateUI(LevelState state)
    {
        levelManager.levelState = state;
        uiManager.ActivateLevelEndUI(levelManager.GetCurrentLevelNumber(), state);
    }

    private void UpdateStarterUI()
    {
        enemyManager.FirstShotCompleted();
        closesetEnemy.targetCanvas.enabled = false;
    }

    private void DrawTargetOnClosestEnemy()
    {
        closesetEnemy = enemyManager.FindClosestEnemy(playerController.transform.position);
        closesetEnemy.targetCanvas.enabled = true;
    }
}
