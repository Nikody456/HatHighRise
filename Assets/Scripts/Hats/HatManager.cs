using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField] List<Hat> _hats = new List<Hat>();

    private float _characterHeight = 0.5f;
    private float _yOffset = 0.25f;

    public void OnPickUpHat(Hat hat)
    {
        //Debug.Log("I am picking up a hat");
        hat.transform.parent = this.transform;
        hat.transform.localPosition = new Vector3(0, GetHeight(_hats.Count), 0);
        hat.SetOrderInSortingLayer(_hats.Count);
        _hats.Add(hat);
    }


    public void OnPutDownHat(Hat hat)
    {
        hat.transform.parent = null;
        _hats.Remove(hat);
        ReOrderHats();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //OnPutDownHat(_hats[0]);
        }

    }

    /***********PRIVATE HELPERS**************************************************************************************************/


    private float GetHeight(int index)
    {
        return _characterHeight + (index * _yOffset);
    }

    private void ReOrderHats()
    {
        for (int i = 0; i < _hats.Count; ++i)
        {
            Hat hat = _hats[i];
            hat.transform.localPosition = new Vector3(0, GetHeight(i), 0);
        }
    }
}
