using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIIdleState : AIState
    {

        float _timeInState = 0f;
        float _timeToFlipDir = 6f;
        float _timesFlipped = 0;
        float _timeToWander = 3;
        float _turningTime = 0.25f;
        bool _isTurning = false;

        /***********INIT**************************************************************************************************/

        public AIIdleState(AIInput ai, ContactFilter2D contactFilter)
        {
            _ai = ai;
            _contactFilter = contactFilter;
        }
        /*************************************************************************************************************/

        public override void OnEnable(Transform target)
        {
            _timeInState = 0;
            _timesFlipped = 0;
            _isTurning = false;
            _ai.SetMovement(0);
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            if (!CheckExitConditions(target))
            {
                _ai.SetMovement(0);
            }
            _timeInState += Time.deltaTime;
           // Debug.Log($"_timeInState= {_timeInState} ... timeFlip={_timesFlipped}");
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if (target == null)
            {
                bool canFlipAround = !GoingToHitWallBehind();
                if (_timeInState > _timeToFlipDir)
                {
                    if (canFlipAround)
                    {
                        SwitchFacingDir();
                        return true; ///Return true to give us one frame of movement the other dir
                    }
                    else
                    {
                        //Debug.Log($"{_ai.gameObject.name} cant flip");
                        return TryFindTarget();
                    }
                }
                else if (_timesFlipped > _timeToWander)
                {
                    //This is like a mega hack to get ourselves unstuck on walls
                    //since simply setting the _aiMoveDir doesnt work in 1 frame to switch dir.

                    if (canFlipAround)
                    {
                        System.Random rng = new System.Random();
                        if (rng.Next() % 2 == 0)
                        {
                            //Debug.Log($"<color=green> go wander</color>");

                            SwitchFacingDir();
                            --_timesFlipped;
                            return true; ///Return true to give us one frame of movement the other dir
                        }

                        return _ai.SetState(eAIStates.MOVE);

                    }
                    else
                    {
                        //Debug.Log($"{_ai.gameObject.name} cant flip");
                        TryFindTarget();
                    }
                }
                else if (_isTurning)
                {
                    if (_timeInState > _turningTime)
                        _isTurning = false;
                    return true;
                }
                else if (MultipleGuardsOnTopOfMe() && !GoingToHitWall() && !GoingToHitWallBehind())
                {
                    System.Random rng = new System.Random();
                    if (rng.Next() % 2 == 0)
                    {
                        _timeInState *= 2;
                    }

                }
                return TryFindTarget();
            }
            else ///Has Target
            {
                if (TargetIsBehindMe(target))
                {
                    SwitchFacingDir();
                    return true;
                }
                if (Mathf.Abs(Vector3.Distance(_ai.transform.position, target.position)) < _ai.DetectionRange)
                {

                    return _ai.SetState(eAIStates.MOVE);
                }
            }


            return false;
        }

        private bool TargetIsBehindMe(Transform target)
        {
            bool dir = (_ai.transform.position.x > target.position.x);
            var faceDir = dir ? -1 : 1;
            return (_ai.FacingDir.x != faceDir);
        }


        private bool TryFindTarget()
        {
            ///RayCast facing DIR
            var ourPos = _ai.transform.position;
            Vector3 facingDir = _ai.FacingDir;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            //Something in here is slightly off, raycast range is slightly farther than detectionRange like 15.3 vs 15
            int numHits = Physics2D.Raycast(ourPos, facingDir, _ai.DetectionInfo, results, _ai.DetectionRange);
            for (int i = 0; i < numHits; i++)
            {
                RaycastHit2D hit = results[i];
                if (hit.collider != null)
                {
                    //Debug.Log($"Dir={facingDir}, #NumHits={numHits}, Detected Hit: {hit.collider.gameObject.name} !");
                    ///We are not ourself
                    if (hit.collider.gameObject != _ai.gameObject)
                    {
                        if (hit.collider.gameObject.layer == GameConstants.PLAYER_LAYER)
                        {
                            _ai.SetTarget(hit.collider.gameObject.transform);
                            Debug.DrawLine(ourPos, ourPos + (facingDir * _ai.DetectionRange), Color.red, 1);

                        }
                        else ///We hit an obstacle first, our view might be  blocked
                        {
                            //Debug.DrawLine(ourPos, hit.point, Color.black, 1);
                            ///We can see thru other guards, but not walls:
                            if (hit.collider.gameObject.layer != GameConstants.AI_LAYER)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return false;
        }

        protected override void SwitchFacingDir()
        {
            base.SwitchFacingDir();
            _isTurning = true;
            _timeInState = 0;
            ++_timesFlipped;
        }
    }
}