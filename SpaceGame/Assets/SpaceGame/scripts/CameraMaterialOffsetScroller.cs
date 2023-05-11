using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SpaceGame
{
    public class CameraMaterialOffsetScroller : MonoBehaviour
    {
        [Required] public Transform CameraTransform;
        [Required] public MeshRenderer MeshRenderer;

        [Tooltip("The parallax value for the material offset. Lower float = more visual movement.")]
        public float Parallax = 500f;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Update()
        {
            MeshRenderer.material.mainTextureOffset = new Vector2(
                CameraTransform.position.x / CameraTransform.localScale.x / Parallax,
                CameraTransform.position.y / CameraTransform.localScale.y / Parallax
            );
        }
    }
}
