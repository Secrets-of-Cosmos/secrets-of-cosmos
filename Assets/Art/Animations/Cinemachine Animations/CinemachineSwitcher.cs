using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary Sitting to Idle Transition
        if (Input.GetKey(KeyCode.K))
        {
            animator.Play("Third Person");
        }   
    }
}
