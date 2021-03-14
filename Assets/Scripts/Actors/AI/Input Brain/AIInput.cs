using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(ActorMovement))]
    public abstract class AIInput : MonoBehaviour
    {
        [SerializeField] protected Transform _target = default;
        ///THESE STATS MAY WANT TO BE READ OFF A SCRIPTABLE OBJ FOR A TYPE OF ENEMY
        [SerializeField] protected float _detectionRange = default;
        [SerializeField] protected float _atkRange = default;
        [SerializeField] protected ContactFilter2D _detectionInfo = default;
        protected ActorMovement _movement;

        protected AIState _currentState;
        protected AIState _idleState;
        protected AIState _moveState;
        protected AIState _deathState;
        protected AIState _attackState;

        [SerializeField] string _debuggCurrState;

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
                _debuggCurrState = _currentState.ToString();
            }
            //Debug.Log($"Curr statename={_currentState.ToString()}");
        }
        /*************************************************************************************************************/
        public float DetectionRange => _detectionRange;
        public float AttackRange => _atkRange;
        public ContactFilter2D DetectionInfo => _detectionInfo;
        public Vector3 FacingDir => _movement.isFacingRight() ? transform.right : -transform.right;
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
        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }

        public void SetMovement(float dir)
        {
            _movement.SetInput(dir);
        }
        /*************************************************************************************************************/

        protected abstract AIState GetAIState(AIState.eAIStates nextState);

        private void OnDisable()
        {
            if (LevelLoader.Instance)
                LevelLoader.Instance.OnSceneIsLoading -= OnSceneLoad;
        }

        private void OnSceneLoad(bool cond)
        {
            sceneIsLoading = cond;
        }
    }
}