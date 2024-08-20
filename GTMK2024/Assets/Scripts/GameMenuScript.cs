using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    public void PauseGame(bool pause)
    {
        player.SetPaused(pause);
        
        if(!pause)
            animator.SetTrigger("CloseMenu");
        else
        {
            animator.SetTrigger("OpenMenu");
        }
            
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
