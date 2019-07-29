using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInput : MonoBehaviour
{
    public TextMeshProUGUI label;
    public InputField inputField;
    public Button doneButton;

    public void SetLabel(string inputLabel)
    {
        label.SetText(inputLabel);
    }
    public string GetUserInput()
    {

    }
}
