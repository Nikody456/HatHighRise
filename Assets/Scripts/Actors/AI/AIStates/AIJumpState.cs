#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System;
using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    public class AIJumpState : AIState
    {

        AICharInput _ai;
        Func<bool> _doJump;

        public AIJumpState(AICharInput ai, Func<bool> actionCall)
        {
            _ai = ai;
            _doJump = actionCall;
        }

        /*************************************************************************************************************/

        public override bool CanExit(eAIStates nextState)
        {
            return true;
        }

        public override void OnDisable(Transform target)
        {
           
        }

        public override void OnEnable(Transform target)
        {
         
        }
        public override void Execute(Transform target)
        {
            ///TRY DO JUMP
            if(_doJump())
                _ai.SetState(eAIStates.IDLE);
        }
        /*************************************************************************************************************/

    }
}
