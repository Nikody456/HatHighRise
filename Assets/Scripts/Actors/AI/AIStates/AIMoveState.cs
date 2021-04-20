using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIMoveState : AIState
    {
        AIInput _ai;
        LayerMask _groundLayer;
        float _howFarToCheckInFront = 1f;
        float _howFarToCheckDown = 2f;

        bool _isWandering = false;
        float _timeInState = 0f;
        float _timeToStopWander = 2f;

        float _moveDir = 1;
        /*************************************************************************************************************/

        public AIMoveState(AIInput ai, LayerMask groundLayer)
        {
            _ai = ai;
            _groundLayer = groundLayer;
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
            _timeInState = 0;
            SetMovementDir(target);
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            _isWandering = target == null;
            if (!CheckExitConditions(target))
            {
                _ai.SetMovement(_moveDir);
            }
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if (_isWandering)
            {
                _timeInState += Time.deltaTime;
                if (_timeInState > _timeToStopWander)
                {
                    return _ai.SetState(eAIStates.IDLE);
                }
            }
            else
            {
                if (Mathf.Abs(Vector3.Distance(_ai.transform.position, target.position)) > _ai.DetectionRange)
                {
                    return _ai.SetState(eAIStates.IDLE);
                }
                if (Mathf.Abs(Vector3.Distance(_ai.transform.position, target.position)) <= _ai.AttackRange)
                {
                    return _ai.SetState(eAIStates.ATTACK);
                }
            }

            if (CheckIfObstacleToJumpOver())
            {
                return _ai.SetState(eAIStates.JUMP);
            }
            if (GoingToFallOffEdge())
            {
                return _ai.SetState(eAIStates.IDLE);
            }
            return false;
        }

        protected virtual float PickADirection(Vector3 pos)
        {
            ///MOVE TOWARDS
            var dir = (_ai.transform.position.x > pos.x);

            return dir ? -1 : 1;

        }

        protected void SetMovementDir(Transform target)
        {
            if(target==null)
            {
                _moveDir = _ai.FacingDir == Vector3.right ? -1 : 1;
            }
            else
            {
                _moveDir= PickADirection(target.position);
            }
        }

        private bool CheckIfObstacleToJumpOver()
        {
            return false;
        }

        private bool GoingToFallOffEdge()
        {
            Vector3 posInFront = (_ai.transform.position + _ai.FacingDir) * _howFarToCheckInFront;
            bool hitSomething = false;
            Vector2 origin2D = new Vector2(posInFront.x, posInFront.y);
            var direction2D = Vector2.down;

            if (Physics2D.Raycast(origin2D, direction2D, _howFarToCheckDown, _groundLayer))
            {
                hitSomething = true;
                //Debug.DrawRay(posInFront, direction2D, Color.blue, 2);
            }
            else
            {
                //Debug.DrawRay(posInFront, direction2D, Color.red, 2);
            }
            //Debug.Log($"Going to fall off edge= {hitSomething}");
            return !hitSomething;

        }
    }
}