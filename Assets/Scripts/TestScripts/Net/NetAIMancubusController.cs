using System.Collections.Generic;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.BaseClasses.Net;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.TestScripts.Net
{
    public class NetAIMancubusController : NetAIControllerBase
    {

        private GameObject _mapManager;

        


public override void Start()
        {
            _mapManager = GameObject.Find(typeof(MapManager).Name);
            base.Start();
            
        }

        public override void Update()
        {
            #if DEBUG
            var vector3List = GetAIControllerPathList();
            List<Vector3> result = new List<Vector3>();
            foreach (var vector3 in vector3List)
            {
                result.Add(vector3);
            }
           
            DebugDrawPath(result, GetComponent<TeamBase>().MyTeam);
            #endif
            base.Update();
        }

        public override List<Vector3> GetPathList()
        {
            
            return _mapManager.GetComponent<MapManager>().GetCheckpointListPath(GetComponent<TeamBase>().MyTeam,this.transform.position);
           
        }

    }
}
