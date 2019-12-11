using System;
using UnityEngine;
public class LogoFitter : MonoBehaviour
{
    public RectTransform aliceLogo;
    public RectTransform menuPanel;
    
    private Vector2 fittedValue = new Vector2();
    void Update() {
        if (Math.Abs(fittedValue.x - menuPanel.offsetMin.x) < 1.0)
            return;
        fittedValue.x = menuPanel.offsetMin.x;
        fittedValue.y = aliceLogo.offsetMax.y;
        aliceLogo.offsetMax = fittedValue;
    }
}