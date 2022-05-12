using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BlowObjProperty
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public float scaleFactor;
}

public class BlowObj : MonoBehaviour
{
    [SerializeField] private float blowTime = 1.5f;
    [SerializeField] private float speedAdjust = 0.9f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private bool isCountDown = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public bool shouldBlow;
    private Rigidbody rigidbody;

    private void Start()
    {
        shouldBlow = false;
        var o = gameObject;
        originalPosition = o.transform.position;
        originalRotation = o.transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
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
                rigidbody.isKinematic = false;
                isCountDown = false;
            }
        }
    }

    public void StartCountDown()
    {
        isCountDown = true;
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}