using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // Singleton instance

    public AudioSource musicSource; // Reference to the AudioSource component

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the music across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate MusicManager instances
        }
    }

    private void Start()
    {
        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene change events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the main menu
        if (scene.name == "Main Menu")
        {
            StopMusic();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
