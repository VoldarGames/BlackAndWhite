using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class FashionBulletBehaviour : ParticleSystemWorkflowBase
    {
        [SerializeField]
        private Material MaterialA;
        [SerializeField]
        private Material MaterialB;
        public void SetMaterial(int teamId)
        {
            if (teamId == Global.TeamId.A) GetComponent<MeshRenderer>().material = MaterialA;
            else if (teamId == Global.TeamId.B) GetComponent<MeshRenderer>().material = MaterialB;
        }
    }
}
    