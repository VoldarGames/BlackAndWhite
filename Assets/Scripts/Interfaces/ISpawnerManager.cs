using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISpawnerManager
    {
        GameObject[] SpawnableGameObjectCatalogA { get; set; }
        GameObject[] SpawnableGameObjectCatalogB { get; set; }
        int SpawnableSelected { get; set; }
        Vector3 GetClickPosition();
        void ChangeGhostModelMaterial();
        void ChangeSpawnableSelected();




    }
}
