using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AudioSourceManager : ExtendedMonoBehaviour
    {
        public static AudioSourceManager Singleton;

        private Dictionary<string, int> _currentPlayingSounds;
        private Dictionary<string, bool> _currentPlayingSoundsLock;

        public void Start()
        {
            Singleton = this;
            Singleton._currentPlayingSounds = new Dictionary<string, int>();
            Singleton._currentPlayingSoundsLock = new Dictionary<string, bool>();
        }

        public bool CanPlaySound(string soundName)
        {
            if (!_currentPlayingSounds.ContainsKey(soundName)) return true;

            return !_currentPlayingSoundsLock[soundName] && _currentPlayingSounds[soundName] < Global.MaxNumberOfSoundType;

        }

        public void AddSound(string soundName)
        {
            if (!_currentPlayingSounds.ContainsKey(soundName))
            {
                _currentPlayingSounds.Add(soundName, 1);
                _currentPlayingSoundsLock.Add(soundName, true);
            }
            else
            {
                _currentPlayingSounds[soundName]++;
            }
            _currentPlayingSoundsLock[soundName] = true;
            InvokeAction(() =>
            {
                _currentPlayingSoundsLock[soundName] = false;
            }, Global.CooldownSoundType);
        }

        public void RemoveSound(string soundName)
        {
            if (!_currentPlayingSounds.ContainsKey(soundName)) return;
            _currentPlayingSounds[soundName]--;
            if (_currentPlayingSounds[soundName] == 0)
            {
                _currentPlayingSounds.Remove(soundName);
                _currentPlayingSoundsLock.Remove(soundName);
            }
        }

    }
}