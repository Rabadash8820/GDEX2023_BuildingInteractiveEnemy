using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

namespace SpaceGame
{
    public class MonsterAiManager : MonoBehaviour
    {
        private PolygonCollider2D _regionWithPlayer;

        [Required] public PerRegionSpawner PerRegionSpawner;
        [Required] public Transform PlayerTransform;

        public float PlayerFollowDistance = 8f;

        private void Awake()
        {
            PerRegionSpawner.InstanceSpawned.AddListener(initializeSpawnedInstance);
            MonsterRegionListener[] monsterRegionListeners = PerRegionSpawner.RegionsParent.GetComponentsInChildren<MonsterRegionListener>();
            foreach (var listener in monsterRegionListeners)
            {
                listener.TriggerEnterred += (sender, e) => handleColliderEnterringRegion(e.RegionTriggerCollider, e.TriggeringCollider);
                listener.TriggerExited += (sender, e) => handleColliderExitingRegion(e.RegionTriggerCollider, e.TriggeringCollider);
            }
        }

        public void handleColliderEnterringRegion(PolygonCollider2D region, Collider2D collider)
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
            monster.RigidbodyFollower.TransformToFollow = collider.attachedRigidbody.transform;
            monster.RigidbodyFollower.FollowDistance = PlayerFollowDistance;
        }

        public void handleColliderExitingRegion(PolygonCollider2D region, Collider2D collider)
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
            monster.RigidbodyFollower.TransformToFollow = spawnPointToGuard;
            monster.RigidbodyFollower.FollowDistance = 0f;
        }

        private void initializeSpawnedInstance(Transform instance, Transform spawnPoint, PolygonCollider2D region)
        {
            var monster = instance.GetComponent<Monster>();
            if (!monster)
                return;

            monster.SpawnPoint = spawnPoint;
            var eyeLookAts = instance.GetComponentsInChildren<LookAt2D>(includeInactive: true);
            foreach (LookAt2D lookAt in eyeLookAts)
                lookAt.LookAtTransform = PlayerTransform;
        }
    }
}
