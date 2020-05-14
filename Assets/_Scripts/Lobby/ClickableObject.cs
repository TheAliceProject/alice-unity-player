using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public GameObject character = null;
    public GameObject panel = null;
    private bool clickable = true;
    // Start is called before the first frame update
    void Start()
    {
        if(panel != null)
            panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnCharacter()
    {
        if(clickable)
            panel.SetActive(!panel.activeSelf);
    }


    public void ClickOnPanel()
    {
        if (clickable)
            panel.SetActive(false);
    }


    public void SetClickable()
    {
        if (!clickable)
            clickable = true;
    }

    public void SetNotClickable()
    {
        if (clickable)
            clickable = false;
    }
}
