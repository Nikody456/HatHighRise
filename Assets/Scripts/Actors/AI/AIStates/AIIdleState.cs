using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIIdleState : AIState
    {
        AIInput _ai;

        float _timeInState = 0f;
        float _timeToFlipDir = 10f;
        /***********INIT**************************************************************************************************/

        public AIIdleState(AIInput ai)
        {
            _ai = ai;
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
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {

            if (target == null)
            {
                if (_timeInState > _timeToFlipDir)
                {
                    SwitchFacingDir();
                    ///Return true to give us one frame of movement the other dir
                    ///otherwise we get set to 0 immediately after returning false
                    ///If we want AI to try to patrol, we can set another timer for how long they
                    ///go this DIR. or create a new patrol class
                    return true; 
                }
                return TryFindTarget();
            }
            if (Mathf.Abs(Vector3.Distance(_ai.transform.position, target.position)) < _ai.DetectionRange)
            {
                _ai.SetState(eAIStates.MOVE);
                return true;
            }

            return false;
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
                        else ///We hit an obstacle first, our view is blocked
                        {
                            Debug.DrawLine(ourPos, hit.point, Color.yellow, 1);

                            return false;
                        }
                    }
                }
            }

            return false;
        }

        protected void SwitchFacingDir()
        {
            if (_ai.FacingDir == Vector3.right)
            {
                _ai.SetMovement(-1);
            }
            else
            {
                _ai.SetMovement(1);
            }
            _timeInState = 0;
        }
    }
}