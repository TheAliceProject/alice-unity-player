using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public GameObject aliceNamePanel = null;
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

    public void ClickOnRabit()
    {
        aliceNamePanel.SetActive(true);
    }

    public void ClickOnAliceNamePanel()
    {
        gameObject.SetActive(false);
    }

}
