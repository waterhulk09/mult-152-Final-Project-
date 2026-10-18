using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class SceneLoader : MonoBehaviour
{
    // Method to load a scene by its exact text name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Method to load a scene by its index number in Build Settings
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
