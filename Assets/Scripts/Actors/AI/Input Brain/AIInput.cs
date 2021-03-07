using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(ActorMovement))]
    public abstract class AIInput : MonoBehaviour
    {
        [SerializeField] protected Transform _target;
        [SerializeField] protected float _detectionRange;
        [SerializeField] protected float _atkRange;

        protected ActorMovement _movement;

        protected AIState _currentState;
        protected AIState _idleState;
        protected AIState _moveState;
        protected AIState _deathState;
        protected AIState _attackState;

        bool sceneIsLoading = false;
        /***********INIT**************************************************************************************************/
        protected virtual void Awake()
        {
            _movement = GetComponent<ActorMovement>();
            CreateStates();
           
        }
        private void OnEnable()
        {
            LevelLoader.Instance.OnSceneIsLoading += OnSceneLoad;
        }


        protected abstract void CreateStates();
        /*************************************************************************************************************/

        protected virtual void Update()
        {
            if (!sceneIsLoading)
            {
                _currentState.Execute(_target);
            }

            //Debug.Log($"Curr statename={_currentState.ToString()}");

        }
        /*************************************************************************************************************/

        public void SetState(AIState.eAIStates state)
        {

            if(_currentState.CanExit(state))
            {
                AIState nextState = GetAIState(state);
                if (nextState!=null)
                {
                    _currentState.OnDisable(_target);
                    _currentState = nextState;
                    _currentState.OnEnable(_target);
                }
            }
        }

        public float DetectionRange => _detectionRange;
        public float AttackRange => _atkRange;
        public void SetMovement(float dir)
        {
            _movement.SetInput(dir);
        }
        /*************************************************************************************************************/

        protected abstract AIState GetAIState(AIState.eAIStates nextState);

        private void OnDisable()
        {
            LevelLoader.Instance.OnSceneIsLoading -= OnSceneLoad;
        }

        private void OnSceneLoad(bool cond)
        {
            sceneIsLoading = cond;
        }
    }
}