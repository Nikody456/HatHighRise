using Statistics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public Vector3 PlayerStart => _playerStart.position;
    [SerializeField] Transform _playerStart;

    public Vector3 LevelEnd => _levelEnd.position;
    [SerializeField] Transform _levelEnd = default;


    [SerializeField] List<LevelDeathPlane> _deathPlanes = default;
    [SerializeField] GameObject _playerPreFab = default;


    private GameObject _activePlayer;

    private void Awake()
    {
       if(_playerPreFab==null)
        {
            _playerPreFab = Resources.Load<GameObject>("Player_Prefab");
        }
       if(_deathPlanes.Count==0)
        {
            _deathPlanes = this.GetComponentsInChildren<LevelDeathPlane>().ToList();
        }
    }


    IEnumerator Start()
    {
        foreach (var deathPlane in _deathPlanes)
        {
            deathPlane.OnDeathPlaneTouched += ResetPlayer;
        }  
        yield return new WaitForSeconds(0.5f);
        StartLevel();
    }
    public void StartLevel()
    {
        ///Instantiate the player at the Starting Position?
        _activePlayer= Instantiate(_playerPreFab, PlayerStart, Quaternion.identity);
        ///TOTAL HACK
        Stats stats = _activePlayer.GetComponent<Stats>();
        stats.OnPlayerResetHack += ResetPlayer;


    }



    public void ResetPlayer(GameObject go) ///We have to match delegate signature
    {
        /// Jaden was doing this, but I think it looks better w.o it, kind of lerps down in a smooth way
        //CameraController cam = go.GetComponentInChildren<CameraController>();
        //if (cam)
        //{
        //    cam.resetCamera();
        //}
        PlayerMonitor pm = _activePlayer.GetComponent<PlayerMonitor>();
        if (pm)
        {
            pm.ResetPlayer();
        }
        ///Call this after reset so we can use players last known Location:
        _activePlayer.transform.position = PlayerStart;

        ///TODO reset the deathBool for anim state and anything else that becomes needed
    }

    private void OnDisable()
    {
        foreach (var deathPlane in _deathPlanes)
        {
            deathPlane.OnDeathPlaneTouched -= ResetPlayer;
        }
    }

}
