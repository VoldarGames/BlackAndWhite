using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BaseClasses.Net
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class NetAIControllerBase : NetworkBehaviour
    {
        
        private NavMeshAgent _agent;

        private List<Vector3> _pathList;

        public NetAIControllerBase()
        {
            _pathList = new List<Vector3>();
        
        }

        

        public List<Vector3> GetAIControllerPathList()
        {
          return _pathList;
        }

        //[SyncVar]
        private Vector3 _destinationPosition;

        public Vector3 DestinationPosition
        {
            get { return _destinationPosition; }
            set
            {
                if (_agent == null) return;
                if (_agent.SetDestination(value)) _destinationPosition = value;
            }
        }


        public virtual void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (!isServer) return;
            _pathList = GetPathList();
            
        }

        
        public virtual List<Vector3> GetPathList()
        {
           return new List<Vector3>();
        }

        public virtual void Update()
        {
            if (!isServer) return;
            GoOverPathList();
        }

        public void GoOverPathList()
        {
            if (_pathList.Count == 0) return;
            if (!_agent.hasPath)
            {
                GoTo(_pathList[0]);
                return;
            }
            if (HasArrived())
            {
                _pathList.RemoveAt(0);
                if(_pathList.Any()) GoTo(_pathList[0]);
            }
        }

        public void GoTo(Vector3 position)
        {
            if(DestinationPosition != position) RpcGoTo(position);
            
        }

        [ClientRpc]
        private void RpcGoTo(Vector3 position)
        {
            DestinationPosition = position;
        }

        public bool HasArrived()
        {
            return (DestinationPosition - transform.position).magnitude < Global.NavMeshAgentDestinationTolerance;
        }

        /// <summary>
        /// Muestra las lineas que forman el path
        /// </summary>
        /// <param name="checkpointsPath">Path a mostrar</param>
        /// <param name="idTeam">Equipo de la entidad</param>
        public void DebugDrawPath(List<Vector3> checkpointsPath, int idTeam)
        {
            if (checkpointsPath.Count <= 1) return;
            var anterior = checkpointsPath.First();
            anterior.y = 1.0f;
            for (int i = 1; i < checkpointsPath.Count; ++i)
            {
                var actual = checkpointsPath[i];
                actual.y = 1.0f;
                if (Global.TeamId.A == idTeam) Debug.DrawLine(anterior, actual, Color.red);
                else if (Global.TeamId.B == idTeam) Debug.DrawLine(anterior, actual, Color.blue);
                anterior = actual;
            }
        }
    }
}
