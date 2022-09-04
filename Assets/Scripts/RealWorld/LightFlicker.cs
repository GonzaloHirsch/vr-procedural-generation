using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private AudioSource audioSource;
    private Light[] lights;
    private float[] sample;
    public int sampleLength = 1024;
    private float value;
    private float loudness;
    public Vector2 intensityLimits = new Vector2(0f, 10f);
    public Vector2 loudnessLimits = new Vector2(0f, 0.1f);

    void Awake()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.lights = this.GetComponentsInChildren<Light>();
        this.sample = new float[this.sampleLength];
    }

    void Update()
    {
        this.loudness = this.GetLoudness();
        if (this.loudness > 0)
        {
            foreach (Light l in this.lights)
            {
                l.intensity = ((this.intensityLimits.y - this.intensityLimits.x) * Mathf.InverseLerp(this.loudnessLimits.x, this.loudnessLimits.y, this.loudness)) + this.intensityLimits.x;
            }
        }
    }

    float GetLoudness()
    {
        audioSource.clip.GetData(sample, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
        value = 0f;
        foreach (var sample in sample)
        {
            value += Mathf.Abs(sample);
        }
        return value /= this.sampleLength; //clipLoudness is what you are looking for
    }
}
