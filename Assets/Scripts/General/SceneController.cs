using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Start()
    {
        GameEvents.current.onSceneLoad += LoadScene;
    }

    private void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
    }
}
