using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelNumberText;

    private ProgressBar progressBar;

    private void Start()
    {
        progressBar = GetComponentInChildren<ProgressBar>();
        progressBar.SetProgress(0);
    }

    public void UpdateProgress(float progress)
    {
        progressBar.SetProgress(progress);
    }

    public void SetLevelNumber(int number)
    {
        levelNumberText.text = $"Level {number}";
    }
}
