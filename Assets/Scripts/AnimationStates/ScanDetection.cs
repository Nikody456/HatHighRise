using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
public class ScanDetection : StateMachineBehaviour
{
    [SerializeField] AISecurityCamInput _secCamera;
    [SerializeField] SecurityCamView _view;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ValidateSecCam(animator))
        {
            _secCamera.SetIsScanning(true);
        }
        if (ValidateSecView(animator))
        {
            animator.ResetTrigger(_view.Scan);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ValidateSecCam(animator))
        {
            _secCamera.SetIsScanning(false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private bool ValidateSecCam(Animator animator)
    {
        if (_secCamera == null)
        {
            _secCamera=animator.transform.GetComponent<AISecurityCamInput>();
        }

        return _secCamera != null;
    }

    private bool ValidateSecView(Animator animator)
    {
        if (_view == null)
        {
            _view = animator.transform.GetComponent<SecurityCamView>();
        }
        return _view != null;
    }
}
