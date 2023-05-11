using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class TimeManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        public float CurrentTimeScale => Time.timeScale;

        [Button]
        public void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
    }
}
