using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface INetBulletBehaviour
    {
        int Damage { get; set; } 
        int MyTeam { get; set; }
        float Speed { get; set; }

        float TimeToLive { get; set; }
        Vector3 Direction { get; set; }

        void SendDamage(GameObject target);
    }
}
    