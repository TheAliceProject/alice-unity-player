using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public GameObject aliceNamePanel = null;
    public GameObject randyPanel = null;
    // Start is called before the first frame update
    void Start()
    {
        if(aliceNamePanel != null)
            aliceNamePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnAlice()
    {
        randyPanel.SetActive(false);
        aliceNamePanel.SetActive(!aliceNamePanel.activeSelf);
    }

    public void ClickOnRandy()
    {
        aliceNamePanel.SetActive(false);
        randyPanel.SetActive(!randyPanel.activeSelf);
    }

    public void ClickOnPanel()
    {
        randyPanel.SetActive(false);
        aliceNamePanel.SetActive(false);
    }

}
