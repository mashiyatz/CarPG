using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPos;
    private float timer;

    private void Awake()
    {
        originalPos = camTransform.localPosition;
    }

    private IEnumerator ShakeCamera()
    {
        timer = shakeDuration;
        while (timer > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            timer -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
        camTransform.localPosition = originalPos;
    }

    public void StartShake()
    {
        StartCoroutine(ShakeCamera());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
