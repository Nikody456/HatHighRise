using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIISecurityCamIdleState : AIState
    {
        AIInput _ai;
        float _timeInState=0;
        readonly float _SEARCH_FREQUENCY;
        /***********INIT**************************************************************************************************/

        public AIISecurityCamIdleState(AIInput ai, float searchFrequency)
        {
            _ai = ai;
            _SEARCH_FREQUENCY = searchFrequency;
        }
        /*************************************************************************************************************/

        public override bool CanExit(eAIStates nextState)
        {
            return true;
        }
        public override void OnDisable(Transform target)
        {
            _timeInState = 0;
        }
        public override void OnEnable(Transform target)
        {
            _ai.SetMovement(0);
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            if(!CheckExitConditions(target))
            {
                _ai.SetMovement(0);
            }
        }
        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if(target!=null)
            {
                _ai.SetState(eAIStates.ATTACK);
                return true;
            }
            else if (CheckTime())
            {
                _ai.SetState(eAIStates.MOVE);
                return true;

            }

            return false;
        }

        private bool CheckTime()
        {
            if (_timeInState > _SEARCH_FREQUENCY)
            {
                return true;
            }

            _timeInState += Time.deltaTime;
            return false;
        }

    }
}