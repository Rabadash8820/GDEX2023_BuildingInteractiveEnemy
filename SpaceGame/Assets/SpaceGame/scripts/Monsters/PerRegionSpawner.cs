using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;
using U = UnityEngine;
using UnityEngine.Events;
using System.Diagnostics.CodeAnalysis;

namespace SpaceGame
{
    public class PerRegionSpawner : MonoBehaviour
    {
        private PolygonCollider2D[] _regions;
        private readonly Dictionary<int, List<Transform>> _regionSpawnPoints = new();
        private readonly Dictionary<int, Transform> _regionSpawnedInstances = new();

        [Required, AssetsOnly]
        public GameObject PrefabToSpawn;
        [Required, SceneObjectsOnly] public Transform RegionsParent;

        public UnityEvent<Transform, Transform, PolygonCollider2D> InstanceSpawned;

        public bool TryGetSpawnedInstanceInRegion(PolygonCollider2D region, out Transform instance) =>
            _regionSpawnedInstances.TryGetValue(region.GetInstanceID(), out instance)
            && instance != null;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Awake()
        {
            _regions = RegionsParent.GetComponentsInChildren<PolygonCollider2D>();
            foreach (PolygonCollider2D region in _regions)
            {
                U.Debug.Log($"Saving region '{region.name}' with {region.transform.childCount} child spawn points...");
                int regionId = region.GetInstanceID();
                foreach (Transform spawnPoint in region.transform)
                {
                    if (!_regionSpawnPoints.TryGetValue(regionId, out List<Transform> regionSpawnPoints))
                    {
                        regionSpawnPoints = new List<Transform>();
                        _regionSpawnPoints.Add(regionId, regionSpawnPoints);
                    }
                    regionSpawnPoints.Add(spawnPoint);
                }
                _regionSpawnedInstances.Add(regionId, null);
            }
        }

        [Button]
        public void SpawnInEachRegion()
        {
            foreach (PolygonCollider2D region in _regions)
            {

                int regionId = region.GetInstanceID();
                if (_regionSpawnedInstances[regionId] != null)
                {
                    U.Debug.Log($"Monster already spawned in region '{region.name}'...");
                    continue;
                }

                U.Debug.Log($"Spawning monster in region '{region.name}'...");

                // Pick a random spawn point at which to instantiate the prefab
                List<Transform> regionSpawnPoints = _regionSpawnPoints[regionId];
                int spawnPointIndex = Random.Range(0, regionSpawnPoints.Count);
                Transform spawnPoint = regionSpawnPoints[spawnPointIndex];

                Transform instance = Instantiate(PrefabToSpawn, spawnPoint).transform;
                _regionSpawnedInstances[regionId] = instance;

                InstanceSpawned.Invoke(instance, spawnPoint, region);
            }
        }

        [Button, Conditional("UNITY_EDITOR")]
        public void ClearSpawnedInstances()
        {
            foreach (PolygonCollider2D region in _regions)
            {
                U.Debug.Log($"Destroying spawned monster in region '{region.name}', if one exists...");

                int regionId = region.GetInstanceID();

                Destroy(_regionSpawnedInstances[regionId].gameObject);
                _regionSpawnedInstances[regionId] = null;
            }
        }
    }
}
