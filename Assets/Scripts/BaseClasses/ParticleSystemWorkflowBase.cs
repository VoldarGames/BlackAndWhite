using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.BaseClasses
{

    public class ParticleSystemWorkflowBase : MonoBehaviour, IParticleSystemWorkflow
    {
        #region Properties
            [SerializeField]
            private ParticleSystem _birthParticleSystem;
            public ParticleSystem BirthParticleSystem
            {
                get { return _birthParticleSystem; }
                set { _birthParticleSystem = value; }
            }

            [SerializeField]
            private ParticleSystem _aliveParticleSystem;
            public ParticleSystem AliveParticleSystem
            {
                get { return _aliveParticleSystem; }
                set { _aliveParticleSystem = value; }
            }

            [SerializeField]
            private ParticleSystem _deathParticleSystem;
            public ParticleSystem DeathParticleSystem
            {
                get { return _deathParticleSystem; }
                set { _deathParticleSystem = value; }

            }

            [SerializeField]
            private AudioClip _birthAudioClip;
            public AudioClip BirthAudioClip
            {
                get { return _birthAudioClip; }
                set { _birthAudioClip = value; }
            }

            [SerializeField]
            private AudioSource _aliveAudioSource;
            public AudioSource AliveAudioSource
            {
                get { return _aliveAudioSource; }
                set { _aliveAudioSource = value; }
            }

            [SerializeField]
            private AudioClip _deathAudioClip;
            public AudioClip DeathAudioClip
            {
                get { return _deathAudioClip; }
                set { _deathAudioClip = value; }

            }
        #endregion

        void Start()
        {
            #region BirthManagement
                BirthParticleSystemManagement();
                BirthAudioClipManagement();
            #endregion

            #region AliveManagement
                AliveParticleSystemManagement();
                AliveAudioSourceManagement();
            #endregion

        }

        private void BirthParticleSystemManagement()
        {
            if (BirthParticleSystem != null)
            {
                var clone = Instantiate(BirthParticleSystem, transform.position, BirthParticleSystem.transform.rotation) as ParticleSystem;
                clone.Play(false);
                Destroy(clone.gameObject, clone.duration);
            }
        }
        private void BirthAudioClipManagement()
        {
            if (BirthAudioClip != null)
            {
                if (AudioSourceManager.Singleton.CanPlaySound(BirthAudioClip.name))
                {
                    AudioSourceManager.Singleton.AddSound(BirthAudioClip.name);
                    AudioSource.PlayClipAtPoint(BirthAudioClip, transform.position);
                    AudioSourceManager.Singleton.InvokeAction(() =>
                    {
                        AudioSourceManager.Singleton.RemoveSound(BirthAudioClip.name);

                    }, Random.Range(BirthAudioClip.length * Global.SoundTypeMinimumSpacingFactor, BirthAudioClip.length * Global.SoundTypeMaximumSpacingFactor));


                }
            }
        }
        private void AliveParticleSystemManagement()
        {
            if (AliveParticleSystem != null) AliveParticleSystem.Play(withChildren: false);
        }
        private void AliveAudioSourceManagement()
        {
            if (AliveAudioSource != null)
            {
                if (AudioSourceManager.Singleton.CanPlaySound(AliveAudioSource.clip.name))
                {
                    AudioSourceManager.Singleton.AddSound(AliveAudioSource.clip.name);
                    AliveAudioSource.Play();
                }
                
            }
        }
        private void AliveAudioSourceManagementDestroy()
        {
            if (AliveAudioSource != null)
            {
                AudioSourceManager.Singleton.RemoveSound(AliveAudioSource.clip.name);
                AliveAudioSource.Stop();
            }
        }

        private void DeathParticleSystemManagement()
        {
            if (DeathParticleSystem != null)
            {
                var clone = Instantiate(DeathParticleSystem, transform.position, DeathParticleSystem.transform.rotation) as ParticleSystem;
                clone.Play(false);
                Destroy(clone.gameObject, clone.duration);
            }
        }
        private void DeathAudioClipManagement()
        {
            if (DeathAudioClip != null)
            {
                if (AudioSourceManager.Singleton.CanPlaySound(DeathAudioClip.name))
                {
                    AudioSourceManager.Singleton.AddSound(DeathAudioClip.name);
                    AudioSource.PlayClipAtPoint(DeathAudioClip, transform.position);
                    AudioSourceManager.Singleton.InvokeAction(() =>
                    {
                        AudioSourceManager.Singleton.RemoveSound(DeathAudioClip.name);
                    }, Random.Range(DeathAudioClip.length * Global.SoundTypeMinimumSpacingFactor, DeathAudioClip.length * Global.SoundTypeMaximumSpacingFactor));
                }

            }
        }
        void OnDestroy()
        {
            AliveAudioSourceManagementDestroy();
            DeathParticleSystemManagement();
            DeathAudioClipManagement();
        }

        


    }
}
