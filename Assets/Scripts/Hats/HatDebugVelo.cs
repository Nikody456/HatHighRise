#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatDebugVelo : MonoBehaviour
{

    public Vector3 _veloToSet;
    public void SetVelo(Vector3 velo)
    {
        _veloToSet = velo;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        this.GetComponent<Rigidbody2D>().velocity = _veloToSet;
    }
}
