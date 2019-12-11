using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using BeauRoutine;

public class BooleanInput : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Button trueButton;
    public Button falseButton;
    public RectTransform rect;

    private Routine m_Routine;
    private BoolState boolState = BoolState.None;
    private enum BoolState
    {
        None,
        True,
        False
    }
    void Awake()
    {
        trueButton.onClick.AddListener(()=>{
            UserClickedButton(true);
        });
        falseButton.onClick.AddListener(()=>{
            UserClickedButton(false);
        });

        if (!VRControl.IsLoadedInVR())
            rect.SetScale(Screen.height / 800f, Axis.XYZ);
    }

    public void Spawn(string label, AsyncReturn<bool> boolReturn)
    {
        SGScene.UIActive = true;
        SetLabel(label);
        boolState = BoolState.None;
        m_Routine.Replace(this, WaitForUserToPopulate(boolReturn));
    }

    public void SetLabel(string inputLabel)
    {
        label.SetText(inputLabel);
    }

    public void UserClickedButton(bool clickedTrue)
    {
        boolState = clickedTrue ? BoolState.True : BoolState.False;
        if (VRControl.IsLoadedInVR())
        {
            VRControl.Rig().EnablePointersForUI(false);
        }
    }

    private IEnumerator WaitForUserToPopulate(AsyncReturn<bool> returnBool)
    {
        while(boolState == BoolState.None){
            yield return null;
        }
        returnBool.Return(boolState == BoolState.True);
        SGScene.UIActive = false;
        if(VRControl.IsLoadedInVR()){
            Destroy(this.transform.parent.gameObject); // Delete the whole canvas in VR space
        }
        else{
            Destroy(this.gameObject);
        }
    }
}
