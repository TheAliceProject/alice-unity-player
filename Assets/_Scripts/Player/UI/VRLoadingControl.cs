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

    public IEnumerator FadeLoader(bool toOn)
    {
        boxMat = boxRenderer.material;
        if(toOn){
            boxMat.color = Color.clear;
            text.color = Color.clear;
            boxRenderer.gameObject.SetActive(true);
            yield return Routine.Combine(boxMat.ColorTo(loadingColor, 0.25f, ColorUpdate.FullColor),
                                        text.ColorTo(Color.white, 0.25f, ColorUpdate.FullColor));

        }
        else{
            yield return Routine.Combine(boxMat.ColorTo(Color.clear, 0.25f, ColorUpdate.FullColor),
                                        text.ColorTo(Color.clear, 0.25f, ColorUpdate.FullColor));
            boxRenderer.gameObject.SetActive(false);
        }
    }
}
