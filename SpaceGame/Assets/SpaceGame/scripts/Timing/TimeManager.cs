using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class TimeManager : MonoBehaviour
    {
        [Button]
        public void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
    }
}
