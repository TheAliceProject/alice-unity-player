using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingVFX : MonoBehaviour
{
    [SerializeField] private GameObject[] surroundingObjs;
    [SerializeField] private Transform tornadoCenter;
    [SerializeField] private float pullForce;
    [SerializeField] private float endPullForce;
    [SerializeField] private float pullForceDecreaseAmount = 0.1f;
    [SerializeField] private float torqueForce = 1.0f;
    [SerializeField] private float refreshRate;
    [SerializeField] private float maxVelocity;

    private float fixedDeltaTime;
    private Vector3 forwardDir;
    public bool isStartDecreaseForce = false;

    private void Update()
    {
        if (isStartDecreaseForce)
        {
            StartCoroutine(StartDecreaseForce());
            isStartDecreaseForce = false;
        }
    }

    IEnumerator StartDecreaseForce()
    {
        while (pullForce >= endPullForce)
        {
            pullForce -= pullForceDecreaseAmount;
            yield return new WaitForSeconds(pullForceDecreaseAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlowObj") && other.gameObject.GetComponent<BlowObj>().shouldBlow)
        {
            Debug.Log("trigger enter");
            StartCoroutine(StartBlowObj(other, true));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlowObj") && other.gameObject.GetComponent<BlowObj>().shouldBlow)
        {
            Debug.Log("trigger stay");
            StartCoroutine(StartBlowObj(other, true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlowObj"))
        {
            StartCoroutine(StartBlowObj(other, false));
        }
    }

    IEnumerator StartBlowObj(Collider other, bool shouldPull)
    {
        if (shouldPull)
        {
            var adjustCenter = new Vector3(0, 0f, 0);

            if (other.CompareTag("BlowObj"))
            {
                forwardDir = tornadoCenter.position + adjustCenter - other.transform.position;
            }

            if (other.CompareTag("BlowObj"))
            {
                other.GetComponent<Rigidbody>().AddForce(forwardDir.normalized * pullForce * Time.deltaTime,
                    ForceMode.Acceleration);
                int randonRot = Random.Range(0, 2);
                switch (randonRot)
                {
                    case 0:
                        other.GetComponent<Rigidbody>().AddTorque(transform.up * torqueForce * Time.deltaTime);
                        break;
                    case 1:
                        other.GetComponent<Rigidbody>().AddTorque(transform.forward * torqueForce * Time.deltaTime);
                        break;
                    case 2:
                        other.GetComponent<Rigidbody>().AddTorque(transform.right * torqueForce * Time.deltaTime);
                        break;
                    default:
                        break;
                }
                
            }

            if (other.GetComponent<Rigidbody>().velocity.sqrMagnitude > maxVelocity)
            {
                other.GetComponent<Rigidbody>().velocity =
                    other.GetComponent<Rigidbody>().velocity.normalized * maxVelocity;
            }

            yield return refreshRate;
            StartCoroutine(StartBlowObj(other, shouldPull));
        }
    }
}