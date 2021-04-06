using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelEndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<PlayerMovement>())
        {
            var hatManager = collision.GetComponentInChildren<HatManager>();
            if(hatManager)
            {
                hatManager.SaveHatData();
            }
            LevelLoader.Instance.LoadNextLevel();
        }
    }
}
