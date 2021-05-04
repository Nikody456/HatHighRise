#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicScroller : MonoBehaviour
{
    [SerializeField] int _posToScrollToX;
    [Range(0.1f, 1f)]
    [SerializeField] float _scrollSpeed;

    float _extraScrollSpeed;
    private bool _enabled = false;
    private bool _paused;

    private void Start()
    {
        StartCoroutine(StartDelay());
    }

    public void LateUpdate()
    {

        if (_enabled)
        {
            if (CheckPaused())
                return;

            CalculateExtraScrollSpeed();
            var myPos = transform.localPosition;
            if (myPos.x != Mathf.Infinity)
            {
                bool keepScrolling = myPos.x > _posToScrollToX;
                if (keepScrolling)
                {
                    this.transform.localPosition = new Vector3(myPos.x - _scrollSpeed + _extraScrollSpeed, 0, 0);
                }
                else
                {
                    StartCoroutine(LoadDelay());
                    _enabled = false;
                }
            }
        }

        _extraScrollSpeed = _extraScrollSpeed / 2;
    }

    private bool CheckPaused()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            _paused = !_paused;
        }
        return _paused;
    }

    private void CalculateExtraScrollSpeed()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _extraScrollSpeed += -_posToScrollToX / 4 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _extraScrollSpeed += _posToScrollToX / 4 * Time.deltaTime;
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        _enabled = true;
    }

    IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(2);
        LevelLoader.Instance.LoadFirstLevel();
    }
}
