using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class IntroManager : MonoBehaviour
    {
        private static bool s_alreadyShown; // Persists between scene reloads (but shared by all instances!)

        public bool ShowIntro = true;

        public UnityEvent ShowingIntro;
        public UnityEvent NotShowingIntro;

        public void TryShowIntro() {
            bool show = ShowIntro && !s_alreadyShown;
            Debug.Log($"{(show ? "Showing" : "Not showing")} intro ({nameof(ShowIntro)}={ShowIntro}, {nameof(s_alreadyShown)}={s_alreadyShown})...");

            (show ? ShowingIntro : NotShowingIntro).Invoke();
            s_alreadyShown = show;
        }
    }
}
