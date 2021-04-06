#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using Statistics;



[RequireComponent(typeof(Stats))]
public class PlayerMonitor : MonoBehaviour
{
    private Stats _playerStats;

    private int _lastKnownHealth;
    private int _currentScoreThisLevel = 0;

    private bool _enabled = false;

    private void OnEnable()
    {
        if (!_enabled && _playerStats)
        {
            _playerStats.OnHealthChanged += PlayerHealthChanged;
        }
    }
    private void OnDisable()
    {
        if (_enabled && _playerStats)
        {
            _playerStats.OnHealthChanged -= PlayerHealthChanged;
        }
    }
    private void Start()
    {
        _playerStats = this.GetComponent<Stats>();
        _lastKnownHealth = _playerStats.CurrentHealth;
        InitializeHealthInUI();
        OnEnable();
    }

    private void InitializeHealthInUI()
    {
        for (int i = 0; i < _lastKnownHealth; i++)
        {
            GameCanvas.Instance.UpdateHealth(true);
        }
    }

    private void PlayerHealthChanged(int currHealth)
    {
        Debug.Log($"PM health changed : cur={currHealth} vs last={_lastKnownHealth} ");
        if (currHealth == _lastKnownHealth)
            return;

        bool gainedHealth = currHealth > _lastKnownHealth;
        GameCanvas.Instance.UpdateHealth(gainedHealth);
        _lastKnownHealth = currHealth;

    }

    public void AlterScoreThisLevel(int amount)
    {
        _currentScoreThisLevel += amount;
        if (_currentScoreThisLevel < 0)
        {
            _currentScoreThisLevel = 0;
        }
        GameCanvas.Instance.UpdateScore(_currentScoreThisLevel);
    }

    public void OnPlayerReset()
    {
        ///this means I got melee hit without any hats,
        /// or fell down a death trap while wearing hats?

        /// 50% chance to drop a coin ur carrying, 50% to lose entirely 
        int secondChanceCoins = 0;
        System.Random rng = new System.Random();
        for (int i = 0; i < _currentScoreThisLevel; i++)
        {
           if( rng.Next() % 2 == 0)
            {
                ++secondChanceCoins;
            }
        }

        CoinCreator.Instance.CreateSomeCoins(secondChanceCoins, this.transform.position);
        ///Drop all coins:
        AlterScoreThisLevel(-_currentScoreThisLevel);
    }

}
