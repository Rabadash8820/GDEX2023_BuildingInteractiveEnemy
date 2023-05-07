using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame
{
    public class SceneRestarter : MonoBehaviour
    {
        public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
