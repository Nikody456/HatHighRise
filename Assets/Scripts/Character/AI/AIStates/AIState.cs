using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public abstract class AIState
    {

        public enum eAIStates { IDLE, MOVE, ATTACK, JUMP, DEATH}

        public abstract void OnEnable(Transform target);

        public abstract void OnDisable(Transform target);

        public abstract void Execute(Transform target);

        public abstract bool CanExit(eAIStates nextState);
    }
}