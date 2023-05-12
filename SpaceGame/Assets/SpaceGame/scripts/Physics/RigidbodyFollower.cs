using System;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public enum OffsetType
    {
        LocalPosition,
        Distance,
    }

    [Serializable]
    public class FollowDistanceEvent
    {
        [MinValue(0d)] public float Distance;
        public UnityEvent NowAtDistanceOrCloser = new();
        public UnityEvent NowFurther = new();
        [NonSerialized] public bool AtDistanceOrCloser;
    }

    public class RigidbodyFollower : MonoBehaviour
    {
        [Required] public Rigidbody2D RigidbodyToMove;
        public Transform TransformToFollow;
        public OffsetType OffsetType;

        [field: ShowInInspector, ReadOnly, LabelText(nameof(CurrentDistance))]
        public float CurrentDistance { get; private set; }

        [Tooltip(
            "How " + nameof(RigidbodyToMove) + "'s speed falls off with distance. " +
            "X-axis is physical distance, y-axis is fraction of " + nameof(MaxSpeed) + ". " +
            "Use a flat curve for constant speed. " +
            "Use a positively sloped curve to slow down while approaching the target offset."
        )]
        public AnimationCurve SpeedDistanceFalloffCurve = AnimationCurve.Constant(timeStart: 0f, timeEnd: 1f, value: 1f);

        public bool UseFiniteSpeed = false;

        [Tooltip("Actual speed at any moment may be less, depending on distance and the " + nameof(SpeedDistanceFalloffCurve))]
        [ShowIf(nameof(UseFiniteSpeed))]
        public float MaxSpeed = 1f;

        [ShowIf("@" + nameof(OffsetType) + " == " + nameof(SpaceGame) + "." + nameof(SpaceGame.OffsetType) + "." + nameof(OffsetType.LocalPosition))]
        public Vector2 FollowOffset;

        [ShowIf("@" + nameof(OffsetType) + " == " + nameof(SpaceGame) + "." + nameof(SpaceGame.OffsetType) + "." + nameof(OffsetType.Distance))]
        [MinValue(0d)]
        public float FollowDistance;

        [ShowIf("@" + nameof(OffsetType) + " == " + nameof(SpaceGame) + "." + nameof(SpaceGame.OffsetType) + "." + nameof(OffsetType.Distance))]
        public FollowDistanceEvent[] FollowDistanceEvents;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void FixedUpdate()
        {
            if (TransformToFollow == null)
                return;

            switch (OffsetType)
            {
                case OffsetType.LocalPosition:
                    RigidbodyToMove.position = (Vector2)TransformToFollow.TransformPoint(FollowOffset);
                    break;

                case OffsetType.Distance:
                    Vector2 vectorBetween = (Vector2)TransformToFollow.position - RigidbodyToMove.position;
                    CurrentDistance = vectorBetween.magnitude;

                    float signedDistToOffset = CurrentDistance - FollowDistance;
                    float distToOffset = Mathf.Abs(signedDistToOffset);
                    Vector2 moveDir = Mathf.Sign(signedDistToOffset) * vectorBetween / CurrentDistance;
                    float currSpeed = SpeedDistanceFalloffCurve.Evaluate(distToOffset) * MaxSpeed;
                    float distToMove = Mathf.Min(currSpeed * Time.fixedDeltaTime, distToOffset);
                    RigidbodyToMove.MovePosition(RigidbodyToMove.position + distToMove * moveDir);

                    for (int x = 0; x < FollowDistanceEvents.Length; x++)
                    {
                        FollowDistanceEvent followDistEvent = FollowDistanceEvents[x];
                        bool wasAtOrCloser = followDistEvent.AtDistanceOrCloser;
                        if (CurrentDistance <= followDistEvent.Distance && !wasAtOrCloser)
                        {
                            followDistEvent.AtDistanceOrCloser = true;
                            followDistEvent.NowAtDistanceOrCloser.Invoke();
                        }
                        else if (CurrentDistance > followDistEvent.Distance && wasAtOrCloser)
                        {
                            followDistEvent.AtDistanceOrCloser = false;
                            followDistEvent.NowFurther.Invoke();
                        }
                    }

                    break;

                default:
                    return;
            }
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDrawGizmos()
        {
            foreach (FollowDistanceEvent followDistEvent in FollowDistanceEvents)
                Gizmos.DrawWireSphere(RigidbodyToMove.position, followDistEvent.Distance);
        }
    }
}
