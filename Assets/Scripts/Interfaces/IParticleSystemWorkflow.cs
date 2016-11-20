using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IParticleSystemWorkflow
    {
        ParticleSystem BirthParticleSystem { get; set; }
        ParticleSystem AliveParticleSystem { get; set; }
        ParticleSystem DeathParticleSystem { get; set; }

        AudioClip BirthAudioClip { get; set; }
        AudioSource AliveAudioSource { get; set; }

        AudioClip DeathAudioClip { get; set; }
    }
}
