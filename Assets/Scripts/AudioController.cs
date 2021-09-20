using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CargoHell.Audio
{
    public class AudioController : Singleton<AudioController>
    {

        [field: SerializeField]
        public List<AudioClip> levelClips { get; private set; }

        [field: SerializeField]
        public List<LevelMusic> levelMusic { get; private set; }

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

        [field: SerializeField]
        public AudioClip scoreCountAudio { get; private set; }
        [field: SerializeField]
        public AudioClip shieldReflectSound { get; private set; }
        [field: SerializeField]
        public AudioClip powerupGetAudio { get; private set; }

        private LevelMusic currentLevelMusic;

        private void Awake()
        {
            base.Awake();
        }

        public void Start()
        {
            if (LevelController.instance._level.LevelMusicIndex >= 0)
            {
                currentLevelMusic = levelMusic[LevelController.instance._level.LevelMusicIndex];
                LevelAudioSource.clip = currentLevelMusic.introClip;
                LevelAudioSource.Play();
            }
            else
            {
                LevelAudioSource.clip = levelClips[LevelController.instance._level.LevelClipIndex];
                LevelAudioSource.Play();
                LevelAudioSource.loop = true;
            }
            

        }

        public void Update()
        {
            if (LevelController.instance._level.LevelMusicIndex >= 0 && !LevelAudioSource.isPlaying)
            {
                LevelAudioSource.clip = currentLevelMusic.loopClip;
                LevelAudioSource.loop = true;
                LevelAudioSource.Play();
            }
        }

        public void PlayShieldSound()
        {
            AuxAudioSource.PlayOneShot(shieldReflectSound);
        }

        public void PlayPowerupSound()
        {
            AuxAudioSource.PlayOneShot(powerupGetAudio);
        }

        public void CountScore()
        {
            AuxAudioSource.clip = scoreCountAudio;
            AuxAudioSource.loop = true;
            AuxAudioSource.Play();
        }

        public void StopCountScore()
        {
            AuxAudioSource.Stop();
            AuxAudioSource.loop = false;
        }
    }
}