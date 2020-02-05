using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeauRoutine;

public class LoadingControl : MonoBehaviour
{
    public CanvasGroup fader;

    private Routine routine;

    public IEnumerator DisplayLoadingScreen(bool enable)
    {
        yield return (fader.FadeTo(enable ? 1f : 0f, 0.25f));
    }


}
