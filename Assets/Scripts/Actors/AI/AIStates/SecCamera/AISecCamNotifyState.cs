
using UnityEngine;
using System;

namespace AI
{
    public class AISecCamNotifyState : AIState
    {
        AIInput _ai;

        private float _timeInState = 0;
        private float _timeToLeaveState = 5;
        private bool _sentNotification = false;
        private Action _onNotify;
        private int _maxGuards = 3;
        /*************************************************************************************************************/

        public AISecCamNotifyState(AIInput ai, Action onNotify)
        {
            _ai = ai;
            _onNotify = onNotify;
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
            _sentNotification = false;
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            if (!CheckExitConditions(target))
            {
                DoAttack();
            }
        }

        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
            if(target==null)
            {
                return true;
            }
            if(_timeInState > _timeToLeaveState)
            {
                _ai.SetTarget(null);
                _ai.SetState(eAIStates.IDLE);
                return true;
            }
            if(MakeSureTooManyGuardsDontExist())
            {
                return true;
            }
            _timeInState += Time.deltaTime;
            return false;
        }


        private bool MakeSureTooManyGuardsDontExist()
        {
            var ffs = _ai as AISecurityCamInput;
            int guardsInArea = ffs.SphereCastAtDoorLocation();
            //Debug.Log($"guardsInArea = {guardsInArea} .. = {guardsInArea > _maxGuards}");
            return guardsInArea > _maxGuards;
        }

        protected virtual void DoAttack()
        {
            ///Only notify once per time through this state
            if (_sentNotification)
                return;

            //Debug.Log($"<color=red> Notify!</color>");
            //Notify someone to spawn guards
            _onNotify?.Invoke();
            _sentNotification = true;

            //could do this? if we dont want to play some kind of blinking light?
            //_timeInState = _timeToLeaveState;

        }
    }
}