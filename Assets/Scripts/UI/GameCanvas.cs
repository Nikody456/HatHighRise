#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoSingleton<GameCanvas>
{
    [SerializeField] UIHealthDisplay _healthDisplay = default;
    [SerializeField] UIScoreDisplay _scoreDisplay = default;


    public void UpdateHealth(bool gainedHealth)
    {
        if(gainedHealth)
        {
            _healthDisplay.IncreaseHealth();
        }
        else
        {
            _healthDisplay.DecreaseHealth();
        }    
    }

    public void UpdateScore(int newScore)
    {
        _scoreDisplay.SetScore(newScore);
    }

    public void IncreaseScore(int amount)
    {
        _scoreDisplay.IncreaseScore(amount);
    }

    public int GetScore()
    {
        return _scoreDisplay.GetScore();
    }
}
