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
    public UIManager uiManager;

    private float completePercent;

    private void Start()
    {
        enemyManager.OnDie += UpdateProgressBar;
    }

    private void UpdateProgressBar(float initialCount, float killedCount)
    {
        completePercent = killedCount / initialCount * 100;
        uiManager.UpdateLevelProgress(completePercent);
    }
}
