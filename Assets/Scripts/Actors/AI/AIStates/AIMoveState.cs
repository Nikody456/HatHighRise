using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIMoveState : AIState
    {

        LayerMask _groundLayer;

        bool _isWandering = false;
        float _timeInState = 0f;
        float _timeToStopWander = 2f;
        float _timeAboveMeVertically = 0;
        float _timeToGiveUpVertically = 2;

        float _moveDir = 1;
        float _randomMoveDir = 0;
        float _randomMoveTime = 0;
        float _randomRefreshTime = 100;
        /*************************************************************************************************************/

        public AIMoveState(AIInput ai, LayerMask groundLayer, ContactFilter2D contactFilter)
        {
            _ai = ai;
            _groundLayer = groundLayer;
            _contactFilter = contactFilter;
        }
        /*************************************************************************************************************/

        public override void OnEnable(Transform target)
        {
            _timeInState = 0;
            _timeAboveMeVertically = 0;
            _randomMoveTime = 0;
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            SetMovementDir(target);
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
                    if (MultipleGuardsOnTopOfMe())
                    {
                        float randTime = ((float)new System.Random().Next(1, 100)) / 10;
                        _randomMoveTime += _timeInState + randTime;
                        _ai.SetMovement(_randomMoveDir);

                    }
                    else
                    {
                        return _ai.SetState(eAIStates.IDLE);
                    }
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
                if (TargetIsAboveMeVertically(target))
                {
                    if (CheckIfObstacleToJumpOver())
                    {
                        return _ai.SetState(eAIStates.JUMP);
                    }
                    _timeAboveMeVertically += Time.deltaTime;
                    if (_timeAboveMeVertically > _timeToGiveUpVertically)
                    {
                        return _ai.SetState(eAIStates.IDLE);
                    }
                }
                else
                    _timeAboveMeVertically = 0;

            }

            if (GoingToFallOffEdge())
            {
                return _ai.SetState(eAIStates.IDLE);
            }
            if (GoingToHitWall())
            {
                if (CheckIfObstacleToJumpOver())
                {
                    _ai.SetMovement(_moveDir);
                    return _ai.SetState(eAIStates.JUMP);
                }
                //SwitchFacingDir();
                _randomMoveDir = 0;
                Debug.Log($"Move 1");
                return _ai.SetState(eAIStates.IDLE);
            }


            return false;
        }

        private bool CheckIfObstacleToJumpOver()
        {
            bool canJumpOver = false;
            Vector3 posInFront = (_ai.transform.position + _ai.FacingDir);
            Vector2 origin2D = new Vector2(posInFront.x, posInFront.y);
            Vector2 dir2D = new Vector2(_ai.FacingDir.x, 0);
            var howFarToCheckHoriz = _howFarToCheckDown;
            //if (Physics2D.Raycast(origin2D, _ai.FacingDir, howFarToCheckHoriz, _groundLayer))
            //RaycastHit2D[] results;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            Debug.DrawRay(origin2D, _ai.FacingDir * howFarToCheckHoriz, Color.blue, 2);
            var hits = Physics2D.Raycast(origin2D, dir2D, _contactFilter, results, howFarToCheckHoriz);
            ///This might be problematic as canJumpverOver can get set to true once and never undo?
            foreach (var hit in results)
            {
                var hitPos = hit.point;
                var direction2DVert = Vector2.up;
                var howFarToCheckUp = howFarToCheckHoriz;
                Vector2 jumpLocationGoal = hitPos + new Vector2(0, howFarToCheckUp);
                Vector2 displacementDir = jumpLocationGoal - origin2D;

                Debug.DrawRay(origin2D, displacementDir * howFarToCheckHoriz, Color.yellow, 2);
                if (!Physics2D.Raycast(origin2D, displacementDir, howFarToCheckHoriz, _groundLayer))
                {
                    Debug.Log($"The guard HIT {hit.collider.gameObject} .. trying to jump over it:");
                    canJumpOver = true;
                }
            }



            return canJumpOver;
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

        protected void SetMovementDir(Transform target)
        {
            if (target == null)
            {
                if (_moveDir == 0)
                    _moveDir = _ai.FacingDir == Vector3.right ? -1 : 1;
                if (_randomMoveTime > _randomRefreshTime)
                {
                    _randomMoveTime = 0;
                    //Total Hacky nonsense
                    int even = new System.Random().Next(0, 10);
                    if (even == 2)
                        _randomMoveDir = 0;
                    else
                        _randomMoveDir = even % 2 == 0 ? -1 : 1;

                    if(randDirWillHitWall())
                    {
                        _randomMoveDir = 0;
                    }
                }
            }
            else
            {
                _moveDir = PickADirection(target.position);
            }
        }

        private bool randDirWillHitWall()
        {
            ///MY BRAIN IS FRIED 
            Vector3 ranDir = new Vector3(_randomMoveDir, 0, 0);
            Vector3 posInFront = (_ai.transform.position + ranDir);
            Vector2 origin2D = new Vector2(posInFront.x, posInFront.y);
            Vector2 dir2D = new Vector2(_randomMoveDir, 0);
            var howFarToCheckHoriz = _howFarToCheckDown / 2;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            Debug.DrawRay(origin2D, ranDir * howFarToCheckHoriz, Color.green, 1);
            var hits = Physics2D.Raycast(origin2D, dir2D, _contactFilter, results, howFarToCheckHoriz);



            return hits!=0;
        }
    }
}