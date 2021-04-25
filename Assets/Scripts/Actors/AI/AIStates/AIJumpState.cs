#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    public class AIJumpState : AIState
    {
        AICharInput _aiChar;
        Func<bool> _doJump;

        public AIJumpState(AICharInput ai, Func<bool> actionCall)
        {
            _ai = ai;
            _aiChar = ai;
            _doJump = actionCall;
        }

        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            var moveDir = _aiChar.FacingDir == Vector3.right ? -1 : 1;
            _aiChar.SetMovement(moveDir);
            ///TRY DO JUMP
            if (!CheckExitConditions(target) && _doJump())
            {
                //If can Jump Again... try it after short delay?
                if (_aiChar.GetComponent<Stats>().CurrentJumpLimit > 1)
                {
                    _aiChar.AttemptDelayedDoubleJump();
                }
            }
                    _aiChar.SetState(eAIStates.IDLE);
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if (target)
            {
                if (Mathf.Abs(Vector3.Distance(_ai.transform.position, target.position)) > _ai.DetectionRange)
                {
                    return _aiChar.SetState(eAIStates.IDLE);
                }
                if (Mathf.Abs(Vector3.Distance(_ai.transform.position, target.position)) <= _ai.AttackRange)
                {
                    return _aiChar.SetState(eAIStates.ATTACK);
                }
                if (!TargetIsAboveMeVertically(target))
                {
                    return _aiChar.SetState(eAIStates.IDLE);
                }
            }

            return false;
        }

    }
}
