using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class ResourceContainer : MonoBehaviour
    {

        public int Capacity;
        private int _currentResources;
        public int CurrentResources {
            get { return _currentResources; }
            set { _currentResources = _currentResources > Capacity ? Capacity : value; }
        }

        public bool ReachedCapacity()
        {
            return _currentResources >= Capacity;
        }

        public bool CanAfford(int cost)
        {
            return cost <= _currentResources;
        }
    }
}