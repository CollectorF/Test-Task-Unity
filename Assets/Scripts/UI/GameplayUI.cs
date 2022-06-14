using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
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
}
