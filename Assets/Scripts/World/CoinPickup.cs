#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    [SerializeField] int _myScoreValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerMonitor = collision.gameObject.GetComponent<PlayerMonitor>();
        if(playerMonitor != null)
        {
            playerMonitor.AlterScoreThisLevel(_myScoreValue);
            Destroy(this.gameObject);
        }
    }
}
