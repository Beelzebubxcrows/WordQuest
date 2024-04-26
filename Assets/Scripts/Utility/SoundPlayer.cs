using System;
using Persistence.PersistenceManager;
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
            var playerPersistenceManager = InstanceManager.GetInstanceAsSingle<PlayerPersistenceManager>();
            if (!playerPersistenceManager.GetEffectsState()) {
                return;
            }
            
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
            Debug.Log("Play ambient music");
            var playerPersistenceManager = InstanceManager.GetInstanceAsSingle<PlayerPersistenceManager>();
            if (!playerPersistenceManager.GetMusicState()) {
                return;
            }

            Debug.Log("Played ambient music");
            musicAudioSource.clip = ambientAudioClip;
            musicAudioSource.Play();
        }


        #endregion
        
        
        public void HandleStateChanged()
        {
            var playerPersistenceManager = InstanceManager.GetInstanceAsSingle<PlayerPersistenceManager>();
            var musicState = playerPersistenceManager.GetMusicState();
            
            if (!musicState) {
                musicAudioSource.Stop();
            } else if (!musicAudioSource.isPlaying) {
                PlayAmbientMusic();
            }

        }
        
        
        public void Dispose()
        {
            
        }
        
    }
}