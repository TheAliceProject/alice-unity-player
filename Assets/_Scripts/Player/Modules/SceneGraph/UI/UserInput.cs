﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using BeauRoutine;

public class UserInput : MonoBehaviour
{
    public TextMeshProUGUI label;
    public InputField inputField;
    public Button doneButton;

    private Routine m_Routine;
    private bool userInputDone = false;

    void Awake()
    {
        doneButton.onClick.AddListener(()=>{
            UserClickedButton();
        });
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            UserClickedButton();
        }
    }

    public void Spawn(string label, AsyncReturn<string> stringReturn)
    {
        SGScene.UIActive = true;
        SetLabel(label);
        userInputDone = false;
        m_Routine.Replace(this, WaitForUserToPopulate(stringReturn));
    }

    public void SetLabel(string inputLabel)
    {
        label.SetText(inputLabel);
    }

    public string GetUserInput()
    {
        return inputField.text;
    }

    public void UserClickedButton()
    {
        userInputDone = true;
        if (VRControl.IsLoadedInVR())
        {
            VRControl.Rig().EnablePointersForUI(false);
        }
    }

    private IEnumerator WaitForUserToPopulate(AsyncReturn<string> returnString)
    {
        while(!userInputDone){
            yield return null;
        }
        returnString.Return(GetUserInput());

        SGScene.UIActive = false;
        if (VRControl.IsLoadedInVR())
        {
            Destroy(this.transform.parent.gameObject); // Delete the whole canvas in VR space
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
