using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public abstract class AIState
    {
        protected AIInput _ai;
        public enum eAIStates { IDLE, MOVE, ATTACK, JUMP, DEATH }

        public virtual void OnEnable(Transform target)
        {

        }

        public virtual void OnDisable(Transform target)
        {

        }
        public virtual bool CanExit(eAIStates nextState)
        {
            return true;
        }

        public abstract void Execute(Transform target);



        protected virtual float PickADirection(Vector3 pos)
        {
            ///MOVE TOWARDS
            bool dir = (_ai.transform.position.x > pos.x);

            return dir ? -1 : 1;

        }

        protected virtual void SwitchFacingDir()
        {
            //Debug.Log($"<color=red> SWITCHFACING DIR</color>");
            if (_ai.FacingDir == Vector3.right)
            {
                _ai.SetMovement(-1);
            }
            else
            {
                _ai.SetMovement(1);
            }
        }

        protected bool TargetIsAboveMeVertically(Transform target)
        {
            return (target.transform.position.y - _ai.transform.position.y > 1);
        }


        protected bool MultipleGuardsOnTopOfMe()
        {
            int maxGuardsNearby = 1;
            float sphereRadius = 0.125f;  ///Tiny radius
            return SphereCastAtLocation(_ai.transform.position, sphereRadius) > maxGuardsNearby;
        }

        protected int SphereCastAtLocation(Vector3 location, float radius)
        {

            var hits = Physics2D.CircleCastAll(location, radius, Vector2.zero, 0, _ai._aiLayerMask);
            int count = 0;
            foreach (var item in hits)
            {
                ///Will hit self
                //Debug.Log($"hit : {item.rigidbody.gameObject.name}");
                var objHit = item.collider.gameObject;
                ++count;

            }
            return count;
        }
    }
}