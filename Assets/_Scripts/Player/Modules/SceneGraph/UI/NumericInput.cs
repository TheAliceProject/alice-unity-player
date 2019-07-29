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

        for(int i = 0; i < numericButtons.Length; i++)
        {
            numericButtons[i].onClick.AddListener(()=>{
                ClickedNumberButton(i);
            });
        }
        decimalButton.onClick.AddListener(()=>{
            if(!inputField.text.Contains("."))
            {
                string inputText = inputField.text;
                inputField.text = inputText + ".";
            }
        });
        plusMinusButton.onClick.AddListener(()=>{
            string inputText = inputField.text;
            if(inputText[0] == '-'){
                inputField.text = inputText.Substring(1);
            }
            else{
                inputField.text = "-" + inputText;
            }
        });
        backspaceButton.onClick.AddListener(()=>{
            if(inputField.text.Length > 0)
            {
                string inputText = inputField.text;
                inputField.text = inputText.Substring(0, inputText.Length-1);
            }
        });
    }

    public void Spawn(string label, AsyncReturn<double> returnDouble)
    {
        SetLabel(label);
        m_Routine.Replace(this, WaitForUserToPopulate(returnDouble));
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
    }

    private IEnumerator WaitForUserToPopulate(AsyncReturn<double> returnDouble)
    {
        while(!userInputDone){
            yield return null;
        }
        returnDouble.Return(Convert.ToDouble(GetUserInput()));
        Destroy(this.gameObject);
    }
}
