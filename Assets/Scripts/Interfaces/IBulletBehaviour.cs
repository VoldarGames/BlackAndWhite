using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IBulletBehaviour
    {
        int Damage { get; set; } 
        int MyTeam { get; set; }
        float Speed { get; set; }

        float TimeToLive { get; set; }
        Vector3 Direction { get; set; }

        void SendDamage(GameObject target);
    }
}
    