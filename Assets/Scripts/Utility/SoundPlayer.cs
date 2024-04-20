using System;
using UnityEngine;

namespace Utility
{
    public class SoundPlayer : MonoBehaviour, IDisposable
    {
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource gameEffectsAudioSource;


        [SerializeField] private AudioClip failAudioClip;
        [SerializeField] private AudioClip typingAudioClip;
        [SerializeField] private AudioClip ambientAudioClip;
        [SerializeField]private AudioClip matchAudioClip;
        [SerializeField]private AudioClip clickAudioClip;
        [SerializeField]private AudioClip shuffleAudioClip;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        #region GAME EFFECTS

        public void PlayTypingSound()
        {
            if (!musicAudioSource.isPlaying)
            {
                PlayGameEffectsAudioSource(typingAudioClip);
            }
        }

      
        public void PlayMatchSound()
        {
            PlayGameEffectsAudioSource(matchAudioClip);
        }

        public void PlayClickSound()
        {
            PlayGameEffectsAudioSource(clickAudioClip);
        }
        
        public void PlayShuffleSound()
        {
            PlayGameEffectsAudioSource(shuffleAudioClip);
        }

        private void PlayGameEffectsAudioSource(AudioClip audioClip)
        {
            gameEffectsAudioSource.clip = audioClip;
            gameEffectsAudioSource.Play();
        }
        
        
        public void PlayFailSound()
        {
            PlayGameEffectsAudioSource(failAudioClip);
        }

        #endregion

        #region AMBIENT MUSIC

        public void PlayAmbientMusic()
        {
            musicAudioSource.clip = ambientAudioClip;
            musicAudioSource.Play();
        }


        #endregion
        
        
        public void Dispose()
        {
            
        }

        
    }
}