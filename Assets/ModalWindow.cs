using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindow : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    public Button okButton;
    
    // Start is called before the first frame update
    void Start()
    {
        okButton.onClick.AddListener(() => { Destroy(this.gameObject); });
    }

    public void SetData(string header, string body)
    {
        headerText.SetText(header);
        bodyText.SetText(body);
    }
}
