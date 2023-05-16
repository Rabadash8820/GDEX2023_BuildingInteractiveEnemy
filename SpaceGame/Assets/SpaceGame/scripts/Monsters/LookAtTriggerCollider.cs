using Sirenix.OdinInspector;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    [RequireComponent(typeof(Collider2D))]
    public class LookAtTriggerCollider : MonoBehaviour
    {
        private const string TOOLTIP_DESIRED_TARGET =
            "If true and " + nameof(DesiredTarget) + " is non-null, then collision events will only be invoked if " +
            "the trigger collider equals " + nameof(DesiredTarget) + ". " +
            "If true and " + nameof(DesiredTarget) + " is null (e.g., after " + nameof(DesiredTarget) + " is destroyed, then " +
            "collision events will never be invoked. " +
            "If false, then " + nameof(DesiredTarget) + " is ignored and all colliding triggers will invoke collision events. " +
            "Likewise, " + nameof(LookAts) + " are only adjusted if a collision event is invoked. " +
            "All of this is useful, e.g., when ignoring all colliders besides those of the player";

        [Tooltip(TOOLTIP_DESIRED_TARGET)]
        public bool UseDesiredTarget = false;

        [Tooltip(TOOLTIP_DESIRED_TARGET)]
        [ShowIf(nameof(UseDesiredTarget))]
        public Transform DesiredTarget;

        [Tooltip("These " + nameof(LookAt2D) + "s will be adjusted to point at the latest triggering collider")]
        public LookAt2D[] LookAts = Array.Empty<LookAt2D>();

        private const string TOOLTIP_DESIRED_TARGET_COLLISION =
            "If " + nameof(DesiredTarget) + " is non-null, then this event is only invoked if the triggering collider is attached to that transform.";

        [Tooltip("Invoked when a collider comes in range (i.e., enters the attached trigger collider). " + TOOLTIP_DESIRED_TARGET_COLLISION)]
        public UnityEvent ColliderInRange = new();

        [Tooltip("Invoked when a collider goes out of range (i.e., exits the attached trigger collider). " + TOOLTIP_DESIRED_TARGET_COLLISION)]
        public UnityEvent ColliderOutOfRange = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Transform triggeringTransform = collision.attachedRigidbody.transform;
            if (tryAdjustLookAts(triggeringTransform, newTargetTransform: triggeringTransform))
                ColliderInRange.Invoke();
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnTriggerExit2D(Collider2D collision)
        {
            Transform triggeringTransform = collision.attachedRigidbody.transform;
            if (tryAdjustLookAts(triggeringTransform, newTargetTransform: null))
                ColliderOutOfRange.Invoke();
        }

        private bool tryAdjustLookAts(Transform triggeringTransform, Transform newTargetTransform)
        {
            if (UseDesiredTarget && (DesiredTarget == null || triggeringTransform != DesiredTarget))
                return false;

            for (int x = 0; x < LookAts.Length; x++) {
                LookAt2D lookAt = LookAts[x];
                lookAt.LookAtTransform = newTargetTransform;
            }

            return true;
        }
    }
}
