using System.Collections;
using UnityEngine;
using BeauRoutine;

public class VRLoadingControl : MonoBehaviour
{
    public MeshRenderer boxRenderer;
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
            boxRenderer.gameObject.SetActive(true);
            yield return boxMat.ColorTo(loadingColor, 0.25f, ColorUpdate.FullColor);
        }
        else
        {
            yield return boxMat.ColorTo(Color.clear, 0.25f, ColorUpdate.FullColor);
            boxRenderer.gameObject.SetActive(false);
        }
    }
}
