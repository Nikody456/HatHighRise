﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AISecCamNotifyState : AIState
    {
        AIInput _ai;

        private float _timeInState = 0;
        private float _timeToLeaveState = 5;
        private bool _sentNotification = false;
        /*************************************************************************************************************/

        public AISecCamNotifyState(AIInput ai)
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
            _timeInState += Time.deltaTime;
            return false;
        }


        protected virtual void DoAttack()
        {
            ///Only notify once per time through this state
            if (_sentNotification)
                return;

            Debug.Log($"<color=red> Notify!</color>");
            //Notify someone to spawn guards TODO

            _sentNotification = true;

        }
    }
}