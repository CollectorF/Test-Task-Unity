using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelNumberText;
    [SerializeField]
    private TextMeshProUGUI btnNextText;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Color winColor;
    [SerializeField]
    private Color loseColor;

    internal void SetAppearence(int levelNumber, LevelState state)
    {
        switch (state)
        {
            case LevelState.Win:
                background.color = winColor;
                levelNumberText.text = $"You win level {levelNumber}!";
                btnNextText.text = "Next level";
                break;
            case LevelState.Lose:
                background.color = loseColor;
                levelNumberText.text = $"You lose level {levelNumber}!";
                btnNextText.text = "Replay level";
                break;
            default:
                break;
        }
    }
}
