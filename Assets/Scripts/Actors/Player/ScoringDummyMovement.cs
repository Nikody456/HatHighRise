#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using Statistics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringDummyMovement : MonoBehaviour
{

    [SerializeField] ScoreIncriminator _scoreCounter;
    [SerializeField] Transform _endGoal = default;
    [SerializeField] bool _isFinalLevel = false;
    private CharMovement _dummy;
    private List<GameObject> _hatsToScore = new List<GameObject>();



    private void Awake()
    {

        if (_isFinalLevel)
        {
            AudioManager.Instance.PlayBackgroundMusic("EndGame");
        }
        else
        {
            AudioManager.Instance.PlayBackgroundMusic("ScoringLevel");
        }

        HatScore score = Resources.Load<HatScore>(GameConstants.HAT_SCORE_PATH);
        if(score)
        {
            foreach (var hatData in score.GetHats())
            {
                var path = $"Hats/{hatData.Name}_Hat";
                var toLoad = Resources.Load<GameObject>(path);
                if(toLoad==null)
                {
                    Debug.Log($"<color=yellow> couldnt load hat </color> at : {path}");
                    continue;
                }
                var newHat=Instantiate(toLoad, this.transform);
                newHat.transform.localPosition = new Vector3(0, 1, 0);
                _hatsToScore.Add(newHat);
            }
        }

        ///Remove the hat entries from last level
        score.Clear();
    }

    private void Start()
    {
        _dummy = GetComponent<CharMovement>();
        _dummy.SetInput(1); ///make him run to the right
        int count = _hatsToScore.Count;
        if (count > 0)
        {
            var dis = Vector3.Distance(_dummy.transform.position, _endGoal.position);
            var tickRate = dis / (GetComponent<Stats>().CurrentMoveSpeed * count);
            //Debug.Log(tickRate);
            _scoreCounter.Initialize(_hatsToScore, tickRate, GetComponentInChildren<HatManager>());
        }
    }

    private void Update()
    {
        if (Vector3.Distance(_dummy.transform.position, _endGoal.position) < 1)
        {
            _dummy.SetInput(0); ///make him stop
                                ///Tell level to load Next
            if (!_isFinalLevel)
            {
                StartCoroutine(LoadNext());
            }
            else
            {
                ///Show quit Menu?
            }
        }
    }

    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(1);
        LevelLoader.Instance.LoadNextLevel();
    }

}
