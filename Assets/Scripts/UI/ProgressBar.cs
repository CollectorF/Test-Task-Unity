using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;

    private float progress;

    public void SetProgress(float progress)
    {
        this.progress = progress;
        fillImage.fillAmount = progress;
    }

    public float GetProgress()
    {
        return progress;
    }
}
