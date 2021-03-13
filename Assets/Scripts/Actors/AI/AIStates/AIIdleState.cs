using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIIdleState : AIState
    {
        AIInput _ai;
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
            _ai.SetMovement(0);
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            if (!CheckExitConditions(target))
            {
                _ai.SetMovement(0);
            }
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if (target == null)
            {
                return TryFindTarget();
            }
            if (Vector3.Distance(_ai.transform.position, target.position) < _ai.DetectionRange)
            {
                _ai.SetState(eAIStates.MOVE);
                return true;
            }
            else
            {
                Debug.Log($"Dis= {Vector3.Distance(_ai.transform.position, target.position)} vs {_ai.DetectionRange}");
            }

            return false;
        }

        private bool TryFindTarget()
        {
            GameObject found = null;
            ///RayCast facing DIR
            var ourPos = _ai.transform.position;
            Vector3 facingDir = _ai.FacingDir;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var depth = ourPos + (facingDir * _ai.DetectionRange);
            Debug.DrawLine(ourPos, depth, Color.red, 1);
            int numHits = (Physics2D.Raycast(ourPos, facingDir, _ai.DetectionInfo, results, _ai.DetectionRange));
            for (int i = 0; i < numHits; i++)
            {
                RaycastHit2D hit = results[i];
                if (hit.collider != null)
                {
                    Debug.Log($"Dir={facingDir}, #{numHits}, Detected Hit: {hit.collider.gameObject.name} !");
                    ///We are not ourself
                    if (hit.collider.gameObject != _ai.gameObject)
                    {
                        found = hit.collider.gameObject;
                    }
                }
            }
            if (found)
            {
                _ai.SetTarget(found.transform);
            }

            return found!=null;
        }
    }
}