using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace SpaceGame
{
    public class MonsterAiManager : MonoBehaviour
    {
        private PolygonCollider2D _regionWithPlayer;

        [Required] public PerRegionSpawner PerRegionSpawner;
        [Required] public Transform PlayerTransform;

        public float PlayerFollowDistance = 8f;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Awake()
        {
            PerRegionSpawner.InstanceSpawned.AddListener(initializeSpawnedInstance);
            MonsterRegionListener[] monsterRegionListeners = PerRegionSpawner.RegionsParent.GetComponentsInChildren<MonsterRegionListener>();
            foreach (MonsterRegionListener listener in monsterRegionListeners)
            {
                listener.TriggerEnterred += (sender, e) => handleColliderEnterringRegion(e.RegionTriggerCollider, e.TriggeringCollider);
                listener.TriggerExited += (sender, e) => handleColliderExitingRegion(e.RegionTriggerCollider, e.TriggeringCollider);
            }
        }

        private void handleColliderEnterringRegion(PolygonCollider2D region, Collider2D collider)
        {
            if (collider.attachedRigidbody.transform != PlayerTransform)
                return;

            if (_regionWithPlayer == region)
            {
                Debug.LogError($"Player enterred region '{region.name}' more than once?");
                return;
            }

            Debug.Log($"Player enterred region '{region.name}'");

            _regionWithPlayer = region;

            if (
                !PerRegionSpawner.TryGetSpawnedInstanceInRegion(region, out Transform instance) ||
                !instance.TryGetComponent(out Monster monster)
            )
            {
                Debug.Log($"No monster in region '{region.name}'");
                return;
            }

            Debug.Log($"Monster is now following at a distance of {PlayerFollowDistance} units");
            monster.Rigidbody2DTransformFollower.TransformToFollow = collider.attachedRigidbody.transform;
            monster.Rigidbody2DTransformFollower.FollowDistance = PlayerFollowDistance;
        }

        private void handleColliderExitingRegion(PolygonCollider2D region, Collider2D collider)
        {
            if (collider.attachedRigidbody.transform != PlayerTransform)
                return;

            if (_regionWithPlayer == null)
            {
                Debug.LogError($"Player exited region '{region.name}' without first enterring it?");
                return;
            }
            if (_regionWithPlayer != region)
            {
                Debug.LogError($"Player exited region '{region.name}' but was in region '{_regionWithPlayer.name}'?");
                return;
            }

            Debug.Log($"Player exited region '{region.name}'");

            _regionWithPlayer = null;

            if (
                !PerRegionSpawner.TryGetSpawnedInstanceInRegion(region, out Transform instance) ||
                !instance.TryGetComponent(out Monster monster)
            )
            {
                Debug.Log($"There was no monster in region '{region.name}'");
                return;
            }

            // Return the monster to the closest spawn point, to "guard" it
            Transform spawnPointToGuard = region.transform
                .Cast<Transform>()
                .OrderBy(x => (x.position - monster.transform.position).sqrMagnitude)
                .First();
            Debug.Log($"Monster is now returning to guard spawn point '{spawnPointToGuard.name}'");
            monster.GuardPoint = spawnPointToGuard;
            monster.transform.SetParent(spawnPointToGuard, worldPositionStays: true);
            monster.Rigidbody2DTransformFollower.TransformToFollow = spawnPointToGuard;
            monster.Rigidbody2DTransformFollower.FollowDistance = 0f;
        }

        private void initializeSpawnedInstance(Transform instance, Transform spawnPoint, PolygonCollider2D region)
        {
            Monster monster = instance.GetComponent<Monster>();
            if (!monster)
                return;

            monster.SpawnPoint = spawnPoint;
            monster.LaserTriggerCollider.DesiredTarget = PlayerTransform;
        }
    }
}
