using Assets.Scripts.BaseClasses;
using Assets.Scripts.Behaviours;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Managers.Net
{
    public class NetSpawnerManager : NetworkBehaviour, ISpawnerManager
    {
        [SerializeField]
        [SyncVar]
        [Range(0, 1, order = 1)]
        public int MyTeam;

        [SerializeField]
        private Material MaterialA;
        [SerializeField]
        private Material MaterialB;

        private GameObject _mapManager;

        [SerializeField]
        private GameObject[] _spawnableGameObjectCatalogA;
        public GameObject[] SpawnableGameObjectCatalogA
        {
            get { return _spawnableGameObjectCatalogA; }
            set { _spawnableGameObjectCatalogA = value; }
        }

        [SerializeField]
        private GameObject[] _spawnableGameObjectCatalogB;
        public GameObject[] SpawnableGameObjectCatalogB
        {
            get { return _spawnableGameObjectCatalogB; }
            set { _spawnableGameObjectCatalogB = value; }
        }

        [SerializeField] private int _spawnableSelected;

        public int SpawnableSelected
        {
            get { return _spawnableSelected; }
            set { _spawnableSelected = value % SpawnableGameObjectCatalogA.Length; }
        }

        public GameObject SpawnableGhostGameObjectSelected { get; set; }

        
        private void SetMaterial(int teamId)
        {
            if (teamId == Global.TeamId.A) GetComponent<MeshRenderer>().material = MaterialA;
            else if (teamId == Global.TeamId.B) GetComponent<MeshRenderer>().material = MaterialB;
        }


        public Vector3 GetClickPosition()
        {

            var playerPlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist;
            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                return ray.GetPoint(hitdist);
            }
            else return new Vector3(-1, -1, -1); //NoPathVector
        }

        public void CheckGhostModel()
        {
            if (!isLocalPlayer)
            {
                CancelInvoke("CheckGhostModel");
                return;
            }
            if (SpawnableGhostGameObjectSelected == null)
            {
                SpawnableGhostGameObjectSelected = SpawnableGameObjectCatalogA[SpawnableSelected].GetComponent<SpawnableBehaviourBase>().GhostModel;
                SpawnableGhostGameObjectSelected = Instantiate(SpawnableGhostGameObjectSelected);
            }
            SpawnableGhostGameObjectSelected.GetComponent<GhostBehaviour>().TargetPosition = GetClickPosition();
            ChangeGhostModelMaterial();
        }

        public void ChangeGhostModelMaterial()
        {
            Vector3 clickPosition = GetClickPosition();
            if (_mapManager.GetComponent<MapManager>().CanSpawn(MyTeam, clickPosition))
            {
                foreach (var meshRenderer in SpawnableGhostGameObjectSelected.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.material = SpawnableGameObjectCatalogA[SpawnableSelected].GetComponent<SpawnableBehaviourBase>().CanSpawnMaterial;
                }
                    
            }
            else
            {
                foreach (var meshRenderer in SpawnableGhostGameObjectSelected.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.material = SpawnableGameObjectCatalogA[SpawnableSelected].GetComponent<SpawnableBehaviourBase>().CantSpawnMaterial;
                }
            }
        }

        public void ChangeSpawnableSelected()
        {
            Destroy(SpawnableGhostGameObjectSelected);
            SpawnableSelected++;
        }

        private void SetLayer()
        {
            if (MyTeam == Global.TeamId.A) gameObject.layer = Global.TeamLayerId.A;
            if (MyTeam == Global.TeamId.B) gameObject.layer = Global.TeamLayerId.B;
        }

        public void Start()
        {
            AudioSourceManager audioSourceManager = new AudioSourceManager();
           
            _mapManager = GameObject.Find(typeof(MapManager).Name);
            
            SetLayer();
            SetMaterial(MyTeam);
            SetPosition();
            SetGameobjectName();

            if (!isLocalPlayer) return;
            InvokeRepeating("CheckGhostModel", 0.0f, 0.1f);
        }

        private void SetGameobjectName()
        {
            if (MyTeam == Global.TeamId.A) name = "CoreA";
            if (MyTeam == Global.TeamId.B) name = "CoreB";
        }

        private void SetPosition()
        {
            if (MyTeam == Global.TeamId.A) transform.position = GameObject.Find("CoreAPosition").transform.position;
            if (MyTeam == Global.TeamId.B) transform.position = GameObject.Find("CoreBPosition").transform.position;
        }

        public void Update()
        {
            if (!isLocalPlayer) return;
            ////Esto debe estar en el Core...
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                Vector3 clickPosition = GetClickPosition();
                clickPosition.y = SpawnableGameObjectCatalogA[SpawnableSelected].transform.localScale.y/2;
                if (_mapManager.GetComponent<MapManager>().CanSpawn(MyTeam, clickPosition))
                {
                    CmdSpawnUnit(MyTeam,SpawnableSelected, clickPosition);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) ChangeSpawnableSelected();
        }

        [Command]
        private void CmdSpawnUnit(int team, int spawnableIndex, Vector3 clickPosition)
        {
            var unit = team == Global.TeamId.A
                ? SpawnableGameObjectCatalogA[spawnableIndex]
                : SpawnableGameObjectCatalogB[spawnableIndex];
            var unitInstance = Instantiate(unit, clickPosition, unit.transform.rotation) as GameObject;
            //NetworkServer.SpawnWithClientAuthority(unitInstance, gameObject);
            NetworkServer.Spawn(unitInstance);
        }
    }
}
