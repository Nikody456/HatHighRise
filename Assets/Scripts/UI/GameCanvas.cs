#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoSingleton<GameCanvas>
{
    [SerializeField] UIHealthDisplay _healthDisplay = default;
    [SerializeField] UIScoreDisplay _scoreDisplay = default;
    [SerializeField] GameObject _pauseMenu = default;


    bool _isPaused = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FlipPause();
        }
    }

    public void FlipPause()
    {
        AudioManager.Instance.PlaySFX("menuClickSound");
        _isPaused = !_isPaused;
        _pauseMenu.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
    }

    public void SetHealth(int amount)
    {
        _healthDisplay.SetHealth(amount);
    }

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

    public void DisplayTotalVsCurrent(bool cond)
    {
        _scoreDisplay.DisplayTotalVsCurrent(cond);
    }




}
