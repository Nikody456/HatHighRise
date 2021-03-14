using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public Vector3 PlayerStart => _playerStart.position;
    [SerializeField] Transform _playerStart;

    public Vector3 LevelEnd => _levelEnd.position;
    [SerializeField] Transform _levelEnd = default;


    [SerializeField] LevelDeathPlane _deathPlane = default;
    [SerializeField] GameObject _playerPreFab = default;


    private GameObject _activePlayer;

    private void Awake()
    {
       if(_playerPreFab==null)
        {
            _playerPreFab = Resources.Load<GameObject>("Player_Prefab");
        }
       if(_deathPlane==null)
        {
            _deathPlane = this.GetComponentInChildren<LevelDeathPlane>();
        }
    }


    IEnumerator Start()
    {
        if(_deathPlane)
        {
            _deathPlane.OnDeathPlaneTouched += ResetPlayer;
        }    
        yield return new WaitForSeconds(0.5f);
        StartLevel();
    }
    public void StartLevel()
    {
        ///Instantiate the player at the Starting Position?
        _activePlayer= Instantiate(_playerPreFab, PlayerStart, Quaternion.identity);
    }

    public void ResetPlayer(GameObject go) ///We have to match delegate signature
    {
        _activePlayer.transform.position = PlayerStart;
        /// Jaden was doing this, but I think it looks better w.o it, kind of lerps down in a smooth way
        CameraController cam = go.GetComponentInChildren<CameraController>();
        if (cam)
        {
            //cam.resetCamera();
        }
        ///TODO reset the deathBool for anim state and anything else that becomes needed
        

    }

    private void OnDisable()
    {
        if (_deathPlane)
        {
            _deathPlane.OnDeathPlaneTouched -= ResetPlayer;
        }
    }

}
