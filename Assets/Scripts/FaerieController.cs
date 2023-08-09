using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaerieController : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        //animator = gameObject.GetComponent<Animator>();        
        animator.SetTrigger("Fly");
        InvokeRepeating("Animate", 2.0f, 2.0f);
    }

    void Animate()
    {
        animator.SetTrigger("Fly");
    }
}
