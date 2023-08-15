using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    CanvasGroup myCG;
    Coroutine currentlyActiveFade = null;
    private void Awake()
    {
        myCG = GetComponent<CanvasGroup>();

    }
    
    public IEnumerator FadeOut(float time)
    {
        return Fade(1, time);

    }

    private IEnumerator FadeRoutine(float target, float time)
    {
        while (!Mathf.Approximately(myCG.alpha, target)) //alpanot1
        {
            myCG.alpha = Mathf.MoveTowards(myCG.alpha, target, Time.deltaTime / time);
            yield return null;
        }

    }

    public IEnumerator Fade(float target, float time)
    {
        if (currentlyActiveFade != null)
        {
            StopCoroutine(currentlyActiveFade);
        }
        currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
        yield return currentlyActiveFade;
    }

    public IEnumerator FadeIn(float time)
    {
        return Fade(0, time);
    }
}
