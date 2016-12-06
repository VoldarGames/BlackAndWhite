using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class MapManager : MonoBehaviour
    {
        public GameObject DebugCheckpointDot;
        public GameObject CoreA;
        public GameObject CoreB;
        public Transform WalkableZone;
        private int _ncols;
        private int _nrows;
        private Vector3 _startPosition;
        private Vector3 _finalPosition;
        public List<List<Vector3>> MapGrid;
        [SerializeField]
        private bool _debug;

        void Awake()
        {
            InitializeGridPositions();
            InitializeMapGrid();
        }


        void Start()
        {
            CoreA = GameObject.Find("CoreA");
            CoreB = GameObject.Find("CoreB");
        }

        void Update()
        {
            if(CoreA == null) CoreA = GameObject.Find("CoreA");
            if (CoreB == null) CoreB = GameObject.Find("CoreB");
        }

        

        /// <summary>
        /// Dados el identificador del equipo y la posicion inicial de la entidad a spawnear retorna una lista de posiciones hasta el núcleo enemigo; sino hay path retorna una lista vacía.
        /// </summary>
        /// <param name="myTeam">Identificador del equipo. (Global.TeamId) </param>
        /// <param name="mySpawnPosition">Posicion inicial de la entidad a spawnear.</param>
        /// <returns></returns>
        public List<Vector3> GetCheckpointListPath(int myTeam, Vector3 mySpawnPosition)
        {
            List<Vector3> path = new List<Vector3>();

            Pair<int, int> spawnPosition = FindMapGridIndexes(mySpawnPosition);
            if (spawnPosition.First == -1 || spawnPosition.Second == -1) return path;
            if (myTeam == Global.TeamId.A)
            {
                path.Add(MapGrid[spawnPosition.First][spawnPosition.Second]);
                int columnPosition = spawnPosition.Second;
                if (spawnPosition.First > 0)
                {
                    for (int i = spawnPosition.First - 1; i >= 0; --i)
                    {
                        columnPosition = GetNextRandomColumn(columnPosition);
                        path.Add(MapGrid[i][columnPosition]);
                    }
                }
                path.Add(CoreB.transform.position);
            }
            else
            {
                path.Add(MapGrid[spawnPosition.First][spawnPosition.Second]);
                int columnPosition = spawnPosition.Second;
                if (spawnPosition.First < _nrows - 1)
                {
                    for (int i = spawnPosition.First + 1; i < _nrows; ++i)
                    {
                        columnPosition = GetNextRandomColumn(columnPosition);
                        path.Add(MapGrid[i][columnPosition]);
                    }
                }
                path.Add(CoreA.transform.position);
            }

            return path;
        }

        private int GetNextRandomColumn(int columnPosition)
        {
            var newColumnPosition = Random.Range(columnPosition - Global.RandomPathOffset,
                columnPosition + Global.RandomPathOffset + 1);
            if (newColumnPosition < 0) return 0;
            if (newColumnPosition >= _ncols) return _ncols - 1;
            return newColumnPosition;
        }

        /// <summary>
        /// Dado un Vector3, busca si existe en el MapGrid actual y devuelve las posiciones de los índices en un Pair; sino lo encuentra retorna un Pair(-1,-1).
        /// </summary>
        /// <param name="vector3">Vector3 a buscar.</param>
        /// <returns>Posicion en los índices del MapGrid.</returns>
        private Pair<int, int> FindMapGridIndexes(Vector3 vector3)
        {
            vector3 = AdjustVectorToGridScope(vector3);
            int rowindex = MapGrid.FindIndex(v => v.Contains(vector3));
            if (rowindex == -1) return new Pair<int, int>(-1, -1);
            int colindex = MapGrid[rowindex].FindIndex(v => v.Equals(vector3));
            return new Pair<int, int>(rowindex, colindex);
        }

        /// <summary>
        /// Dada una posicion de una escena de unity representada con un Vector3 retorna un Vector3 normalizado para que cumpla las coordenadas en el ámbito del MapGrid actual.
        /// </summary>
        /// <param name="vector3"> Vector3 a normalizar.</param>
        /// <returns>Vector3 ajustado al ámbito del MapGrid.</returns>
        private Vector3 AdjustVectorToGridScope(Vector3 vector3)
        {
            vector3.x = Mathf.RoundToInt(vector3.x);
            vector3.x -= (_startPosition.x%Global.CheckpointCellWidth) - (vector3.x%Global.CheckpointCellWidth);
            vector3.y = 0.0f;
            vector3.z = Mathf.RoundToInt(vector3.z);
            vector3.z += (_startPosition.z%Global.CheckpointCellHeight) - (vector3.z%Global.CheckpointCellHeight);
            return vector3;
        }

        /// <summary>
        /// Rellena el MapGrid con todas las posiciones de los puntos de control (Checkpoints) segun el tamaño de las celdas.
        /// </summary>
        private void InitializeMapGrid()
        {
            _ncols = (Mathf.RoundToInt(_finalPosition.x) - Mathf.RoundToInt(_startPosition.x))/
                        Global.CheckpointCellWidth;
            _nrows = (Mathf.RoundToInt(_startPosition.z) - Mathf.RoundToInt(_finalPosition.z))/
                        Global.CheckpointCellHeight;

            MapGrid = new List<List<Vector3>>();

            List<Vector3> row = new List<Vector3>();

            Vector3 pos1 = _startPosition;
            for (int i = 0; i < _nrows; ++i)
            {
                for (int j = 0; j < _ncols; ++j)
                {
                    row.Add(pos1);
                    if (_debug) Instantiate(DebugCheckpointDot, pos1, DebugCheckpointDot.transform.rotation);
       
                    pos1.x += Global.CheckpointCellWidth;
                }
                MapGrid.Add(row);
                row = new List<Vector3>();
                pos1.x = _startPosition.x;
                pos1.z = pos1.z - Global.CheckpointCellHeight;
            }
        }

        /// <summary>
        /// Inicializa las variables privadas _startPosition y _finalPosition que indican las aristas inicial y final de la parte Walkable.
        /// </summary>
        private void InitializeGridPositions()
        {
            _startPosition = Vector3.zero;
            _finalPosition = Vector3.zero;
            _startPosition.x = WalkableZone.position.x - WalkableZone.localScale.x/2;
            _startPosition.z = WalkableZone.position.z + WalkableZone.localScale.z/2;
            _finalPosition.x = WalkableZone.position.x + WalkableZone.localScale.x/2;
            _finalPosition.z = WalkableZone.position.z - WalkableZone.localScale.z/2;
        }

        /// <summary>
        /// Dado el identificador de equipo y una posicion, retorna si se puede spawnear una entidad.
        /// </summary>
        /// <param name="myTeam">Identificador de equipo (Global.TeamId)</param>
        /// <param name="position">Vector3 indicando una posicion de una escena de Unity</param>
        /// <returns>true: Si que puede spawnear, false: no.</returns>
        public bool CanSpawn(int myTeam, Vector3 position)
        {
            var pair = FindMapGridIndexes(position);
            return !IsInvalidPair(pair) && IsMyLand(myTeam,pair.First) /*&& MapGridPositionIsEmpty(position)*/;
        }

        //TODO: No va bien, hacer raycast desde arriba y no desde la cámara
        private bool MapGridPositionIsEmpty(Vector3 position)
        {
            var origin = new Vector3(position.x,position.y + Global.CanSpawnOriginOffsetForRaycast, position.z);
            var direction = position - origin;
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(origin,direction, out hit, Global.CanSpawnOriginOffsetForRaycast,LayerMask.NameToLayer("Default"),QueryTriggerInteraction.Ignore))
            {
                return hit.transform.tag != "Unit";
            }
            return true;
        }

        private bool IsInvalidPair(Pair<int, int> pair)
        {
            Pair<int,int> invalidPair = new Pair<int, int>() {First = -1, Second = -1};
            return pair.First == invalidPair.First || pair.Second == invalidPair.Second;
        }

        private bool IsMyLand(int myTeam, int rowIndex)
        {
            if (myTeam == Global.TeamId.A)
            {
                if (rowIndex < _nrows && rowIndex >= _nrows-1 -_nrows*Global.LandTeamPercentage) return true;
            }
            else if (myTeam == Global.TeamId.B)
            {
                if (rowIndex >= 0 && rowIndex <= _nrows*Global.LandTeamPercentage) return true;
            }
            return false;
        }
    }   
}