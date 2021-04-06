#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncriminator : MonoBehaviour
{
    [SerializeField] Transform _endPos = default;
    [SerializeField] HatManager _hatManager = default;
    List<GameObject> _itemsToScore;
    float _currTime = 0;
    float _tickRate = 0;
    bool _initialized = false;


    public void Awake()
    {
        
        var totalScore = PlayerPrefs.GetInt(GameConstants.HAT_SCORE_KEY);
        GameCanvas.Instance.UpdateScore(totalScore);
    }

    public void Initialize(List<GameObject> itemsToScore, float tickRate, HatManager hatManager)
    {
        _itemsToScore = itemsToScore;
        _hatManager = hatManager;
        _tickRate = tickRate;
        _currTime = _tickRate;
        _initialized = true;
    }


    void Update()
    {
        if (!_initialized)
            return;

        _currTime += Time.deltaTime;
        if (_currTime >= _tickRate)
        {
            _currTime = 0;
            ScoreNextItem();
            if(_itemsToScore.Count==0)
            {
                _initialized = false;
            }
        }
    }

    void ScoreNextItem()
    {
        var nextItem = _itemsToScore[_itemsToScore.Count-1];
        _itemsToScore.Remove(nextItem);
        StartCoroutine(MoveToScoreText(nextItem));
    }
    
    IEnumerator MoveToScoreText(GameObject item)
    {
        
        var hat = item.GetComponent<Hat>();
        _hatManager.OnPutDownHat(hat);
        Destroy(hat);
        Destroy(item.GetComponent<Rigidbody2D>());
        var itemTransform = item.transform;
        item.transform.parent = null;
        while(itemTransform.position != _endPos.position)
        {
            itemTransform.position = Vector3.MoveTowards(itemTransform.position, _endPos.position, 10 * _tickRate* Time.deltaTime);
            //itemTransform.position = Vector3.Lerp(itemTransform.position, _endPos.position, 0.1f);
            yield return new WaitForSeconds(Time.deltaTime );
        }
        Destroy(item);
        var gc = GameCanvas.Instance;
        gc.IncreaseScore(GameConstants.HAT_SCORE);
        PlayerPrefs.SetInt(GameConstants.HAT_SCORE_KEY, gc.GetScore());

    }

}

