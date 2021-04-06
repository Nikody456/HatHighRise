#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreCount =default;
    int _currScore = 0;

    public void IncreaseScore(int amnt)
    {
        _currScore += amnt;
        _scoreCount.text = _currScore.ToString();
    }

    public void SetScore(int amount)
    {
        _currScore = amount;
        _scoreCount.text = _currScore.ToString();
    }

    public int GetScore() => _currScore;
}
