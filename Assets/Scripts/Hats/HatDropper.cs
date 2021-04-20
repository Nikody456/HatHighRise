#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatDropper : MonoBehaviour
{

    [SerializeField]
    List<GameObject> _hatDummyPrefabs = new List<GameObject>();

    [SerializeField]
    int dummyHatLayer = 13;

    int _numberOfBursts = 3;

    int count = 0;
    System.Random _rng;

    IEnumerator Start()
    {
        _rng = new System.Random();
        yield return new WaitForEndOfFrame();
        while (_numberOfBursts > 0)
        {
            StartCoroutine(HatBurst());
            yield return new WaitForSeconds(1);
            --_numberOfBursts;
        }
    }

    IEnumerator HatBurst()
    {
        Queue<GameObject> hatQueue = new Queue<GameObject>();
        _hatDummyPrefabs.ForEach(hat => hatQueue.Enqueue(hat));
        yield return new WaitForEndOfFrame();
        while (hatQueue.Count > 0)
        {
            Debug.Log($"hatQueue count = {hatQueue.Count}");
            DropRandomHat(hatQueue);
            yield return new WaitForEndOfFrame();
        }
    }

    public void DropRandomHat(Queue<GameObject> hats)
    {
        ++count;
        var HATPRFAB = hats.Dequeue();
        var hat = GameObject.Instantiate(HATPRFAB);
        hat.name = "hat " + count;
        var hatComponent = hat.GetComponent<Hat>();
        hat.gameObject.layer = dummyHatLayer;
        if (hatComponent)
        {
            Destroy(hatComponent); ///So we dont pick this up
        }
        hat.transform.position = GetRandomPos();
        var rb = hat.GetComponent<Rigidbody2D>();
        if (rb)
        {
            var velo = GetRandomVelocity();
            rb.AddForce(velo, ForceMode2D.Impulse);
            //var veloComp = hat.AddComponent<HatDebugVelo>();
            //veloComp.SetVelo(velo);
        }
    }

    private Vector3 GetRandomPos()
    {
        var transPos = this.transform.position;
        float dec = (float)_rng.Next(0, 10) / 10; ///Get some RNG on the float
        float x = (_rng.Next(-20, 14) + dec) + transPos.x;
        float y = _rng.Next(1, 15) + transPos.y;
        return new Vector3(x, y, 0);
    }

    private Vector3 GetRandomVelocity()
    {
        float dec = (float)_rng.Next(0, 10) / 10; ///Get some RNG on the float
        float x = (_rng.Next(-10, 10) + dec);
        float y = _rng.Next(-5, 5);
        return new Vector3(x, y, 0);
    }
}

