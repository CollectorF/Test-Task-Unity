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
    private float completePercent;

    private void Awake()
    {
        playerController = Player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        uiManager.ActivateGameplayUI(levelManager.GetCurrentLevelNumber());
        levelManager.OnMove += playerController.MoveToNextPlatform;
        playerController.OnReachEndPoint += UpdateUi;
        enemyManager.OnDie += uiManager.UpdateProgressBar;
        levelManager.OnLoad += uiManager.ActivateGameplayUI;
    }

    private void UpdateUi(LevelState state)
    {
        levelManager.levelState = state;
        uiManager.ActivateLevelEndUI(levelManager.GetCurrentLevelNumber(), state);
    }
}
