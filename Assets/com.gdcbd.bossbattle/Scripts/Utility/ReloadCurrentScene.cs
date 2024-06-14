using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossBattle.Utility
{
    public class ReloadScene : MonoBehaviour
    {
        public void ReloadCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}