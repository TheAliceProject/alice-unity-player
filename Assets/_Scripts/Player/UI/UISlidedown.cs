using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using BeauRoutine;
using UnityEngine.Events;
using UnityEngine.XR;

public class UISlidedown : MonoBehaviour, IPointerEnterHandler
{
    public RectTransform anchor;

    private Routine m_routine;

    private const float onYPosition = 30f;
    private const float onYPositionVR = -150f;
    private const float offYPosition = 100f;
    private const float offYPositionVR = -750f;
    public bool isVR = false;

    private const float moveTime = 0.25f;
    private bool controlsActive = false;

    void Update() {
        if(isVR && VRControl.IsMenuTriggerDown())
            VRSlide();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(!isVR)
        {
            GameObject sceneGraph = GameObject.Find("SceneGraph");
            if (sceneGraph == null)
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
        m_routine.Replace(this, anchor.AnchorPosTo(on ? GetOnPosition() : GetOffPosition(), moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut));
    }

    public void VRSlide()
    {
        controlsActive = !controlsActive;
        ForceSlide(controlsActive);
        VRControl.Rig().EnablePointersForControl(controlsActive);
    }

    public void ShowBriefly()
    {
        m_routine.Replace(this, ShowBrieflyRoutine());
    }

    private IEnumerator ShowBrieflyRoutine()
    {
        yield return anchor.AnchorPosTo(GetOnPosition(), moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
        controlsActive = true;
        yield return 1.5f;
        yield return anchor.AnchorPosTo(GetOffPosition(), moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
        controlsActive = false;
    }

    private float GetOnPosition(){
        return isVR ? onYPositionVR : onYPosition;
    }
    private float GetOffPosition(){
        return isVR ? offYPositionVR : offYPosition;
    }
}
