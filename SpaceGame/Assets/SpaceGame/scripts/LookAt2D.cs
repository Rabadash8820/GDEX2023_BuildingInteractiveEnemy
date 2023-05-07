using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class LookAt2D : MonoBehaviour
    {
        [Required] public Transform LookAtTransform;
        [Required] public Transform LookerTransform;

        private void Update()
        {
            LookerTransform.right = LookAtTransform.position - LookerTransform.position;
        }
    }
}
