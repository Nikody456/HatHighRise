#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using Statistics;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Spike : MonoBehaviour
{
    [SerializeField] float _aboveHeightThreshold = 0.5f;
    Stats _myStats;

    private void Awake()
    {
        _myStats = this.GetComponent<Stats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool aboveMe = collision.transform.position.y > (transform.position.y + _aboveHeightThreshold);
        if (!aboveMe)
            return;
        ///let the AI hit this too
        var stats = collision.gameObject.GetComponent<Stats>();
        if (stats != null)
        {
            //Debug.Log("..someone1 HitSpike");
            HitManager.CalculateHit(_myStats, stats);
        }
    }
}
