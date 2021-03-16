using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] Vector3 fadeOutSize;
    [SerializeField] float fadeDuration;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float timeElapsed = 0;

        while (timeElapsed < fadeDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, fadeOutSize, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = fadeOutSize;
    }

    public IEnumerator FadeIn()
    {
        float timeElapsed = 0;

        while (timeElapsed < fadeDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = Vector3.zero;
    }
}
