#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using System.Collections;
using UnityEngine;

public class ScoringDummyMovement : MonoBehaviour
{

    [SerializeField] Transform _endGoal = default;
    private CharMovement _dummy;

    private void Start()
    {
        _dummy = GetComponent<CharMovement>();
        _dummy.SetInput(1); ///make him run to the right
    }

    private void Update()
    {
        if(Vector3.Distance(_dummy.transform.position, _endGoal.position) < 1)
        {
            _dummy.SetInput(0); ///make him stop
            ///Tell level to load Next
            StartCoroutine(LoadNext());
        }
    }

    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(1);
        LevelLoader.Instance.LoadNextLevel();
    }

}
