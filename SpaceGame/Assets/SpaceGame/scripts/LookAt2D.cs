using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SpaceGame
{
    public class LookAt2D : MonoBehaviour
    {
        [Required] public Transform LookerTransform;
        public Transform LookAtTransform;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Update()
        {
            if (LookAtTransform != null)
                LookerTransform.right = LookAtTransform.position - LookerTransform.position;
        }
    }
}
