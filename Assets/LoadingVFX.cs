using System;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine.Internal;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingVFX : MonoBehaviour
{
    [SerializeField] private BlowObj[] surroundingObjs;
    [SerializeField] private Transform tornadoCenter;
    [SerializeField] private float pullForce;
    [SerializeField] private float endPullForce;
    [SerializeField] private float pullForceDecreaseAmount = 0.1f;
    [SerializeField] private float pullForceDecreaseSmoothness = 0.1f;
    [SerializeField] private float torqueForce = 1.0f;
    [SerializeField] private float refreshRate;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float startDelay = 3.0f;
    [SerializeField] private ParticleSystem[] loadingVFX;
    [SerializeField] private GameObject blocker;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private Material blockerMaterial;
    [SerializeField] private Transform blockerStartPoint;
    [SerializeField] private Transform blockerEndPoint;

    [SerializeField] private float thicknessSmoothness = 0.01f;
    [SerializeField] private float movingSmoothness = 0.01f;
    [SerializeField] private float thicknessDuration = 3.0f;
    [SerializeField] private float movingDuration = 3.0f;

    private float fixedDeltaTime;
    private Vector3 forwardDir;
    public bool isStartDecreaseForce = false;
    private float timer = 0f;
    private bool hasPlayed = false;

    private void Start()
    {
        surroundingObjs = FindObjectsOfType<BlowObj>();
        blocker.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (timer < startDelay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (!hasPlayed)
            {
                foreach (ParticleSystem system in loadingVFX)
                {
                    system.Play();
                }

                hasPlayed = true;
                blocker.GetComponent<MeshRenderer>().enabled = true;
                StartCoroutine(StartIncreaseBlockerThickness());
                StartCoroutine(StartMovingBlocker());
            }

            foreach (BlowObj obj in surroundingObjs)
            {
                obj.StartCountDown();
            }

            blocker.transform.RotateAround(blocker.transform.position, Vector3.up, rotateSpeed);
            isStartDecreaseForce = true;
        }

        if (isStartDecreaseForce)
        {
            StartCoroutine(StartDecreaseForce());
            isStartDecreaseForce = false;
        }
    }

    IEnumerator StartIncreaseBlockerThickness()
    {
        float timer = 0;
        Color blockerMaterialColor = blockerMaterial.color;
        while (timer <= thicknessDuration)
        {
            blockerMaterial.color =
                Color.Lerp(new Color(0, 0, 0, 0),
                    blockerMaterialColor, timer);
            timer += thicknessSmoothness;
            yield return new WaitForSeconds(thicknessSmoothness);
        }
    }

    IEnumerator StartMovingBlocker()
    {
        float timer = 0;
        while (timer <= movingDuration)
        {
            blocker.transform.position = Vector3.Lerp(blockerStartPoint.position, blockerEndPoint.position,
                timer / movingDuration);
            timer += movingSmoothness;
            yield return new WaitForSeconds(movingSmoothness);
        }
    }

    IEnumerator StartDecreaseForce()
    {
        while (pullForce >= endPullForce)
        {
            pullForce -= pullForceDecreaseAmount;
            yield return new WaitForSeconds(pullForceDecreaseSmoothness);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlowObj") && other.gameObject.GetComponent<BlowObj>().shouldBlow)
        {
            StartCoroutine(StartBlowObj(other, true));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlowObj") && other.gameObject.GetComponent<BlowObj>().shouldBlow)
        {
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