using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingVFX : MonoBehaviour
{
    [Header("Tornado")] [SerializeField] private Transform tornadoCenter;
    [SerializeField] private ParticleSystem[] windVFX;
    [Header("Forces")] [SerializeField] private float pullForce;
    [SerializeField] private float endPullForce;
    [SerializeField] private float pullForceDecreaseAmount = 0.1f;
    [SerializeField] private float pullForceDecreaseSmoothness = 0.1f;
    [SerializeField] private float torqueForce = 1.0f;
    [SerializeField] private float refreshRate;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float startDelay = 3.0f;
    [Header("Blocker")] [SerializeField] private GameObject blocker;
    [SerializeField] private Material blockerMaterial;
    [SerializeField] private Transform blockerStartPoint;
    [SerializeField] private Transform blockerEndPoint;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private float thicknessDuration = 3.0f;
    [SerializeField] private float movingDuration = 3.0f;
    [Header("Ground")] [SerializeField] private GameObject ground;
    [SerializeField] private Light groundLight;

    public bool startCountdown = false;
    private float timer = 0f;
    public bool isStartDecreaseForce = false;
    private bool hasWindPlayed = false;
    private BlowObj[] surroundingObjs;
    private Vector3 forwardDir;

    private void Start()
    {
        surroundingObjs = FindObjectsOfType<BlowObj>();
        blocker.GetComponent<MeshRenderer>().enabled = false;
        ground.SetActive(false);
        groundLight.intensity = 0;
    }

    private void Update()
    {
        if (!startCountdown) return;
        if (timer < startDelay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (!hasWindPlayed)
            {
                foreach (ParticleSystem system in windVFX)
                {
                    system.Play();
                }

                hasWindPlayed = true;
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
        }
    }

    public void StartCountdown()
    {
        startCountdown = true;
    }

    IEnumerator StartIncreaseBlockerThickness()
    {
        float timer = 0;
        Color blockerMaterialColor = blockerMaterial.color;
        while (timer < thicknessDuration)
        {
            blockerMaterial.color =
                Color.Lerp(new Color(0, 0, 0, 0),
                    blockerMaterialColor, timer / thicknessDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        blockerMaterial.color = blockerMaterialColor;
    }

    IEnumerator StartMovingBlocker()
    {
        float timer = 0;
        while (timer < movingDuration)
        {
            blocker.transform.position = Vector3.Lerp(blockerStartPoint.position, blockerEndPoint.position,
                timer / movingDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        blocker.transform.position = blockerEndPoint.position;
    }

    IEnumerator StartDecreaseForce()
    {
        while (pullForce > endPullForce)
        {
            pullForce -= pullForceDecreaseAmount;
            yield return new WaitForSeconds(pullForceDecreaseSmoothness);
        }

        ground.SetActive(true);
        groundLight.intensity = 3;
        pullForce = endPullForce;
        isStartDecreaseForce = false;
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