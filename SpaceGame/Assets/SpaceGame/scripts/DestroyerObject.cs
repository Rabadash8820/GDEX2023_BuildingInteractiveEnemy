using UnityEngine;

namespace SpaceGame
{
    [CreateAssetMenu(menuName = nameof(SpaceGame) + "/" + nameof(DestroyerObject), fileName = "destroyer")]
    public class DestroyerObject : ScriptableObject
    {
        public new void Destroy(Object target) => Object.Destroy(target);
        public new void DestroyImmediate(Object target) => Object.DestroyImmediate(target);
    }
}
