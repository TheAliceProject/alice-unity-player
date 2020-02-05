using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BeauRoutine;
using UnityEngine.Events;

public class RectTransformResize : UIBehaviour {
    [System.Serializable]
    public sealed class ResizeEvent : UnityEvent<RectTransform> {} 

    private RectTransform m_RectTransform;
    public RectTransform rectTransform { get { return m_RectTransform; } }

    public RectTransform outlineResize;
    public float resizeAddedHeight;

    protected override void Awake() {
        base.Awake();
        m_RectTransform = (RectTransform)transform;
    }


    protected override void OnRectTransformDimensionsChange() {
        base.OnRectTransformDimensionsChange();

        Vector2 newHeight = outlineResize.anchoredPosition;
        Vector2 newSize = outlineResize.sizeDelta;
        newSize.y = m_RectTransform.sizeDelta.y + resizeAddedHeight;
        newSize.x = m_RectTransform.sizeDelta.x + resizeAddedHeight;
        newHeight.y = m_RectTransform.anchoredPosition.y + resizeAddedHeight / 2f;
        outlineResize.sizeDelta = newSize;
        outlineResize.anchoredPosition = newHeight;

    }
}