using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeauRoutine;

public class ModalWindow : MonoBehaviour
{
    public RectTransform dialog;
    public TextMeshProUGUI bodyText;
    public Button okButton;

    private Routine m_routine;
    // Start is called before the first frame update
    void Start()
    {
        okButton.onClick.AddListener(() => { m_routine.Replace(this, PopRoutine(false)); });
        dialog.SetScale(0f, Axis.XY);
        m_routine.Replace(this, PopRoutine(true));
    }

    public void SetData(string header, string body)
    {
        string modalText = string.Format("<size=100%>{0}\n\n<size=75%>{1}", header, body);
        bodyText.SetText(modalText);
    }

    IEnumerator PopRoutine(bool popIn)
    {
        if(popIn)
        {
            yield return dialog.ScaleTo(1f, 0.4f, Axis.XY).Ease(Curve.CubeInOut);
        }
        else{
            yield return dialog.ScaleTo(0f, 0.4f, Axis.XY).Ease(Curve.CubeInOut);
            Destroy(this.gameObject);
        }
    }
}
