using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamView : ActorView
{
    public string Scan => SCAN;
    private const string SCAN = "Scan";

   public void SetScanning()
    {
        Debug.Log("we set scanning!");
        _animator.SetTrigger(SCAN);
    }
}
