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

    private const float onYPosition = 30f;
    private const float offYPosition = 100f;

    private const float onYPositionVR = 250f;
    private const float offYPositionVR = -100f;

    private const float moveTime = 0.25f;
    private bool controlsActive = false;
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(!VRControl.IsLoadedInVR())
        {
            GameObject sceneGraphTest = GameObject.Find("SceneGraph");
            if (sceneGraphTest == null)
                return;
            m_routine.Replace(this, CheckHoveringOverUI());
        }
    }
    
    IEnumerator CheckHoveringOverUI()
    {
        yield return anchor.AnchorPosTo(GetOnPosition(), moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
        while(EventSystem.current.IsPointerOverGameObject())
        {
            yield return null;
        }
        yield return anchor.AnchorPosTo(GetOffPosition(), moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
    }

    public void ForceSlide(bool on)
    {
        controlsActive = on;
        m_routine.Replace(this, anchor.AnchorPosTo(on ? GetOffPosition() : GetOnPosition(), moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut));
    }

    public void ToggleSlide()
    {
        ForceSlide(!controlsActive);
    }

    private float GetOnPosition(){
        return VRControl.IsLoadedInVR() ? onYPositionVR : onYPosition;
    }

    private float GetOffPosition(){
        return VRControl.IsLoadedInVR() ? offYPositionVR : offYPosition;
    }
}
