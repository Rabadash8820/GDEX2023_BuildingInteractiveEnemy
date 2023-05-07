using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class AtTransformSpawner : MonoBehaviour
    {
        [Required] public Transform SpawnPoint;
        [Required, AssetsOnly] public GameObject Prefab;

        public void Spawn() => Instantiate(Prefab, SpawnPoint.position, SpawnPoint.rotation);
    }
}
