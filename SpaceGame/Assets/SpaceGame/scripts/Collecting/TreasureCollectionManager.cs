using Sirenix.OdinInspector;
using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceGame
{
    [Serializable]
    public class TreasureCountEvent
    {
        [MinValue(0d)] public int TreasureCount;
        public UnityEvent NowAtCountOrHigher;
        public UnityEvent NowLower;
        [NonSerialized] public bool AtCountOrHigher;
    }

    [RequireComponent(typeof(Collider2D))]
    public class TreasureCollectionManager : MonoBehaviour
    {
        private TreasureCollectible _collectedTreasure;

        [Required] public Transform PlayerTransform;
        [Required] public TMP_Text TxtTreasureCount;
        public string TreasureCountFormatString = "Treasures: {0}/{1}";
        [Required] public TMP_Text TxtCollectedTreasure;
        public Vector2 CollectedPlayerOffset = new(-2f, 0f);
        public UnityEvent TreasureCollected = new();

        [ListDrawerSettings(Expanded = true)]
        public TreasureCountEvent[] TreasureCountEvents;

        [MinValue(0d)]
        public int ReturnedTreasuresToWin = 5;
        public Image ImgTreasure;
        public UnityEvent TreasureReturned = new();
        public UnityEvent AllTreasureReturned = new();

        [field: ShowInInspector]
        public int ReturnedCount { get; private set; } = 0;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Start() => TxtTreasureCount.text = string.Format(TreasureCountFormatString, 0, ReturnedTreasuresToWin);

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.attachedRigidbody.TryGetComponent(out TreasureCollectible treasure) || treasure != _collectedTreasure)
                return;

            ReturnTreasure(treasure);
        }

        public bool CollectTreasure(TreasureCollectible treasure)
        {
            if (_collectedTreasure != null) {
                Debug.Log("Player has already collected a treasure, cannot collect more.");
                return false;
            }

            _collectedTreasure = treasure;
            TxtCollectedTreasure.text = treasure.Name;
            TxtCollectedTreasure.gameObject.SetActive(true);
            treasure.RigidbodyFollower.TransformToFollow = PlayerTransform;

            TreasureCollected.Invoke();

            return true;
        }

        public void ReturnTreasure(TreasureCollectible treasure)
        {
            TxtTreasureCount.text = string.Format(TreasureCountFormatString, ++ReturnedCount, ReturnedTreasuresToWin);
            if (ImgTreasure != null)
                ImgTreasure.sprite = treasure.Sprite;
            Destroy(treasure.gameObject);
            _collectedTreasure = null;
            TreasureReturned.Invoke();

            foreach (TreasureCountEvent treasureCountEvent in TreasureCountEvents) {
                bool wasAtOrHigher = treasureCountEvent.AtCountOrHigher;
                if (ReturnedCount >= treasureCountEvent.TreasureCount && !wasAtOrHigher) {
                    treasureCountEvent.AtCountOrHigher = true;
                    treasureCountEvent.NowAtCountOrHigher.Invoke();
                }
                else if (ReturnedCount < treasureCountEvent.TreasureCount && wasAtOrHigher) {
                    treasureCountEvent.AtCountOrHigher = false;
                    treasureCountEvent.NowLower.Invoke();
                }
            }
        }

        [Button]
        public void CheckIfAllReturned()
        {
            if (ReturnedCount == ReturnedTreasuresToWin) {
                Debug.Log($"All {ReturnedTreasuresToWin} treasures have been returned");
                AllTreasureReturned.Invoke();
            }
        }
    }
}
