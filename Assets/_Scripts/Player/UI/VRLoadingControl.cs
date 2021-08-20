using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BeauRoutine;

public class VRLoadingControl : MonoBehaviour
{
    public MeshRenderer boxRenderer;
    public TextMeshPro text;
    public Color loadingColor;

    private Material boxMat;

    public void FadeLoader(bool toOn)
    {
        if (!gameObject.activeInHierarchy) {
           return;
        }

        StartCoroutine(FadeLoaderRoutine(toOn));
    }

    public IEnumerator FadeLoaderRoutine(bool toOn)
    {
        if (!gameObject.activeInHierarchy) {
            yield break;
        }

        boxMat = boxRenderer.material;
        if(toOn){
            boxMat.color = Color.clear;
            text.color = Color.clear;
            boxRenderer.gameObject.SetActive(true);

            Coroutine boxMatTween = StartCoroutine(boxMat.ColorTo(loadingColor, 0.25f, ColorUpdate.FullColor));
            Coroutine textColorTween = StartCoroutine(text.ColorTo(Color.white, 0.25f, ColorUpdate.FullColor));

            yield return boxMatTween;
            yield return textColorTween;
        }
        else{
            Coroutine boxMatTween = StartCoroutine(boxMat.ColorTo(Color.clear, 0.25f, ColorUpdate.FullColor));
            Coroutine textColorTween = StartCoroutine(text.ColorTo(Color.clear, 0.25f, ColorUpdate.FullColor));
            yield return boxMatTween;
            yield return textColorTween;
            boxRenderer.gameObject.SetActive(false);
        }
    }
}
