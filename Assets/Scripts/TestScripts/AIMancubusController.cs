using System.Collections.Generic;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.TestScripts
{
    public class AIMancubusController : AIControllerBase
    {

        private GameObject _mapManager;

        


public override void Start()
        {
            _mapManager = GameObject.Find(typeof(MapManager).Name);
            base.Start();
            
        }

        public override void Update()
        {
            //#if DEBUG
                DebugDrawPath(GetAIControllerPathList(), GetComponent<TeamBase>().MyTeam);
           // #endif
            base.Update();
        }

        public override List<Vector3> GetPathList()
        {
            return _mapManager.GetComponent<MapManager>().GetCheckpointListPath(GetComponent<TeamBase>().MyTeam,this.transform.position);
        }

    }
}
