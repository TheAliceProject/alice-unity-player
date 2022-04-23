using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowObj : MonoBehaviour
{
    [SerializeField] private float blowTime = 3.0f;
    [SerializeField] private float speedAdjust = 0.9f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private bool isCountDown = false;

    public bool shouldBlow;

    private void Start()
    {
        shouldBlow = false;
    }

    private void Update()
    {
        if (isCountDown)
        {
            if (timer <= blowTime)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * speedAdjust, Space.World);
            }
            else
            {
                shouldBlow = true;
                isCountDown = false;
            }
        }
    }

    public void StartCountDown()
    {
        isCountDown = true;
    }
}