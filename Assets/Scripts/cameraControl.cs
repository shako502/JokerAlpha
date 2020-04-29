using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    private Transform transformShake;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.05f;
    private float dampingSpeed = 2.5f;
    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        if (transformShake == null)
        {
            transformShake = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = transformShake.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transformShake.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transformShake.localPosition = initialPosition;
        }
    }

    public void triggerShake()
    {
        shakeDuration = 0.5f;
    }
}
