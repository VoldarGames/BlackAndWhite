using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BaseClasses
{
    public class TeamBase : NetworkBehaviour {
        [SerializeField]
        [Range(0, 1, order = 1)]
        [SyncVar]
        public int MyTeam;
    }
}
