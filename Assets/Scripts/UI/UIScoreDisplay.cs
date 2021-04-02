#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreCount =default;

    public void SetScore(int amount)
    {
        _scoreCount.text = amount.ToString();
    }

}
