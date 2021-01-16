using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PostProcessController : MonoBehaviour
{
    // Start is called before the first frame update

    public Volume volume;
    public ParticleSystem particleSystem;
    public AudioSource audioSource;

    private UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
    private float chromaticIntensity;

    private UnityEngine.Rendering.Universal.Bloom bloom;
    private float bloomIntensity;

    private float[] multiChannelSamples;
    private int sampleIndex;


    void Start()
    {
        
        volume.profile.TryGet(out chromatic);
        chromaticIntensity = chromatic.intensity.value;
        volume.profile.TryGet(out bloom);
        bloomIntensity = bloom.intensity.value;


        multiChannelSamples = new float[1024];

    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {
            var vel = particleSystem.velocityOverLifetime;
            vel.enabled = true;
        }
        if(sampleIndex > 1024)
        {
            sampleIndex = 0;
        }

        audioSource.GetSpectrumData(multiChannelSamples, 1, FFTWindow.Blackman);

        float mean = 0f;
        for (int i = 0; i < multiChannelSamples.Length; i++)
        {
            mean += multiChannelSamples[i];
        }

        mean /= 1024;

        chromatic.intensity.value = chromaticIntensity + mean * 200;
        chromatic.active = true;

        bloom.intensity.value = bloomIntensity + mean * 200;
        bloom.active = true;


        sampleIndex++;
    }
}
