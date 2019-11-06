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

    public float onYPosition = 30f;
    public float offYPosition = 100f;
    public bool isVR = false;

    private const float moveTime = 0.25f;
    private bool controlsActive = false;
    
    void Update()
    {
        if(isVR && Input.GetButtonDown("MenuLeft")){
            VRSlide();
        }
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
        yield return anchor.AnchorPosTo(onYPosition, moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
        while(EventSystem.current.IsPointerOverGameObject())
        {
            yield return null;
        }
        yield return anchor.AnchorPosTo(offYPosition, moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut);
    }

    public void ForceSlide(bool on, bool withVR=false)
    {
        if (withVR){
            VRControl.Rig().EnablePointersForControl(on);
        }

        controlsActive = on;
        m_routine.Replace(this, anchor.AnchorPosTo(on ? onYPosition : offYPosition, moveTime * Time.timeScale, Axis.Y).Ease(Curve.BackOut));
    }

    public void VRSlide()
    {
        controlsActive = !controlsActive;
        ForceSlide(controlsActive, true);
    }
}
