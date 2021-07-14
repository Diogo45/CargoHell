using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : Singleton<AudioController>
{

    [field: SerializeField]
    public List<AudioClip> levelClips { get; private set; }

    [field: SerializeField]
    public AudioMixerGroup MasterMixer { get; private set; }
    [field: SerializeField]
    public AudioMixerGroup SFXMixer { get; private set; }

    [field: SerializeField]
    public DoubleAudioSource doubleAudio { get; private set; }
    [field: SerializeField]
    public AudioSource LevelAudioSource { get; private set; }
    [field: SerializeField]
    public AudioSource AuxAudioSource { get; private set; }
    [field: SerializeField]
    public AudioClip GameOverAudioClip { get; private set; }


    public AudioClip scoreCountAudio { get; private set; }

    private void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        LevelAudioSource.clip = levelClips[LevelController.instance._level.LevelMusic];
        LevelAudioSource.Play();

    }

    public void PlayGameOverAudioClip()
    {
       doubleAudio.CrossFade(GameOverAudioClip, 0.5f, 0.5f);

    }



}
