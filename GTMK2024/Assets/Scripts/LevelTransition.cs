using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class LevelTransition : MonoBehaviour
{
    public string nextSceneName; // Name of the scene to load

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Ensure it's the player
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Load the next scene by name
        SceneManager.LoadScene(nextSceneName);
    }
}
