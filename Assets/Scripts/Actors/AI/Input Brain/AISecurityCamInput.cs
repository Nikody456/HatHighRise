using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace AI
{
    public class AISecurityCamInput : AIInput
    {
        [SerializeField] SpriteRenderer _spriteRenderer = default;
        [SerializeField] float _scanFrequency=2.5f;
       
        public bool IsScanning { get; private set; }

        protected override void CreateStates()
        {
            _idleState = new AIISecurityCamIdleState(this, _scanFrequency);
            _moveState = new AIScanState(this);
            _attackState = new AIAttackState(this);
            _currentState = _idleState;

        }

        protected override AIState GetAIState(AIState.eAIStates nextState)
        {
            AIState state = null;
            switch (nextState)
            {
                case AIState.eAIStates.IDLE:
                    {
                        state = _idleState;
                        break;
                    }
                case AIState.eAIStates.MOVE:
                    {
                        state = _moveState;
                        break;
                    }
                case AIState.eAIStates.DEATH:
                    {
                        state = _deathState;
                        break;
                    }
                case AIState.eAIStates.ATTACK:
                    {
                        state = _attackState;
                        break;
                    }
            }
            return state;
        }
        
        public void SetIsScanning(bool cond)
        {
            IsScanning = cond;
        }

        public void ScanDirection()
        {
            Debug.Log(PixelDetector.DetectFirstPixel(_spriteRenderer.sprite, Color.black));
            PixelDetector.PrintAllPixels(_spriteRenderer.sprite);
        }


        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                ScanDirection();

            base.Update();
        }
    }
}
