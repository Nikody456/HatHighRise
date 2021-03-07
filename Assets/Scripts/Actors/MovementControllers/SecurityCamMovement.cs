using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamMovement : ActorMovement
{
     SecurityCamView _view;

    private void Awake()
    {
        _view = this.GetComponent<SecurityCamView>();
    }



    public override void SetInput(float newInput)
    {
        _input = newInput;

        if (_input != 0)
        {
            _view.SetScanning();
        }
    }


}
