using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject endUi;
    [SerializeField]
    private GameObject gameplayUi;

    private LevelEndUI endUiController;
    private GameplayUI gameplayUiController;

    private void Awake()
    {
        endUiController = endUi.GetComponent<LevelEndUI>();
        gameplayUiController = gameplayUi.GetComponent<GameplayUI>();
    }

    internal void ActivateLevelEndUI(int number, LevelState state)
    {
        endUiController.SetAppearence(number, state);
        endUi.SetActive(true);
        gameplayUi.SetActive(false);
    }

    internal void ActivateGameplayUI()
    {
        endUi.SetActive(false);
        gameplayUi.SetActive(true);
    }

    internal void UpdateProgressBar(float initialCount, float killedCount)
    {
        var completePercent = killedCount / initialCount;
        gameplayUiController.UpdateProgress(completePercent);
    }
}
