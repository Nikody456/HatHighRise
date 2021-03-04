using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public Vector3 PlayerStart => _playerStart.position;
    [SerializeField] Transform _playerStart;

    public Vector3 LevelEnd => _levelEnd.position;
    [SerializeField] Transform _levelEnd;

    [SerializeField] GameObject _playerPreFab;

    private GameObject _activePlayer;

    private void Awake()
    {
       if(_playerPreFab==null)
        {
            _playerPreFab = Resources.Load<GameObject>("Player_Prefab");
        }
    }


    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        StartLevel();
    }
    public void StartLevel()
    {
        ///Instantiate the player at the Starting Position?
        _activePlayer= Instantiate(_playerPreFab, PlayerStart, Quaternion.identity);
    }

    public void ResetPlayer()
    {
        _activePlayer.transform.position = PlayerStart;
        ///TODO reset the deathBool for anim state and anything else
    }


    ///HACK FOR TESTING
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetPlayer();
    }

}
