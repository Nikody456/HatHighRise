using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIScanState : AIState
    {
        AIInput _ai;
        AISecurityCamInput _secCamInputAi;
        /*************************************************************************************************************/

        public AIScanState(AIInput ai)
        {
            _ai = ai;
            _secCamInputAi=_ai as AISecurityCamInput;
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
            _ai.SetMovement(PickADirection(_ai.transform.position));
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            if (!CheckExitConditions(target))
            {
                TryRayCastForTarget();
            }
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if (target != null)
            {
                return  _ai.SetState(eAIStates.ATTACK);
            }
            else if (!_secCamInputAi.IsScanning)
            {
                return  _ai.SetState(eAIStates.IDLE);
            }

            return false;
        }

        protected virtual float PickADirection(Vector3 pos)
        {
            ///Pick a Random Direction left or right
            System.Random random = new System.Random();
            return random.Next()%2 ==0 ? -1 : 1;

        }

        private void TryRayCastForTarget()
        {
            _secCamInputAi.RayCastScanDirection();
        }
    }
}