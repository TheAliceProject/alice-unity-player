using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using BeauRoutine;
using UnityEngine.Events;

public class UISlidedown : MonoBehaviour, IPointerEnterHandler
{
    public RectTransform anchor;

    private Routine m_routine;

    private const float downYPosition = 30f;
    private const float upYPosition = 100f;
    private const float moveTime = 0.25f;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GameObject sceneGraphTest = GameObject.Find("SceneGraph");
        if(sceneGraphTest == null)
            return;
        m_routine.Replace(this, CheckHoveringOverUI());
    }
    
    IEnumerator CheckHoveringOverUI()
    {
        yield return anchor.AnchorPosTo(downYPosition, moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
        while(EventSystem.current.IsPointerOverGameObject())
        {
            yield return null;
        }
        yield return anchor.AnchorPosTo(upYPosition, moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
    }

    public void ForceSlide(bool up)
    {
        m_routine.Replace(this, anchor.AnchorPosTo(up ? upYPosition : downYPosition, moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut));
    }
}
