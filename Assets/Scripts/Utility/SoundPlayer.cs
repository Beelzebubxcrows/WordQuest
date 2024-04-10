using System;
using UnityEngine;

namespace Utility
{
    public class SoundPlayer : MonoBehaviour, IDisposable
    {
        [SerializeField] private AudioSource typingAudioSource;
        [SerializeField] private AudioSource ambientAudioSource;
        [SerializeField]private AudioSource matchAudioSource;
        [SerializeField]private AudioSource clickAudioSource;
        [SerializeField]private AudioSource shuffleAudioSource;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void PlayTypingSound()
        {
            if (!typingAudioSource.isPlaying)
            {
                typingAudioSource.Play();
            }
            
        }

        public void PlayAmbientMusic()
        {
           ambientAudioSource.Play();
        }

        public void PlayMatchSound()
        {
            matchAudioSource.Play();
        }

        public void PlayClickSound()
        {
            clickAudioSource.Play();
        }
        
        public void PlayShuffleSound()
        {
            shuffleAudioSource.Play();
        }
        
        public void Dispose()
        {
            
        }

        
    }
}