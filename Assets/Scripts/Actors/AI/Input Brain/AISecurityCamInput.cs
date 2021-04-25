using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace AI
{
    public class AISecurityCamInput : AIInput
    {
        [SerializeField] SpriteRenderer _spriteRenderer = default;
        [SerializeField] float _scanFrequency = 2.5f;
        [SerializeField] float _scanSpread = 3f;
        [SerializeField] Vector3 _scanPositionOffset = Vector3.zero;
        [SerializeField] SecDoor _securityDoor = default;

        public bool IsScanning { get; private set; }

        protected override void CreateStates()
        {
            _idleState = new AIISecurityCamIdleState(this, _scanFrequency);
            _moveState = new AIScanState(this);
            _attackState = new AISecCamNotifyState(this, _securityDoor.SpawnGuard);
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

        public void RayCastScanDirection()
        {
            var pixelDir = PixelDetector.DetectFirstPixel(_spriteRenderer.sprite, Color.black, false);

            var facingDir = new Vector3(pixelDir.x, pixelDir.y, 0);

            if(RaycastViaDir (facingDir) == 0)
            {
                ///If we miss, Try scanning slighlty to the right / left
                var slightOffset = new Vector3(_scanSpread, 0, 0);
                if (RaycastViaDir(facingDir + slightOffset) ==0)
                    RaycastViaDir(facingDir - slightOffset);
            }

            //PixelDetector.PrintAllPixels(_spriteRenderer.sprite);
        }

        private int RaycastViaDir(Vector3 facingDir)
        {
            var ourPos = transform.position + _scanPositionOffset;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            int numHits = Physics2D.Raycast(ourPos, facingDir, DetectionInfo, results, DetectionRange);
            for (int i = 0; i < numHits; i++)
            {
                RaycastHit2D hit = results[i];
                if (hit.collider != null)
                {
                    //Debug.Log($"Dir={facingDir}, #NumHits={numHits}, Detected Hit: {hit.collider.gameObject.name} !");
                    ///We are not ourself
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        if (hit.collider.gameObject.layer == GameConstants.PLAYER_LAYER)
                        {
                            SetTarget(hit.collider.gameObject.transform);
                            Debug.DrawLine(ourPos, ourPos + (facingDir.normalized * DetectionRange), Color.red, 1);

                        }
                        else ///We hit an obstacle first, our view is blocked
                        {
                            Debug.DrawLine(ourPos, hit.point, Color.yellow, 1);
                            SphereCastAtLocation(hit.point);
                        }
                    }
                }
            }
            if(numHits==0)
            {
                ///Have to normalize facingDir to match Physics2D.RayCast which is normalizing under the hood
                Debug.DrawLine(ourPos, ourPos + (facingDir.normalized * DetectionRange), Color.blue, 1);
            }

            return numHits;
        }


        public int RayCastScanDirectionAI()
        {
            var pixelDir = PixelDetector.DetectFirstPixel(_spriteRenderer.sprite, Color.black, false);


            var ourPos = transform.position + _scanPositionOffset;
            var facingDir = new Vector3(pixelDir.x, pixelDir.y, 0);
            int count = 0;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            //Need to figure out how to scan thicker radius or cone
            //  int numHits = Physics2D.CircleCast(ourPos, scanRadius, facingDir, DetectionInfo, results, DetectionRange);
            int numHits = Physics2D.Raycast(ourPos, facingDir, DetectionInfo, results, DetectionRange);
            for (int i = 0; i < numHits; i++)
            {
                RaycastHit2D hit = results[i];
                if (hit.collider != null)
                {
                    //Debug.Log($"Dir={facingDir}, #NumHits={numHits}, Detected Hit: {hit.collider.gameObject.name} !");
                    ///We are not ourself
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        if (hit.collider.gameObject.layer == GameConstants.PLAYER_LAYER)
                        {
                            SetTarget(hit.collider.gameObject.transform);
                            Debug.DrawLine(ourPos, ourPos + (facingDir * DetectionRange), Color.red, 1);

                        }
                        else ///We hit an obstacle first, our view is blocked
                        {
                            Debug.DrawLine(ourPos, hit.point, Color.yellow, 1);
                           count = SphereCastAtLocation(hit.point, GameConstants.AI_LAYER);
                        }
                    }
                }
            }
            return count;
            //PixelDetector.PrintAllPixels(_spriteRenderer.sprite);
        }

        protected void SphereCastAtLocation(Vector3 hitPoint)
        {
            float _sphereRadius = 2;
            var hits = Physics2D.CircleCastAll(hitPoint, _sphereRadius, Vector2.zero, 0, _detectionInfo.DetectionInfo.layerMask);
            foreach (var item in hits)
            {
                //Debug.Log($"hit : {item.rigidbody.gameObject.name}");
                var objHit = item.collider.gameObject;
                if (objHit.layer == GameConstants.PLAYER_LAYER)
                {
                    SetTarget(objHit.transform);
                    return;
                }
            }
        }

        public int SphereCastAtDoorLocation()
        {
            return SphereCastAtLocation(_securityDoor.transform.position, GameConstants.AI_LAYER);
        }

        protected int SphereCastAtLocation(Vector3 hitPoint, int raycastLayer)
        {
            float _sphereRadius = 2;
            var hits = Physics2D.CircleCastAll(hitPoint, _sphereRadius, Vector2.zero, 0, _detectionInfo.DetectionInfo.layerMask);
            int count = 0;
            foreach (var item in hits)
            {
                //Debug.Log($"hit : {item.rigidbody.gameObject.name}");
                var objHit = item.collider.gameObject;
                if (objHit.layer == raycastLayer)
                {
                    ++count;
                }
            }
            return count;
        }

        protected override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.P))
            {
                _securityDoor.SpawnGuard();
            }
        }

    }
}
