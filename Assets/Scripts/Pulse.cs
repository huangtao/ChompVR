using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class Pulse : MonoBehaviour {

    [SerializeField]
    private new Light light;

    //[SerializeField]
    //private float invokeRate = 0.5f;

    [SerializeField]
    private float intensityRate = 2000;

    [SerializeField]
    [Range(0,8)]
    private float minIntensity = 1.1f;

    [SerializeField]
    [Range(0, 8)]
    private float maxIntensity = 1.2f;

    void Start() {
        intensityRate = 1 / intensityRate;
        light.intensity = minIntensity;
        //InvokeRepeating("BeginPulse", 0, invokeRate);
    }

    private void BeginPulse() {
        StartCoroutine(Pulsate());
    }

    private IEnumerator Pulsate() {
        yield return StartCoroutine(Brighten());
        yield return StartCoroutine(Dim());
    }

    private IEnumerator Brighten() {
        while(light.intensity < maxIntensity) {
            light.intensity += intensityRate;
            yield return null;
        }
    }

    private IEnumerator Dim() {
        while(light.intensity > minIntensity) {
            light.intensity -= intensityRate;
            yield return null;
        }
    }

}
