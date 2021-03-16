using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers

{
    public class TestPixelHack : MonoBehaviour
    {
        public SpriteRenderer _sr;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                PixelDetector.PrintAllPixels(_sr.sprite);
            }
        }
    }
}