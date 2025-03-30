using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTo : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isCarry", false); 
    }
}
