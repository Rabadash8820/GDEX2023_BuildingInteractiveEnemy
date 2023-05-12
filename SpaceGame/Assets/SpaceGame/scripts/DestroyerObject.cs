using UnityEngine;

namespace SpaceGame
{
    [CreateAssetMenu(menuName = nameof(SpaceGame) + "/" + nameof(DestroyerObject), fileName = "destroyer")]
    public class DestroyerObject : ScriptableObject
    {
        public new void Destroy(Object target) => Destroy(target);
        public new void DestroyImmediate(Object target) => DestroyImmediate(target);
    }
}
