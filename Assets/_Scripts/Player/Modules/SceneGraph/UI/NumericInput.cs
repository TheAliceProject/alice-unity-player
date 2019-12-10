using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using BeauRoutine;

public class NumericInput : MonoBehaviour
{
    public TextMeshProUGUI label;
    public InputField inputField;
    public RectTransform rect;
    public Button okButton;
    public Button[] numericButtons;
    public Button plusMinusButton;
    public Button decimalButton;
    public Button backspaceButton;

    private Routine m_Routine;
    private bool userInputDone = false;

    void Awake()
    {
        okButton.onClick.AddListener(()=>{
            ClickedOkayButton();
        });

        for(int i = 0; i < numericButtons.Length; i++){
            int x = i; // This looks silly but must be here.
            numericButtons[x].onClick.AddListener(()=>{
                ClickedNumberButton(x);
            });
        }
        decimalButton.onClick.AddListener(()=>{
            if(!inputField.text.Contains(".")){
                string inputText = inputField.text;
                inputField.text = inputText + ".";
            }
        });
        plusMinusButton.onClick.AddListener(()=>{
            string inputText = inputField.text;
            if(inputText.Length == 0){
                inputField.text = "-";
            }
            if(inputText.Length > 0 && inputText[0] == '-'){
                inputField.text = inputText.Substring(1);
            }
            else{
                inputField.text = "-" + inputText;
            }
        });
        backspaceButton.onClick.AddListener(()=>{
            if(inputField.text.Length > 0){
                string inputText = inputField.text;
                inputField.text = inputText.Substring(0, inputText.Length-1);
            }
        });

        if(!VRControl.IsLoadedInVR())
            rect.SetScale(Screen.height / 800f, Axis.XYZ);
    }

    public void Spawn(string label, AsyncReturn<double> returnDouble)
    {
        SGScene.UIActive = true;
        SetLabel(label);
        m_Routine.Replace(this, WaitForUserToPopulate(returnDouble));
    }

    public void Spawn(string label, AsyncReturn<int> returnInt)
    {
        SGScene.UIActive = true;
        SetLabel(label);
        m_Routine.Replace(this, WaitForUserToPopulate(returnInt));
    }

    public void SetLabel(string inputLabel)
    {
        label.SetText(inputLabel);
    }

    public void ClickedNumberButton(int theNumber)
    {
        string inputText = inputField.text;
        inputField.text = inputText + theNumber.ToString();
    }

    public string GetUserInput()
    {
        return inputField.text;
    }

    public void ClickedOkayButton()
    {
        userInputDone = true;
        if (VRControl.IsLoadedInVR())
        {
            VRControl.Rig().EnablePointersForUI(false);
        }
    }

    private IEnumerator WaitForUserToPopulate(AsyncReturn<int> returnInt)
    {
        yield return DoneWithValidInput();
        returnInt.Return(Convert.ToInt32(GetUserInput()));
        DestroyMe();
    }

    private IEnumerator WaitForUserToPopulate(AsyncReturn<double> returnDouble)
    {
        yield return DoneWithValidInput();
        returnDouble.Return(Convert.ToDouble(GetUserInput()));
        DestroyMe();
    }

    private IEnumerator DoneWithValidInput()
    {
        bool validInput = false;
        while(!validInput){
            while(!userInputDone){
                yield return null;
            }
            string userInput = GetUserInput();
            if(userInput.Length == 0 || userInput == "-" || userInput == "." || userInput == "-."){
                userInputDone = false;
                continue;
            }
            else{
                validInput = true;
            }
        }
    }  

    private void DestroyMe()
    {
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
