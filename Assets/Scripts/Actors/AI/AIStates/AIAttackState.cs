using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIAttackState : AIState
    {
        AIInput _ai;
        System.Action _OnAttack;

        private float _attackDelayMax = 0.5f;
        private float _attackDelay = 0;

        /*************************************************************************************************************/

        public AIAttackState(AIInput ai, System.Action attackCall)
        {
            _ai = ai;
            _OnAttack = attackCall;
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
            _attackDelay = 0;
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
         
            if (target==null)
            {
                return _ai.SetState(eAIStates.IDLE);
            }
            ///Obv refactor this
            else if (Vector3.Distance(_ai.transform.position, target.position) > _ai.DetectionRange)
            {
                return _ai.SetState(eAIStates.IDLE);
            }
            else if(Vector3.Distance(_ai.transform.position, target.position) > _ai.AttackRange)
            {
                return  _ai.SetState(eAIStates.MOVE);
            }
            if(_attackDelay < _attackDelayMax)
            {
                FaceTarget(target);
                _attackDelay += Time.deltaTime;
                _ai.SetMovement(0);
                return true;
            }

            return false;
        }

        protected void FaceTarget(Transform target)
        {
            bool dir = (_ai.transform.position.x > target.position.x);
            var faceDir = dir ? -1 : 1;
            if (_ai.FacingDir.x != faceDir)
            {
                _ai.SetMovement(faceDir);
            }
        }
        

        protected virtual void DoAttack()
        {
            _OnAttack();
            _ai.SetMovement(0);
            _attackDelay = 0;
        }
    }
}