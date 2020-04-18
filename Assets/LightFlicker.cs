using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private float flickerMinTime = 0.02f;
    [SerializeField]
    private float flickerMaxTime = 0.1f;
    [SerializeField]
    private float minIntensity = 2.8f;
    [SerializeField]
    private float maxIntensity = 3.1f;

    private float newIntensity;
    private float lastIntensity;
    private float flickerTimer = 0;
    private float maxFlickerTime;

    private Light targetLight;

    void Start()
    {
        targetLight = GetComponent<Light>();
    }

    void Update()
    {
        flickerTimer -= Time.deltaTime;

        if(flickerTimer <= 0)
        {
            lastIntensity = targetLight.intensity;
            newIntensity = Random.Range(minIntensity, maxIntensity);
            flickerTimer = Random.Range(flickerMinTime, flickerMaxTime);
            maxFlickerTime = flickerTimer;
        }

        targetLight.intensity = Mathf.Lerp(lastIntensity, newIntensity, 1f - flickerTimer / maxFlickerTime);
    }
}
