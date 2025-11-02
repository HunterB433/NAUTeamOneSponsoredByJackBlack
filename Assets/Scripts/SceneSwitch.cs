using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneName; // Name of the scene to load

    // Public function you can call to switch scenes
    public void SwitchScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Switching to scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name not set in the inspector.");
        }
    }
}
