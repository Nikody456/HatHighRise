#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UIScoreDisplay : MonoBehaviour
{
    public static string ANIM_TRIGGER = "ScoreAdded";
    [SerializeField] TextMeshProUGUI _scoreCount =default;
    Animator _animator;
    int _currScore = 0;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    public void IncreaseScore(int amnt)
    {
        ///IF you dont want this short anim to play multiple times before exit,
        ///Make sure loop is set to false on anim , and the preview area in the anim transition is tiny
        ///ExitTime =1.234273e-07 , bools seem better than triggers
        ///Also awkwardly need an animState script that flips the bool OnStateEnter....whatever
        _animator.SetBool(ANIM_TRIGGER, true);
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
