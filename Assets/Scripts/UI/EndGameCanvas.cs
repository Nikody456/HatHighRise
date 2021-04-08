#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections.Generic;
using UnityEngine;

public class EndGameCanvas : GameCanvas
{
    protected override void Awake()
    {
        ///Reverse this to be the new canvas HACK
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }

    }

    private void Start()
    {
        var totalScore = PlayerPrefs.GetInt(GameConstants.HAT_SCORE_KEY);
        UpdateScore(totalScore);


        ///TODO Start dropping a bunch of random hats everywhere?
    }
}

