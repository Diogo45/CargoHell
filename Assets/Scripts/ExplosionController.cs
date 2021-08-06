using CargoHell.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    public int explosionTime; // set it in inspector
    private AudioSource explosionAudioSource;
    public AudioClip explosionSound;


    private void Start()
    {
        explosionAudioSource = gameObject.AddComponent<AudioSource>();
        explosionAudioSource.outputAudioMixerGroup = AudioController.instance.SFXMixer;
        explosionAudioSource.PlayOneShot(explosionSound);
        Invoke("DestroyMe", explosionTime); // shedules derived call 
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

}
