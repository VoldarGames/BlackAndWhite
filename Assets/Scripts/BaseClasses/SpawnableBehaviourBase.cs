using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.BaseClasses
{
    public class SpawnableBehaviourBase : MonoBehaviour, ISpawnableBehaviour
    {
        [SerializeField]
        private GameObject _ghostModel;
        
        public GameObject GhostModel
        {
            get { return _ghostModel; }
            set { _ghostModel = value; }
        }

        [SerializeField]
        private Material _canSpawnMaterial;

        public Material CanSpawnMaterial
        {
            get { return _canSpawnMaterial; }
            set { _canSpawnMaterial = value; }
        }

        [SerializeField]
        private Material _cantSpawnMaterial;

        public Material CantSpawnMaterial {

            get { return _cantSpawnMaterial;}
            set { _cantSpawnMaterial = value; }
        }

    }
}
