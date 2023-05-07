using UnityEngine;

namespace SpaceGame
{
    public class ApplicationQuitter : MonoBehaviour
    {
        public void Quit() => Application.Quit();
        public void Quit(int exitCode) => Application.Quit(exitCode);
    }
}
