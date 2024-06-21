using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.gdcbd.bossbattle.components
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