using Assets.Scripts.BaseClasses;
using Assets.Scripts.Behaviours;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SpawnerManager : MonoBehaviour, ISpawnerManager
    {
        [SerializeField]
        [Range(0, 1, order = 1)]
        public int MyTeam;

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
                SpawnableGhostGameObjectSelected.GetComponent<MeshRenderer>().material =
                    SpawnableGameObjectCatalogA[SpawnableSelected].GetComponent<SpawnableBehaviourBase>()
                        .CanSpawnMaterial;
            }
            else
            {
                SpawnableGhostGameObjectSelected.GetComponent<MeshRenderer>().material =
                   SpawnableGameObjectCatalogA[SpawnableSelected].GetComponent<SpawnableBehaviourBase>()
                       .CantSpawnMaterial;
            }
        }

        public void ChangeSpawnableSelected()
        {
            Destroy(SpawnableGhostGameObjectSelected);
            SpawnableSelected++;
        }

        public void Start()
        {
            _mapManager = GameObject.Find(typeof(MapManager).Name);
            InvokeRepeating("CheckGhostModel",0.0f,0.1f);
        }

        public void Update()
        {
            ////Esto debe estar en el Core...
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = GetClickPosition();
                clickPosition.y = SpawnableGameObjectCatalogA[SpawnableSelected].transform.localScale.y/2;
                if (_mapManager.GetComponent<MapManager>().CanSpawn(MyTeam, clickPosition))
                {
                    if (MyTeam == Global.TeamId.A) Instantiate(SpawnableGameObjectCatalogA[SpawnableSelected], clickPosition, SpawnableGameObjectCatalogA[SpawnableSelected].transform.rotation);
                    else Instantiate(SpawnableGameObjectCatalogB[SpawnableSelected], clickPosition, SpawnableGameObjectCatalogB[SpawnableSelected].transform.rotation);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) ChangeSpawnableSelected();
        }

    }
}
