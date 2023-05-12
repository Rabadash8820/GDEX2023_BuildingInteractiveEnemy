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
        public UnityEvent NowAtCountOrHigher = new();
        public UnityEvent NowLower = new();
        [NonSerialized] public bool AtCountOrHigher;
    }

    [RequireComponent(typeof(Collider2D))]
    public class TreasureReturnManager : MonoBehaviour
    {
        [Required] public TreasureCollector TreasureCollector;
        [Required] public TMP_Text TxtTreasureCount;
        public string TreasureCountFormatString = "Treasures: {0}/{1}";

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
            if (!collider.attachedRigidbody.TryGetComponent(out TreasureCollectible treasure) || treasure != TreasureCollector!.CollectedTreasure)
                return;

            ReturnTreasure(treasure);
        }

        public void ReturnTreasure(TreasureCollectible treasure)
        {
            if (treasure == null)
                throw new ArgumentNullException(nameof(treasure));

            Debug.Log($"Player returning treasure '{treasure.Description}'. Returned count is now {++ReturnedCount}");

            TreasureCollector.Drop(treasure);

            TxtTreasureCount.text = string.Format(TreasureCountFormatString, ReturnedCount, ReturnedTreasuresToWin);
            if (ImgTreasure != null)
                ImgTreasure.sprite = treasure.Sprite;
            Destroy(treasure.gameObject);
            TreasureReturned.Invoke();

            for (int x = 0; x < TreasureCountEvents.Length; x++) {
                TreasureCountEvent treasureCountEvent = TreasureCountEvents[x];

                bool wasAtOrHigher = treasureCountEvent.AtCountOrHigher;
                if (ReturnedCount >= treasureCountEvent.TreasureCount && !wasAtOrHigher) {
                    Debug.Log($"Invoking {nameof(TreasureCountEvent.NowAtCountOrHigher)} event of {nameof(TreasureCountEvent)} at index {x}...");
                    treasureCountEvent.AtCountOrHigher = true;
                    treasureCountEvent.NowAtCountOrHigher.Invoke();
                }
                else if (ReturnedCount < treasureCountEvent.TreasureCount && wasAtOrHigher) {
                    Debug.Log($"Invoking {nameof(TreasureCountEvent.NowLower)} event of {nameof(TreasureCountEvent)} at index {x}...");
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
