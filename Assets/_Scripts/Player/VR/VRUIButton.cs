using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUIButton : MonoBehaviour
{
    public UserInputVR.VRButtonType buttonType;
    public UserInputVR userInput;
    public string normalCharacter;
    public string shiftCharacter;

    private bool shifted = false;
    private Button button;
    private Text buttonText;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<Text>();
        if(button == null || buttonText == null)
            Debug.LogError("Could not find keyboard button component: " + this.gameObject.name);
        button.onClick.AddListener(() =>
        {
            switch(buttonType)
            {
                case UserInputVR.VRButtonType.Normal:
                    userInput.AddToInputField(shifted ? shiftCharacter : normalCharacter);
                    break;
                case UserInputVR.VRButtonType.Caps:
                    userInput.SetShifted(!shifted, !shifted);
                    break;
                case UserInputVR.VRButtonType.Backspace:
                    userInput.Backspace();
                    break;
                case UserInputVR.VRButtonType.Shift:
                    userInput.SetShifted(true, false);
                    break;
                case UserInputVR.VRButtonType.Space: 
                    userInput.AddToInputField(" ");
                    break;
            }
        });
        SetShifted(false);
    }

    public void SetShifted(bool on)
    {
        shifted = on;
        buttonText.text = shifted ? shiftCharacter : normalCharacter;
    }
}
