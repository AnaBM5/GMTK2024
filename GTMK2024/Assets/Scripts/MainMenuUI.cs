using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuInterface;
    [SerializeField] private GameObject levelSelectInterface;
    [SerializeField] private Transform background;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimationTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
