#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicScroller : MonoBehaviour
{
    [SerializeField] int _posToScrollToX;
    [Range(0.1f, 1f)]
    [SerializeField] float _scrollSpeed;

    private bool _enabled = false;

    private void Start()
    {
        StartCoroutine(StartDelay());
    }

    public void LateUpdate()
    {
        if (_enabled)
        {
            var myPos = transform.localPosition;
            if (myPos.x != Mathf.Infinity)
            {
                bool keepScrolling = myPos.x > _posToScrollToX;
                if (keepScrolling)
                {

                    this.transform.localPosition = new Vector3(myPos.x - _scrollSpeed, 0, 0);
                    Debug.Log($" {myPos.x} is > {_posToScrollToX} = <color=red>{keepScrolling} </color>");
                }
                else
                {
                    Debug.Log($" {myPos.x} is > {_posToScrollToX} = <color=green>{keepScrolling} </color>");
                    StartCoroutine(LoadDelay());
                    _enabled = false;
                }
            }
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
